using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using kawaii.twitter.core.tests.SelectLogic.Stubs;
using kawaii.twitter.db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace kawaii.twitter.core.tests.SelectLogic.FindPageForBlob
{
	[TestClass]
	public class FinPageByBlobNameIntegrationTests
	{

		[TestMethod]
		[Description("Создает тестовую базу в MongoDB, испытывает поиск страницы по блоб-имени и проверка найденного результата")]
		[TestCategory("Integration.FindPageByBlobName")]
		public void FindPageByBlobName_Normal_Find()
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";
			string collName = "sitepages-find-page-by-blobname";

			var db = new Database(connString, false, dbName);

			SitePageCollection sitePageCollection = new SitePageCollection(db, collName, true);
			var sitePages = sitePageCollection.SitePages;

			//коллекцию очистить от данных
			var delFilter = Builders<SitePage>.Filter.Exists(x => x.URL);
			sitePages.DeleteMany(delFilter);

			//заполняем тест-данные
			var page2 = new SitePage()
			{
				Blocked = false,
				SpecialDay = null,
				Title = "Shuumatsu no Izetta",
				LastModified = new DateTime(2020, 04, 25),
				TweetDate = null,
				URL = "https://kawaii-mobile.com/2017/01/shuumatsu-no-izetta/"
			};

			List<SitePage> pagesForTest = new List<SitePage>()
				{
					new SitePage(){ Blocked=false, SpecialDay=null, Title="Princess connect! Re:Dive", LastModified=new DateTime(2020,04,26), TweetDate=null, URL="https://kawaii-mobile.com/2020/08/princess-connect-redive/" },
					page2,
					new SitePage(){ Blocked=false, SpecialDay=null, Title="Gleipnir", LastModified=new DateTime(2020,04,23), TweetDate=null, URL="https://kawaii-mobile.com/2020/07/gleipnir/" },
				};

			sitePages.InsertMany(pagesForTest);

			string blobName = "shuumatsu-no-izetta:image1.gif";

			var queryAble = sitePages.AsQueryable();

			var fnd = new FindPageByBlobName(queryAble, new BlobNameToURLPart());
			var result = fnd.Find(blobName).Result;

			//сверим что мы нашли -
			Assert.IsNotNull(result);
			Assert.IsTrue(result.URL == page2.URL, "URL");
			Assert.IsTrue(result.Blocked == page2.Blocked, "Blocked");
			Assert.IsTrue(result.Title == page2.Title, "Title");
			Assert.IsTrue(result.LastModified == page2.LastModified, "LastModified");
			Assert.IsTrue(result.TweetDate == page2.TweetDate, "TweetDate");

			//и еще тест на "не нашли"
			string blobNot = "rezero:image1.gif";
			var resultNot = fnd.Find(blobNot).Result;

			Assert.IsTrue(resultNot == null, "Ожидался возврат resultNot == null");
		}


	}
}
