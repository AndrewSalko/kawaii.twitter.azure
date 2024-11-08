using System;
using System.Net.Http;
using System.Data;
using System.Linq;
using kawaii.twitter.blob;
using kawaii.twitter.core.HtmlParsers;
using kawaii.twitter.core.SelectLogic;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using kawaii.twitter.core.SelectLogic.Images;
using kawaii.twitter.core.SelectLogic.Page;
using kawaii.twitter.core.Text;
using kawaii.twitter.db;
using kawaii.twitter.db.TweetDateUpdaters;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.core.SelectLogic.URL;
using kawaii.twitter.core.SelectLogic.PageOrExternalImage;
using kawaii.twitter.core.SelectLogic.Images.ExcludeUsed;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.SpecialDay;

namespace kawaii.twitter.azure.func
{
    /// <summary>
    /// Azure function
    /// </summary>
    public static class TweetPostFunction
    {
		static HttpClient _HttpClient = new HttpClient();

		[FunctionName("TweetPostFunction")]
		public static async Task Run([TimerTrigger("0 0 */2 * * *")] TimerInfo timer, ILogger log)
		{
			//https://docs.microsoft.com/ru-ru/azure/azure-functions/functions-bindings-timer?tabs=csharp#ncrontab-expressions
			//0 */5 * * * * - каждые 5 мин
			//"0 0 * * * *"	- каждый час
			//0 0 */2 * * * - каждый второй час

			Logger logger = new Logger(log);

			logger.Log($"TweetPostFunction executed at: {DateTime.Now}");

			//await _TestHttp(logger);
			//return;//!!!

			string animatedBlobConnectionString = kawaii.twitter.core.Env.EnvironmentSecureData.GetValueFromEnvironment("env:kawaii_twitter_azure_animatedblob");
			string azureSiteDBConnectionString = kawaii.twitter.core.Env.EnvironmentSecureData.GetValueFromEnvironment("env:kawaii_twitter_azure_sitepages");

			if (string.IsNullOrEmpty(animatedBlobConnectionString))
			{
				logger.LogError("animatedBlobConnectionString not found!");
				return;
			}

			if (string.IsNullOrEmpty(azureSiteDBConnectionString))
			{
				logger.LogError("azureSiteDBConnectionString not found!");
				return;
			}

			IDatabase database = new Database(azureSiteDBConnectionString, true, null);

			logger.Log($"database connected: {DateTime.Now}");

			//если в переменных задано что "не обновлять индексы", то мы сэкономим время работы в прод-окружении - индексы создаются один раз,
			//и дальше и так работают, хотя MongoDB и говорит что "нет проблем".
			//Если будет нужно обновлять индексы, или будет новое окружение (новая база, коллекция и прочее) то эту переменную среды надо убрать.
			//А когда все настроено и уже работает, ее можно создать для общего ускорения
			string dontCreateIndexesStr = kawaii.twitter.core.Env.EnvironmentSecureData.GetValueFromEnvironment("env:kawaii_twitter_dont_create_indexes");
			bool dontCreateIndexes = false;
			if (!string.IsNullOrEmpty(dontCreateIndexesStr))
			{
				bool.TryParse(dontCreateIndexesStr, out dontCreateIndexes);
			}

			if (dontCreateIndexes)
			{
				logger.Log("Indexes will not updated (found env:kawaii_twitter_dont_create_indexes)");
			}

			AnimatedImageCollection animatedImageCollection = new AnimatedImageCollection(database, null, !dontCreateIndexes);
			var imagesCollection = animatedImageCollection.AnimatedImages;

			logger.Log($"imagesCollection init done: {DateTime.Now}");

			SitePageCollection sitePageCollection = new SitePageCollection(database, null, !dontCreateIndexes);
			var sitePagesCollection = sitePageCollection.SitePages;

			logger.Log($"sitePagesCollection init done: {DateTime.Now}");


			//обновляем (при необходимости) посты из карты сайта
			UpdateRecentPosts recentPostsUpdater = new UpdateRecentPosts();
			if (recentPostsUpdater.UpdateTimeReached)
			{
				logger.Log($"UpdateFromSiteMap: {DateTime.Now}");

				await recentPostsUpdater.UpdateFromSiteMap(_HttpClient, logger, database, dontCreateIndexes, UpdateRecentPosts.RECENT_POSTS_COUNT);

				logger.Log($"UpdateFromSiteMap done: {DateTime.Now}");
			}


			ITwitterTextCreator textCreator = new kawaii.twitter.core.Text.TwitterTextCreator();
			IImageOnWeb imageOnWeb = new kawaii.twitter.core.HtmlParsers.ImageOnWeb(_HttpClient);
			ITwitterImageURL twitterImageURL = new kawaii.twitter.core.HtmlParsers.TwitterImageURL(_HttpClient);

			//--- блок для lastTweetUpdater
			var dateSupply = new kawaii.twitter.core.Env.DateSupply();

			IAnimatedTweetDateUpdater animatedTweetDateUpdater = new AnimatedTweetDateUpdater(imagesCollection);
			ISitePageTweetDateUpdater sitePageTweetDateUpdater = new SitePageTweetDateUpdater(sitePagesCollection);
			ILastTweetUpdater lastTweetUpdater = new kawaii.twitter.core.SelectLogic.LastTweetUpdater(dateSupply, animatedTweetDateUpdater, sitePageTweetDateUpdater);
			//--- 

			//сервису твиттера нужно передать авториз.данные
			var service = new kawaii.twitter.core.TwitterService.Service();

			var animatedImagesBlobContainer = new AnimatedImagesBlobContainer(animatedBlobConnectionString);
			IBlobDownload blobDownload = animatedImagesBlobContainer;

			var randomSelector = new kawaii.twitter.core.SelectLogic.Randomize.RandomSelector();

			int topQueryCount = 10;

			ISpecialDaySelector specialDaySelector = new SpecialDaySelector(dateSupply, randomSelector);
			string specialDayName = specialDaySelector.DetectSpecialDayName();

			IFolderFromURL folderFromURL = new kawaii.twitter.core.SelectLogic.URL.FolderFromURL();
			var formatter = new kawaii.twitter.core.SelectLogic.BlobName.Formatter();

			IPageSelector pageSelectorForNewPages = new kawaii.twitter.core.SelectLogic.Page.NotTwittedPages(sitePagesCollection, randomSelector, specialDayName, topQueryCount);
			IFindAnimatedByPage findNewAnimatedByPage = new kawaii.twitter.core.SelectLogic.Images.Newly.NotTwittedAnimated(imagesCollection, randomSelector, topQueryCount, folderFromURL, formatter);

			IPageSelector pageSelectorForAnyPages = new kawaii.twitter.core.SelectLogic.Page.PageSelector(sitePagesCollection, randomSelector, specialDayName, topQueryCount);

			IFindAnimatedByPage findAnimatedByPage = new kawaii.twitter.core.SelectLogic.Images.Find.FindAnimatedByPage(imagesCollection.AsQueryable(), folderFromURL, formatter);

			IPageOrExternalImageSelector pageOrExternalImageSelector = new kawaii.twitter.core.SelectLogic.PageOrExternalImage.PageOrExternalImageSelector();

			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new kawaii.twitter.core.SelectLogic.Images.ExcludeUsed.AnimatedSelectorWithExcludeLast();

			IPageForTwittingSelector pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, findNewAnimatedByPage, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, logger);

			var tweetCreator = new kawaii.twitter.core.TweetCreator(pageForTwittingSelector, textCreator, twitterImageURL, imageOnWeb, blobDownload, service, lastTweetUpdater, logger);

			logger.Log($"tweetCreator.Execute: {DateTime.Now}");

			//выполнить твит заданной страницы и изображения
			await tweetCreator.Execute();
		}

		static async Task _TestHttp(Logger log)
		{
			HttpResponseMessage response = await _HttpClient.GetAsync("https://google.com");

			// Проверяем статус ответа
			if (response.IsSuccessStatusCode)
			{
				// Получаем HTML-код страницы
				string htmlContent = await response.Content.ReadAsStringAsync();
				log.Log("Страница успешно получена:");
				log.Log(htmlContent.Take(40).ToString());
			}
			else
			{
				log.LogError($"Ошибка: статус {response.StatusCode}");
			}
		}

    }
}
