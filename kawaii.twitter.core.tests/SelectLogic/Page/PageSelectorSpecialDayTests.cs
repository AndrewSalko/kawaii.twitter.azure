using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.Page;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.core.tests.SelectLogic.Stubs;
using kawaii.twitter.db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;


namespace kawaii.twitter.core.tests.SelectLogic.Page
{
	[TestClass]
	public class PageSelectorSpecialDayTests
	{
		const int _MAX_PAGES_FOR_RANDOM = 5;

		IMongoCollection<SitePage> _PrepareSitePagesCollection()
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";
			string collName = "page-selector-halloween-pages";

			var db = new Database(connString, false, dbName);
			var sitePagesCollection = new SitePageCollection(db, collName, true);
			var pages = sitePagesCollection.SitePages;

			//удаляем все записи, заполняем тест данными
			var delFilter = Builders<SitePage>.Filter.Exists(x => x.URL);
			pages.DeleteMany(delFilter);

			SitePage[] pagesToAdd = new SitePage[]
			{
				SamplePages.PagePrincessReDive, SamplePages.PageSpeedGrapher, SamplePages.PageGleipnir, SamplePages.PageHameFura,
				SamplePages.PageHalloween2013, SamplePages.PageHalloween2014, SamplePages.PageHalloween2015, SamplePages.PageHalloween2019
			};

			pages.InsertMany(pagesToAdd);

			return pages;
		}

		[TestMethod]
		[Description("Тест нормальной выборки")]
		[TestCategory("PageSelector.SpecialDay.Halloween")]

		public void PageSelector_Halloween_Normal_2013()
		{
			var pages = _PrepareSitePagesCollection();

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 0  //он будет выдавать индекс 0 для выбора
			};

			var pg = new PageSelector(pages, rndStub, kawaii.twitter.db.SpecialDays.HALLOWEEN, _MAX_PAGES_FOR_RANDOM);
			var resultPage = pg.GetPageForTwitting().Result;

			Assert.IsNotNull(resultPage);

			//должно найти страницу SamplePages.PageHalloween2013 - см.порядок в ф-ции _PrepareSitePagesCollection
			Assert.IsTrue(resultPage.URL == SamplePages.PageHalloween2013.URL, "Очукувалася сторінка PageHalloween2013");
		}

		[TestMethod]
		[Description("Тест нормальной выборки")]
		[TestCategory("PageSelector.SpecialDay.Halloween")]

		public void PageSelector_Halloween_Normal_2019()
		{
			var pages = _PrepareSitePagesCollection();

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 3  //он будет выдавать индекс 3 для выбора
			};

			var pg = new PageSelector(pages, rndStub, kawaii.twitter.db.SpecialDays.HALLOWEEN, _MAX_PAGES_FOR_RANDOM);
			var resultPage = pg.GetPageForTwitting().Result;

			Assert.IsNotNull(resultPage);

			//должно найти страницу SamplePages.PageHalloween2013 - см.порядок в ф-ции _PrepareSitePagesCollection
			Assert.IsTrue(resultPage.URL == SamplePages.PageHalloween2019.URL, "Очукувалася сторінка PageHalloween2019");
		}


	}
}
