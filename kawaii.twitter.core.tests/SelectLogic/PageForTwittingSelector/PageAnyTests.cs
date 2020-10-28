using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using kawaii.twitter.core.SelectLogic.Images;
using kawaii.twitter.core.SelectLogic.Images.ExcludeUsed;
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.core.SelectLogic.Page;
using kawaii.twitter.core.SelectLogic.PageOrExternalImage;
using kawaii.twitter.db;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace kawaii.twitter.core.tests.SelectLogic.PageForTwittingSelector
{
	/// <summary>
	/// Тест случая когда мы берем из базы уже просто "самые давние страницы" (которые уже не новые, но давно не твитили),
	/// и постим их либо сами по себе, либо с аним.изображением
	/// </summary>
	[TestClass]
	public class PageAnyTests
	{

		void _PageFoundBody(AnimatedImage[] resultForFindAnimated)
		{
			SitePage page = new SitePage
			{
				URL = "https://dummy/url"
			};

			//pageSelectorForNewPages в данном тесте выдает null чтобы логика прошла дальше (нет новых страниц)
			var stubNewPages = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null
			};

			//этот стаб вернет null (нет новых аним.изображений)
			var animNewStub = new Stubs.AnimatedSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null
			};


			IPageSelector pageSelectorForNewPages = stubNewPages;
			IFindAnimatedByPage animatedSelectorForNewImages = animNewStub;

			IFindPageByBlobName findPageByBlobName = new Stubs.FindPageByBlobNameStub();

			var stubAnyPages = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = page
			};
			//Главный "герой" этого теста - должен вернуть одну страницу (ее когда-то твитили, но уже давно)
			IPageSelector pageSelectorForAnyPages = stubAnyPages;


			//в нашем тесте этот универсальный поиск выдает null (нет аним.изображений для страницы)
			var stubFindAnimated = new Stubs.FindAnimatedByPageStub
			{
				DontThrowNotImpl = true,
				Result = resultForFindAnimated
			};

			IFindAnimatedByPage findAnimatedByPage = stubFindAnimated;

			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());

			TwittData result = pageForTwittingSelector.GetPageForTwitting().Result;

			//проверяем что он вернул
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Page);
			Assert.IsNull(result.Image, "Не очікувалося заповнення TwittData.Image");

			Assert.AreSame(page, result.Page);
		}


		[TestMethod]
		[Description("Страница найдена, но без анимированных изображений (null возврат)")]
		[TestCategory("PageForTwittingSelector")]
		public void PageFound_But_Null_Animated()
		{
			_PageFoundBody(null);
		}

		[TestMethod]
		[Description("Страница найдена, но без анимированных изображений (пустой массив)")]
		[TestCategory("PageForTwittingSelector")]
		public void PageFound_But_EmptyArray_Animated()
		{
			//этот аналог важен для покрытия кода (случай когда не null но пустой массив)

			AnimatedImage[] emptyArr = new AnimatedImage[0];

			_PageFoundBody(emptyArr);
		}


		[TestMethod]
		[Description("Новых страниц нет, и выбор любой страницы дает null - ошибка")]
		[TestCategory("PageForTwittingSelector")]
		public void No_Page_For_Twitting()
		{
			//этот аналог важен для покрытия кода

			var stubNewPages = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null
			};

			//этот стаб вернет null (нет новых аним.изображений)
			var animNewStub = new Stubs.AnimatedSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null
			};

			IPageSelector pageSelectorForNewPages = stubNewPages;
			IFindAnimatedByPage animatedSelectorForNewImages = animNewStub;

			IFindPageByBlobName findPageByBlobName = new Stubs.FindPageByBlobNameStub();

			var stubAnyPages = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null	//Это ситуация которую моделирует тест - такое не должно быть в реальной системе (и будет выброс исключения)
			};
			IPageSelector pageSelectorForAnyPages = stubAnyPages;

			//в нашем тесте этот универсальный поиск выдает null (нет аним.изображений для страницы)
			var stubFindAnimated = new Stubs.FindAnimatedByPageStub
			{
				DontThrowNotImpl = true,
				Result = null
			};

			IFindAnimatedByPage findAnimatedByPage = stubFindAnimated;
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());

			try
			{
				TwittData result = pageForTwittingSelector.GetPageForTwitting().Result;
				Assert.Fail("Ожидался выброс исключения ApplicationException так как селектор pageSelectorForAnyPages вернул null (чего быть в работе не должно никогда)");
			}
			catch (System.AggregateException aggrEx)
			{
				ApplicationException appEx = (ApplicationException)aggrEx.InnerExceptions[0];

				//это и должно было произойти. В тексте наш спец.текст
				Assert.IsTrue(appEx.Message.Contains("No page found for twitting"));
			}

		}


		[TestMethod]
		[Description("Страница найдена, и найдены аним.изображения, но рандомизатор сообщил не использовать их")]
		[TestCategory("PageForTwittingSelector")]
		public void PageFound_And_AnimatedFound_Random_Select_PageOnly()
		{
			//Тест-кейс: найдена не-новая страница и у нее есть внешние изображения. Рандомизированный выбор "решил" что их не нужно использовать
			SitePage page = new SitePage
			{
				URL = "https://dummy/url"
			};

			//pageSelectorForNewPages в данном тесте выдает null чтобы логика прошла дальше (нет новых страниц)
			var stubNewPages = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null
			};

			//этот стаб вернет null (нет новых аним.изображений)
			var animNewStub = new Stubs.AnimatedSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null
			};


			IPageSelector pageSelectorForNewPages = stubNewPages;
			IFindAnimatedByPage animatedSelectorForNewImages = animNewStub;

			IFindPageByBlobName findPageByBlobName = new Stubs.FindPageByBlobNameStub();

			var stubAnyPages = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = page
			};
			//Главный "герой" этого теста - должен вернуть одну страницу (ее когда-то твитили, но уже давно)
			IPageSelector pageSelectorForAnyPages = stubAnyPages;

			//аним.изображения найдены, но их не будем использовать
			AnimatedImage[] animatedImgs = new AnimatedImage[]
			{
				//здесь нам не важно что - оно просто должно быть, тест "не выбирает" это
				new AnimatedImage() { BlobName="dummy", TweetDate=new DateTime(2020,04,26,00,00,00) }
			};

			var stubFindAnimated = new Stubs.FindAnimatedByPageStub
			{
				DontThrowNotImpl = true,
				Result = animatedImgs
			};

			IFindAnimatedByPage findAnimatedByPage = stubFindAnimated;

			//этот стаб имитирует "случайное решение" о том использовать ли изобр.со страницы , или внешнее анимированное.
			//В этом тесте он всегда выбирает "со страницы"
			var stubRandomPageOrImg = new Stubs.PageOrExternalImageSelectorStub();
			stubRandomPageOrImg.DontThrowNotImpl = true;
			stubRandomPageOrImg.UseExternalAnimatedImage = false;	//именно это важно для данного теста

			IPageOrExternalImageSelector pageOrExternalImageSelector = stubRandomPageOrImg;

			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());

			TwittData result = pageForTwittingSelector.GetPageForTwitting().Result;

			//проверяем что он вернул
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Page);
			Assert.IsNull(result.Image, "Не очікувалося заповнення TwittData.Image");

			Assert.AreSame(page, result.Page);
		}


	}
}
