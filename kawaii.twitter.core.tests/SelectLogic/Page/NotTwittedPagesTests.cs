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
	public class NotTwittedPagesTests
	{
		const int _TOP_QUERY_COUNT = 3;

		[TestMethod]
		[Description("Тест конструктора, аргумент pages == null")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Ctor_Argument_Pages_Null()
		{
			try
			{
				var pg = new NotTwittedPages(null, new RandomSelector(), _TOP_QUERY_COUNT);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "pages");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент randomSelector == null")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Ctor_Argument_RandomSelector_Null()
		{
			try
			{
				var pg = new NotTwittedPages(new PagesCollectionStub(), null, _TOP_QUERY_COUNT);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "randomSelector");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент topQueryCount == 0")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Ctor_Argument_TopQueryCount_Zero()
		{
			try
			{
				var pg = new NotTwittedPages(new PagesCollectionStub(), new RandomSelector(), 0);
				Assert.Fail("Очікувалося ArgumentException");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "topQueryCount");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, все аргументы в нормальном виде")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Ctor_Arguments_Normal()
		{
			var pg = new NotTwittedPages(new PagesCollectionStub(), new RandomSelector(), _TOP_QUERY_COUNT);
		}

		static readonly SitePage _PagePrincessReDive = new SitePage
		{
			LastModified = new DateTime(2020, 08, 09),
			Title = "Princess Connect! Re:Dive",
			TweetDate = null,
			URL = "https://kawaii-mobile.com/2020/08/princess-connect-redive/"
		};

		static readonly DateTime _PagePrincessReDiveTweetDate = new DateTime(2020, 08, 09, 10, 00, 00);

		static readonly SitePage _PageSpeedGrapher = new SitePage
		{
			LastModified = new DateTime(2020, 08, 02),
			Title = "Speed Grapher",
			TweetDate = null,
			URL = "https://kawaii-mobile.com/2020/08/speed-grapher/"
		};

		static readonly DateTime _PageSpeedGrapherTweetDate = new DateTime(2020, 08, 02, 7, 00, 00);


		static readonly SitePage _PageGleipnir = new SitePage
		{
			LastModified = new DateTime(2020, 07, 19),
			Title = "Gleipnir",
			TweetDate = null,
			URL = "https://kawaii-mobile.com/2020/07/gleipnir/"
		};

		static readonly DateTime _PageGleipnirTweetDate = new DateTime(2020, 07, 19, 6, 00, 00);


		static readonly SitePage _PageHameFura = new SitePage
		{
			LastModified = new DateTime(2020, 07, 05),
			Title = "Otome Game no Hametsu Flag shika Nai Akuyaku Reijou ni Tensei Shiteshimatta",
			TweetDate = null,
			URL = "https://kawaii-mobile.com/2020/07/otome-game-no-hametsu-flag-shika-nai-akuyaku-reijou-ni-tensei-shiteshimatta/"
		};

		static readonly DateTime _PageHameFuraTweetDate = new DateTime(2020, 07, 05, 4, 00, 00);


		/// <summary>
		/// Эта страница имитирует уже "твиченую" (заполнена TweetDate)
		/// </summary>
		static readonly SitePage _PageMagiaRecord = new SitePage
		{
			LastModified = new DateTime(2020, 05, 20),
			Title = "Magia Record",
			TweetDate = new DateTime(2020, 05, 20, 10, 00, 00),
			URL = "https://kawaii-mobile.com/2020/05/magia-record/"
		};

		/// <summary>
		/// Эта страница имитирует уже "твиченую" (заполнена TweetDate)
		/// </summary>
		static readonly SitePage _PageHatarakuSaibou = new SitePage
		{
			LastModified = new DateTime(2020, 05, 09),
			Title = "Hataraku Saibou",
			TweetDate = new DateTime(2020, 05, 09, 8, 00, 00),
			URL = "https://kawaii-mobile.com/2020/05/hataraku-saibou/"
		};


		IMongoCollection<SitePage> _PrepareSitePagesCollection(bool doEmptyCollection, bool doAllWithTweetDate)
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";
			string collName = "not-twitted-pages";

			var sitePagesCollection = new SitePageCollection();
			var pages = sitePagesCollection.Initialize(connString, false, dbName, collName);

			//удаляем все записи, заполняем тест данными
			var delFilter = Builders<SitePage>.Filter.Exists(x => x.URL);
			pages.DeleteMany(delFilter);

			if (doEmptyCollection)
				return pages;	//на этом все - пустая коллекция

			SitePage[] pagesToAdd = new SitePage[] { _PagePrincessReDive, _PageSpeedGrapher, _PageGleipnir, _PageHameFura, _PageMagiaRecord, _PageHatarakuSaibou };

			if (doAllWithTweetDate)
			{
				//здесь чуть сложнее - клонируем исходные данные, чтобы потенциально не мешать другим, и прошьем дату всем (порядок и разм.массива достаточна для тех, у кого нет своей даты твита)

				DateTime[] tweedDates = new DateTime[] { _PagePrincessReDiveTweetDate , _PageSpeedGrapherTweetDate , _PageGleipnirTweetDate , _PageHameFuraTweetDate };

				SitePage[] pagesToAddCloned = new SitePage[pagesToAdd.Length];
				for (int i = 0; i < pagesToAdd.Length; i++)
				{
					var srcPage = pagesToAdd[i];

					var pg = new SitePage
					{
						Blocked = srcPage.Blocked,
						LastModified = srcPage.LastModified,
						SpecialDay = srcPage.SpecialDay,
						Title = srcPage.Title,
						TweetDate = srcPage.TweetDate,
						URL = srcPage.URL
					};

					if (pg.TweetDate == null)
					{
						pg.TweetDate = tweedDates[i];
					}

					pagesToAddCloned[i] = pg;

				}//for i

				pagesToAdd = pagesToAddCloned;
			}

			pages.InsertMany(pagesToAdd);

			return pages;
		}


		[TestMethod]
		[Description("Тест нормальной работы получения новых страниц, которые ни разу не твитили")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Normal_Find_Result_Index_0()
		{
			var pages = _PrepareSitePagesCollection(false, false);

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 0  //он будет выдавать индекс 0 для выбора
			};

			//в нашей тест-коллекции есть страницы с null-полем TweetDate.
			//Их ровно 4 шт, и две заполненные.

			var notTwittedPages = new NotTwittedPages(pages, rndStub, _TOP_QUERY_COUNT);

			//случайный селектор работает по 3 страницам, и первая из них - _PagePrincessReDive

			var resultPage = notTwittedPages.GetPageForTwitting().Result;

			Assert.IsNotNull(resultPage, "Результат не повинен бути null");
			Assert.IsTrue(resultPage.URL == _PagePrincessReDive.URL, "Очікувався результат _PagePrincessReDive.URL");
		}

		[TestMethod]
		[Description("Тест нормальной работы получения новых страниц, которые ни разу не твитили")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Normal_Find_Result_Index_1()
		{
			var pages = _PrepareSitePagesCollection(false, false);

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 1  //он будет выдавать индекс 0 для выбора
			};

			//в нашей тест-коллекции есть страницы с null-полем TweetDate.
			//Их ровно 4 шт, и две заполненные.

			var notTwittedPages = new NotTwittedPages(pages, rndStub, _TOP_QUERY_COUNT);

			var resultPage = notTwittedPages.GetPageForTwitting().Result;

			Assert.IsNotNull(resultPage, "Результат не повинен бути null");
			Assert.IsTrue(resultPage.URL == _PageSpeedGrapher.URL, "Очікувався результат _PageSpeedGrapher.URL");
		}

		[TestMethod]
		[Description("Тест нормальной работы получения новых страниц, которые ни разу не твитили")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Normal_Find_Result_Index_2()
		{
			var pages = _PrepareSitePagesCollection(false, false);

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 2  //он будет выдавать индекс 0 для выбора
			};

			//в нашей тест-коллекции есть страницы с null-полем TweetDate.
			//Их ровно 4 шт, и две заполненные.

			var notTwittedPages = new NotTwittedPages(pages, rndStub, _TOP_QUERY_COUNT);

			var resultPage = notTwittedPages.GetPageForTwitting().Result;

			Assert.IsNotNull(resultPage, "Результат не повинен бути null");
			Assert.IsTrue(resultPage.URL == _PageGleipnir.URL, "Очікувався результат _PageGleipnir.URL");
		}

		[TestMethod]
		[Description("Коллекция не пустая, но у всех страниц заполнена дата твита")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_No_Not_Twitted()
		{
			var pages = _PrepareSitePagesCollection(false, true);	//второй аргумент - приготовить коллекцию так, что у всех будет дата твита

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 0  //он будет выдавать индекс 0 для выбора
			};

			//в нашей тест-коллекции у всех заполнено поле TweetDate.
			//Ответом будет null

			var notTwittedPages = new NotTwittedPages(pages, rndStub, _TOP_QUERY_COUNT);

			var resultPage = notTwittedPages.GetPageForTwitting().Result;

			Assert.IsNull(resultPage, "Результат повинен бути null");
		}



	}
}
