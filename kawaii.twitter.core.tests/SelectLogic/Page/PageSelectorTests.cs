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
	public class PageSelectorTests
	{
		const int _MAX_PAGES_FOR_RANDOM = 3;

		[TestMethod]
		[Description("Тест конструктора, аргумент pages == null")]
		[TestCategory("PageSelector")]
		public void PageSelector_Ctor_Argument_Pages_Null()
		{
			try
			{
				var pg = new PageSelector(null, new RandomSelector(), null, _MAX_PAGES_FOR_RANDOM);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "pages");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент randomSelector == null")]
		[TestCategory("PageSelector")]
		public void PageSelector_Ctor_Argument_RandomSelector_Null()
		{
			try
			{
				var pg = new PageSelector(new PagesCollectionStub(), null, null, _MAX_PAGES_FOR_RANDOM);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "randomSelector");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент maxPagesForRandomSelection == 0")]
		[TestCategory("PageSelector")]
		public void PageSelector_Ctor_Argument_MaxPagesForRandomSelection_Zero()
		{
			try
			{
				var pg = new PageSelector(new PagesCollectionStub(), new RandomSelector(), null, 0);
				Assert.Fail("Очікувалося ArgumentException");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "maxPagesForRandomSelection");
			}
		}

		IMongoCollection<SitePage> _PrepareSitePagesCollection()
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";
			string collName = "page-selector-pages";

			var db = new Database(connString, false, dbName);
			var sitePagesCollection = new SitePageCollection(db, collName, true);
			var pages = sitePagesCollection.SitePages;

			//удаляем все записи, заполняем тест данными
			var delFilter = Builders<SitePage>.Filter.Exists(x => x.URL);
			pages.DeleteMany(delFilter);

			SitePage[] pagesToAdd = new SitePage[] { SamplePages.PagePrincessReDive, SamplePages.PageSpeedGrapher, SamplePages.PageGleipnir, SamplePages.PageHameFura };

			pages.InsertMany(pagesToAdd);

			return pages;
		}

		IMongoCollection<SitePage> _PrepareSitePagesEmptyCollection()
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";
			string collName = "page-selector-pages";

			var db = new Database(connString, false, dbName);
			var sitePagesCollection = new SitePageCollection(db, collName, true);
			var pages = sitePagesCollection.SitePages;

			//удаляем все записи, заполняем тест данными
			var delFilter = Builders<SitePage>.Filter.Exists(x => x.URL);
			pages.DeleteMany(delFilter);

			//будет пустая коллекция
			return pages;
		}

		[TestMethod]
		[Description("Тест выборки но в таблице нет записей")]
		[TestCategory("PageSelector")]
		public void PageSelector_Select_NoPagesForResult()
		{
			var pages = _PrepareSitePagesEmptyCollection();

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 0  //он будет выдавать индекс 0 для выбора
			};

			var pg = new PageSelector(pages, rndStub, null, _MAX_PAGES_FOR_RANDOM);
			var resultPage = pg.GetPageForTwitting().Result;

			Assert.IsNull(resultPage, "Очікувався null");
		}


		[TestMethod]
		[Description("Тест нормальной выборки")]
		[TestCategory("PageSelector")]
		public void PageSelector_Normal_Select_0()
		{
			var pages = _PrepareSitePagesCollection();

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 0  //он будет выдавать индекс 0 для выбора
			};

			var pg = new PageSelector(pages, rndStub, null, _MAX_PAGES_FOR_RANDOM);
			var resultPage = pg.GetPageForTwitting().Result;

			Assert.IsNotNull(resultPage);
			//должно найти страницу HameFura
			Assert.IsTrue(resultPage.URL == SamplePages.PageHameFura.URL, "Очукувалася сторінка pageHameFura");
		}

		[TestMethod]
		[Description("Тест нормальной выборки")]
		[TestCategory("PageSelector")]
		public void PageSelector_Normal_Select_1()
		{
			var pages = _PrepareSitePagesCollection();

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 1  //он будет выдавать индекс 0 для выбора
			};

			var pg = new PageSelector(pages, rndStub, null, _MAX_PAGES_FOR_RANDOM);
			var resultPage = pg.GetPageForTwitting().Result;

			Assert.IsNotNull(resultPage);
			//должно найти страницу HameFura
			Assert.IsTrue(resultPage.URL == SamplePages.PageGleipnir.URL, "Очукувалася сторінка Gleipnir");
		}

		[TestMethod]
		[Description("Тест нормальной выборки")]
		[TestCategory("PageSelector")]
		public void PageSelector_Normal_Select_2()
		{
			var pages = _PrepareSitePagesCollection();

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 2  //он будет выдавать индекс 0 для выбора
			};

			var pg = new PageSelector(pages, rndStub, null, _MAX_PAGES_FOR_RANDOM);
			var resultPage = pg.GetPageForTwitting().Result;

			Assert.IsNotNull(resultPage);
			//должно найти страницу HameFura
			Assert.IsTrue(resultPage.URL == SamplePages.PageSpeedGrapher.URL, "Очукувалася сторінка SpeedGrapher");
		}



	}
}
