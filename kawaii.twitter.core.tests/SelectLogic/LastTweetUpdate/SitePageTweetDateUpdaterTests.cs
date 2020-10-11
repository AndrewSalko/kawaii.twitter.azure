using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.tests.DatabaseFromSiteMapUpdate;
using kawaii.twitter.db.TweetDateUpdaters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace kawaii.twitter.core.tests.SelectLogic.LastTweetUpdate
{
	[TestClass]
	public class SitePageTweetDateUpdaterTests
	{

		[TestMethod]
		[Description("Тест конструктора")]
		[TestCategory("SitePageTweetDateUpdater")]

		public void SitePageTweetDateUpdater_Ctor_Test()
		{
			try
			{
				var upd = new SitePageTweetDateUpdater(null);
				Assert.Fail("Очікувалося виключення ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "pages");
			}
		}


		[TestMethod]
		[Description("Тест нормальной работы")]
		[TestCategory("SitePageTweetDateUpdater")]

		public void SitePageTweetDateUpdater_UpdateTweetDateForPage_Test()
		{
			TestDB testDB = new TestDB();
			var sitePages = testDB.CreateSitePagesTestDB("sitepages-sitepagetweetdateupdater_1");

			string url = "https://kawaii-mobile.com/2020/08/speed-grapher/";

			DateTime dtNow = new DateTime(2020, 10, 14, 0, 0, 0);

			var findResult = sitePages.FindAsync(x => x.URL == url).Result;
			var recordSpeedGrapher = findResult.FirstOrDefault();

			Assert.IsNotNull(recordSpeedGrapher);

			var upd = new SitePageTweetDateUpdater(sitePages);

			//найти страницу, обновить дату
			upd.UpdateTweetDateForPage(recordSpeedGrapher, dtNow);

			//теперь снова поиск - и сверяем дату
			var findResult2 = sitePages.FindAsync(x => x.URL == url).Result;
			var recordSpeedGrapher2 = findResult2.FirstOrDefault();

			Assert.IsTrue(recordSpeedGrapher2.TweetDate == dtNow);
		}


		[TestMethod]
		[Description("Тест аргументов")]
		[TestCategory("SitePageTweetDateUpdater")]

		public void SitePageTweetDateUpdater_UpdateTweetDateForPage_Arg_Test()
		{
			TestDB testDB = new TestDB();
			var sitePages = testDB.CreateSitePagesTestDB("sitepages-sitepagetweetdateupdater_1");
			DateTime dtNow = new DateTime(2020, 10, 14, 0, 0, 0);
			var upd = new SitePageTweetDateUpdater(sitePages);

			try
			{
				upd.UpdateTweetDateForPage(null, dtNow);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "page");
			}
		}

		[TestMethod]
		[Description("Тест аргументов - передали запись без MongoID")]
		[TestCategory("SitePageTweetDateUpdater")]

		public void SitePageTweetDateUpdater_UpdateTweetDateForPage_Arg_No_Id_Test()
		{
			TestDB testDB = new TestDB();
			var sitePages = testDB.CreateSitePagesTestDB("sitepages-sitepagetweetdateupdater_1");
			DateTime dtNow = new DateTime(2020, 10, 14, 0, 0, 0);
			var upd = new SitePageTweetDateUpdater(sitePages);

			try
			{
				db.SitePage page = new db.SitePage();
				page.Title = "Speed Grapher";
				page.URL = "https://kawaii-mobile.com/2020/08/speed-grapher/";

				upd.UpdateTweetDateForPage(page, dtNow);
				Assert.Fail("Очікувалося ArgumentException");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "page");
				Assert.IsTrue(ex.Message.Contains("Id"));
			}
		}

		[TestMethod]
		[Description("Дата задана неправильно")]
		[TestCategory("SitePageTweetDateUpdater")]

		public void SitePageTweetDateUpdater_UpdateTweetDateForPage_Argument_Date_Invalid_Test()
		{
			TestDB testDB = new TestDB();
			var sitePages = testDB.CreateSitePagesTestDB("sitepages-sitepagetweetdateupdater_1");

			string url = "https://kawaii-mobile.com/2020/08/speed-grapher/";

			DateTime dtNow = new DateTime(2020, 10, 14, 0, 0, 0);

			var findResult = sitePages.FindAsync(x => x.URL == url).Result;
			var recordSpeedGrapher = findResult.FirstOrDefault();

			Assert.IsNotNull(recordSpeedGrapher);

			var upd = new SitePageTweetDateUpdater(sitePages);

			try
			{
				upd.UpdateTweetDateForPage(recordSpeedGrapher, DateTime.MinValue);
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "date");
			}
		}


	}
}
