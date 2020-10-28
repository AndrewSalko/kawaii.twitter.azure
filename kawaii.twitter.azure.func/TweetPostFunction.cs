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
		public static async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo timer, ILogger log)
		{
			//https://docs.microsoft.com/ru-ru/azure/azure-functions/functions-bindings-timer?tabs=csharp#ncrontab-expressions
			//0 */5 * * * * - каждые 5 мин
			//"0 0 * * * *"	- каждый час

			log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

			string animatedBlobConnectionString = kawaii.twitter.core.Env.EnvironmentSecureData.GetValueFromEnvironment("env:kawaii_twitter_azure_animatedblob");
			string azureSiteDBConnectionString = kawaii.twitter.core.Env.EnvironmentSecureData.GetValueFromEnvironment("env:kawaii_twitter_azure_sitepages");

			if (string.IsNullOrEmpty(animatedBlobConnectionString))
			{
				log.LogError("animatedBlobConnectionString not found!");
				return;
			}

			if (string.IsNullOrEmpty(azureSiteDBConnectionString))
			{
				log.LogError("azureSiteDBConnectionString not found!");
				return;
			}

			Logger logger = new Logger(log);

			IDatabase database = new Database(azureSiteDBConnectionString, true, null);

			log.LogInformation($"database connected: {DateTime.Now}");

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
				log.LogInformation("Indexes will not updated (found env:kawaii_twitter_dont_create_indexes)");
			}

			AnimatedImageCollection animatedImageCollection = new AnimatedImageCollection(database, null, !dontCreateIndexes);
			var imagesCollection = animatedImageCollection.AnimatedImages;

			log.LogInformation($"imagesCollection init done: {DateTime.Now}");

			SitePageCollection sitePageCollection = new SitePageCollection(database, null, !dontCreateIndexes);
			var sitePagesCollection = sitePageCollection.SitePages;

			log.LogInformation($"sitePagesCollection init done: {DateTime.Now}");

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

			log.LogInformation($"tweetCreator.Execute: {DateTime.Now}");

			//выполнить твит заданной страницы и изображения
			await tweetCreator.Execute();
		}
    }
}
