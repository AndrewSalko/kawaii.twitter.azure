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

namespace kawaii.twitter.azure.func
{
    /// <summary>
    /// Azure function
    /// </summary>
    public static class TweetPostFunction
    {
		[FunctionName("TweetPostFunction")]
		public static async void Run([TimerTrigger("0 0 * * * *")] TimerInfo timer, ILogger log)
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

			AnimatedImageCollection animatedImageCollection = new AnimatedImageCollection();
			animatedImageCollection.Initialize(azureSiteDBConnectionString, true, null, null);
			var imagesCollection = animatedImageCollection.AnimatedImages;

			SitePageCollection sitePageCollection = new SitePageCollection();
			sitePageCollection.Initialize(azureSiteDBConnectionString, true, null, null);
			var sitePagesCollection = sitePageCollection.SitePages;

			HttpClient httpClient = new HttpClient();

			ITwitterTextCreator textCreator = new kawaii.twitter.core.Text.TwitterTextCreator();
			IImageOnWeb imageOnWeb = new kawaii.twitter.core.HtmlParsers.ImageOnWeb(httpClient);
			ITwitterImageURL twitterImageURL = new kawaii.twitter.core.HtmlParsers.TwitterImageURL(httpClient);

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

			IPageSelector pageSelectorForNewPages = new kawaii.twitter.core.SelectLogic.Page.NotTwittedPages(sitePagesCollection, randomSelector, topQueryCount);
			IAnimatedSelector animatedSelectorForNewImages = new kawaii.twitter.core.SelectLogic.Images.Newly.NotTwittedAnimated(imagesCollection, randomSelector, topQueryCount);

			var blobNameToURLPart = new kawaii.twitter.core.SelectLogic.FindPageForBlob.BlobNameToURLPart();
			IFindPageByBlobName findPageByBlobName=new kawaii.twitter.core.SelectLogic.FindPageForBlob.FindPageByBlobName(sitePagesCollection.AsQueryable(), blobNameToURLPart);

			IPageSelector pageSelectorForAnyPages = new kawaii.twitter.core.SelectLogic.Page.PageSelector(sitePagesCollection, randomSelector, topQueryCount);

			IFolderFromURL folderFromURL = new kawaii.twitter.core.SelectLogic.URL.FolderFromURL();
			var formatter = new kawaii.twitter.core.SelectLogic.BlobName.Formatter();
			IFindAnimatedByPage findAnimatedByPage = new kawaii.twitter.core.SelectLogic.Images.Find.FindAnimatedByPage(imagesCollection.AsQueryable(), folderFromURL, formatter);

			IPageOrExternalImageSelector pageOrExternalImageSelector = new kawaii.twitter.core.SelectLogic.PageOrExternalImage.PageOrExternalImageSelector();

			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new kawaii.twitter.core.SelectLogic.Images.ExcludeUsed.AnimatedSelectorWithExcludeLast();

			IPageForTwittingSelector pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, findPageByBlobName, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast);

			var tweetCreator = new kawaii.twitter.core.TweetCreator(pageForTwittingSelector, textCreator, twitterImageURL, imageOnWeb, blobDownload, service, lastTweetUpdater);

			//выполнить твит заданной страницы и изображения
			await tweetCreator.Execute();
		}
    }
}
