using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace kawaii.twitter.core.tests.SelectLogic.LastTweetUpdate
{
	[TestClass]
	public class LastTweetUpdaterTests
	{

		[TestMethod]
		[Description("Тест конструктора")]
		[TestCategory("LastTweetUpdater")]
		public void LastTweetUpdater_Ctor_DateSupply_Test()
		{
			//var nowDate = new SelectLogic.Stubs.DateSupplyStub();
			var animatedTweetDateUpdaterStub = new Stubs.AnimatedTweetDateUpdaterStub();
			var pageUpdStub = new Stubs.SitePageTweetDateUpdaterStub();

			try
			{
				var upd = new LastTweetUpdater(null, animatedTweetDateUpdaterStub, pageUpdStub);
				Assert.Fail("Очікувалося виключення ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "dateSupply");
			}
		}

		[TestMethod]
		[Description("Тест конструктора")]
		[TestCategory("LastTweetUpdater")]
		public void LastTweetUpdater_Ctor_AnimatedTweetDateUpdater_Test()
		{
			var nowDate = new SelectLogic.Stubs.DateSupplyStub();
			//var animatedTweetDateUpdaterStub = new Stubs.AnimatedTweetDateUpdaterStub();
			var pageUpdStub = new Stubs.SitePageTweetDateUpdaterStub();

			try
			{
				var upd = new LastTweetUpdater(nowDate, null, pageUpdStub);
				Assert.Fail("Очікувалося виключення ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "animatedTweetDateUpdater");
			}
		}

		[TestMethod]
		[Description("Тест UpdateLastTweetDate с null аргументом и проверка исключения")]
		[TestCategory("LastTweetUpdater")]
		public void LastTweetUpdater_UpdateLastTweetDate_ArgNull_Test()
		{
			var nowDate = new SelectLogic.Stubs.DateSupplyStub();
			var animatedTweetDateUpdaterStub = new Stubs.AnimatedTweetDateUpdaterStub();
			var pageUpdStub = new Stubs.SitePageTweetDateUpdaterStub();

			var upd = new LastTweetUpdater(nowDate, animatedTweetDateUpdaterStub, pageUpdStub);
			try
			{
				
				upd.UpdateLastTweetDate(null);

				Assert.Fail("Очікувалося виключення ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "data");
			}
		}

		[TestMethod]
		[Description("Тест UpdateLastTweetDate с аргументом data но поле page==null и проверка исключения")]
		[TestCategory("LastTweetUpdater")]
		public void LastTweetUpdater_UpdateLastTweetDate_PageNull_Test()
		{
			var nowDate = new SelectLogic.Stubs.DateSupplyStub();
			var animatedTweetDateUpdaterStub = new Stubs.AnimatedTweetDateUpdaterStub();
			var pageUpdStub = new Stubs.SitePageTweetDateUpdaterStub();

			var upd = new LastTweetUpdater(nowDate, animatedTweetDateUpdaterStub, pageUpdStub);
			try
			{
				TwittData data = new TwittData();

				upd.UpdateLastTweetDate(data);

				Assert.Fail("Очікувалося виключення ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "data");
				Assert.IsTrue(ex.Message.Contains("data.Page"));
			}
		}


		[TestMethod]
		[Description("Тест конструктора")]
		[TestCategory("LastTweetUpdater")]
		public void LastTweetUpdater_Ctor_SitePageTweetDateUpdater_Test()
		{
			var nowDate = new SelectLogic.Stubs.DateSupplyStub();
			var animatedTweetDateUpdaterStub = new Stubs.AnimatedTweetDateUpdaterStub();
			//var pageUpdStub = new Stubs.SitePageTweetDateUpdaterStub();

			try
			{
				var upd = new LastTweetUpdater(nowDate, animatedTweetDateUpdaterStub, null);
				Assert.Fail("Очікувалося виключення ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "sitePageTweetDateUpdater");
			}
		}


		[TestMethod]
		[Description("Установка даты последнего твита в базу и проверка результата (без аним.изображения)")]
		[TestCategory("LastTweetUpdater")]
		public void LastTweetUpdater_ForPage_Only_Test()
		{
			//тест дата твита
			var dtNow1 = new DateTime(2020, 10, 14, 0, 0, 0);

			var nowDate = new SelectLogic.Stubs.DateSupplyStub
			{
				Now = dtNow1
			};

			//это в данном тесте не применяется (и выбросит исключение если туда пройдет)
			var animatedTweetDateUpdaterStub = new Stubs.AnimatedTweetDateUpdaterStub();

			var pageUpdStub = new Stubs.SitePageTweetDateUpdaterStub
			{
				DontThrowNotImpl = true //режим работы правильный
			};

			var upd = new LastTweetUpdater(nowDate, animatedTweetDateUpdaterStub, pageUpdStub);

			var page = new db.SitePage
			{
				Title = "https://kawaii-mobile.com/2020/10/uchuu-no-stellvia/",
				URL = "Uchuu no Stellvia"
			};

			TwittData data = new TwittData
			{
				Image = null,
				Page = page
			};

			upd.UpdateLastTweetDate(data);

			Assert.IsTrue(pageUpdStub.CalledDate == dtNow1);
			Assert.AreSame(pageUpdStub.CalledPage, page);
		}


		[TestMethod]
		[Description("Установка даты последнего твита в базу и проверка результата с аним.изображением")]
		[TestCategory("LastTweetUpdater")]
		public void LastTweetUpdater_ForPage_And_Animated_Test()
		{
			//тест дата твита
			var dtNow1 = new DateTime(2020, 10, 14, 0, 0, 0);

			var nowDate = new SelectLogic.Stubs.DateSupplyStub
			{
				Now = dtNow1
			};

			var animatedTweetDateUpdaterStub = new Stubs.AnimatedTweetDateUpdaterStub
			{
				DontThrowNotImpl = true
			};


			var pageUpdStub = new Stubs.SitePageTweetDateUpdaterStub
			{
				DontThrowNotImpl = true //режим работы правильный
			};

			var upd = new LastTweetUpdater(nowDate, animatedTweetDateUpdaterStub, pageUpdStub);

			var page = new db.SitePage
			{
				Title = "https://kawaii-mobile.com/2020/10/uchuu-no-stellvia/",
				URL = "Uchuu no Stellvia"
			};

			var img = new db.AnimatedImage
			{
				BlobName= "uchuu-no-stellvia",
			};

			TwittData data = new TwittData
			{
				Image = img,
				Page = page
			};

			upd.UpdateLastTweetDate(data);

			Assert.IsTrue(pageUpdStub.CalledDate == dtNow1);
			Assert.AreSame(pageUpdStub.CalledPage, page);

			Assert.IsTrue(animatedTweetDateUpdaterStub.CalledDate == dtNow1);
			Assert.AreSame(animatedTweetDateUpdaterStub.CalledImage, img);
		}


	}
}
