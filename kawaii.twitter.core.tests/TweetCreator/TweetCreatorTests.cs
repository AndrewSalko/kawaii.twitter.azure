using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using kawaii.twitter.blob;
using kawaii.twitter.core.HtmlParsers;
using kawaii.twitter.core.SelectLogic;
using kawaii.twitter.core.tests.DatabaseFromSiteMapUpdate;
using kawaii.twitter.core.Text;
using kawaii.twitter.db;
using kawaii.twitter.db.TweetDateUpdaters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace kawaii.twitter.core.tests.TweetCreator
{
	[TestClass]
	public class TweetCreatorTests
	{

		[TestMethod]
		[Description("TweetCreator формирование базы (теста), испытание одного твита")]
		[TestCategory("TweetCreator")]
		public void TweetCreator_OneTweet_Test()
		{
			var testDB = new TestDB();
			var sitePagesCollection = testDB.CreateSitePagesTestDB("tweetcreator_sitepages_1");
			var animatedCollection = testDB.CreateAnimatedTestDB("tweetcreator_animated_1");

			HttpClient httpClient = new HttpClient();

			ITwitterTextCreator textCreator = new kawaii.twitter.core.Text.TwitterTextCreator();
			IImageOnWeb imageOnWeb = new kawaii.twitter.core.HtmlParsers.ImageOnWeb(httpClient);
			ITwitterImageURL twitterImageURL = new kawaii.twitter.core.HtmlParsers.TwitterImageURL(httpClient);

			//--- блок для lastTweetUpdater
			DateTime nowDate = new DateTime(2020, 04, 26, 10, 00, 00);

			var dateSupply = new SelectLogic.Stubs.DateSupplyStub
			{
				Now = nowDate
			};

			IAnimatedTweetDateUpdater animatedTweetDateUpdater = new AnimatedTweetDateUpdater(animatedCollection);
			ISitePageTweetDateUpdater sitePageTweetDateUpdater = new SitePageTweetDateUpdater(sitePagesCollection);
			ILastTweetUpdater lastTweetUpdater = new kawaii.twitter.core.SelectLogic.LastTweetUpdater(dateSupply, animatedTweetDateUpdater, sitePageTweetDateUpdater);
			//--- 

			var service = new Stubs.TwitterServiceStub();     //стаб для анализа (проверим что в него попадет в итоге)
			IBlobDownload blobDownload = new Stubs.BlobDownloadStub();  //в данном тесте он не применяется (выбросит исключение)

			//селектор - главная вещь, но здесь он будет "простой"
			//Мы берем в обработку одну страницу:
			string url = "https://kawaii-mobile.com/2020/10/uchuu-no-stellvia/";

			var findResultStellvia = sitePagesCollection.FindAsync(x => x.URL == url).Result;
			var pageStellvia = findResultStellvia.FirstOrDefault();

			var selector = new Stubs.PageForTwittingSelectorStub
			{
				ResultPage = pageStellvia
			};

			var creator = new kawaii.twitter.core.TweetCreator(selector, textCreator, twitterImageURL, imageOnWeb, blobDownload, service, lastTweetUpdater);
			creator.Execute().Wait();

			//проверка состояния базы после данной операции:
			
			var findResultStellvia2 = sitePagesCollection.FindAsync(x => x.URL == url).Result;
			var pageStellvia2 = findResultStellvia2.FirstOrDefault();

			//1) должна быть дата твита - та что мы задали
			Assert.IsTrue(pageStellvia2.TweetDate == nowDate);

			//2) проверяем, что там прислали в "твит-текст"
			Assert.IsNotNull(service.ResultText);
			Assert.IsTrue(service.ResultText.Contains(url));

			//3) TODO@: проверить остальное - текст, хештеги, тело изображения
		}

	}
}
