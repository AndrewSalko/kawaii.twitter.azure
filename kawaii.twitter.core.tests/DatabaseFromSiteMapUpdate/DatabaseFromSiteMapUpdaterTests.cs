using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SiteMap;
using kawaii.twitter.db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace kawaii.twitter.core.tests.DatabaseFromSiteMapUpdate
{
	[TestClass]
	public class DatabaseFromSiteMapUpdaterTests
	{
		[TestMethod]
		[Description("DatabaseFromSiteMapUpdater формирование базы из тест-карты сайта")]
		[TestCategory("DatabaseFromSiteMapUpdater")]
		public void DatabaseFromSiteMapUpdater_UpdateFromSitemap_Main1()
		{
			TestDB testDB = new TestDB();
			var sitePages = testDB.CreateSitePagesTestDB("sitepages-databasefromsitemapupdater_1");

			//далее валидация что вышло в базе

			var findResult = sitePages.FindAsync(x => x.URL == "https://kawaii-mobile.com/2020/08/speed-grapher/").Result;
			var recordSpeedGrapher = findResult.FirstOrDefault();
			Assert.IsNotNull(recordSpeedGrapher);

			Assert.IsTrue(recordSpeedGrapher.Blocked == false);
			Assert.IsTrue(recordSpeedGrapher.LastModified == SamplePostsDatabase.PostURLs[0].LastModified);
			Assert.IsTrue(recordSpeedGrapher.SpecialDay == null);
			//Assert.IsTrue(recordSpeedGrapher.Title == SamplePostsDatabase.PostURLs[0].Title);
			Assert.IsTrue(recordSpeedGrapher.URL == SamplePostsDatabase.PostURLs[0].URL);
			Assert.IsTrue(recordSpeedGrapher.TweetDate == null);
			Assert.IsTrue(recordSpeedGrapher.SpecialDay == null);

			var findResultStellvia = sitePages.FindAsync(x => x.URL == "https://kawaii-mobile.com/2020/10/uchuu-no-stellvia/").Result;
			var recordStellvia = findResultStellvia.FirstOrDefault();
			Assert.IsNotNull(recordStellvia);

			Assert.IsTrue(recordStellvia.Blocked == false);
			Assert.IsTrue(recordStellvia.LastModified == SamplePostsDatabase.PostURLs[4].LastModified);
			Assert.IsTrue(recordStellvia.SpecialDay == null);
			//Assert.IsTrue(recordStellvia.Title == SamplePostsDatabase.PostURLs[4].Title);
			Assert.IsTrue(recordStellvia.URL == SamplePostsDatabase.PostURLs[4].URL);
			Assert.IsTrue(recordStellvia.TweetDate == null);
			Assert.IsTrue(recordStellvia.SpecialDay == null);


		}


	}
}
