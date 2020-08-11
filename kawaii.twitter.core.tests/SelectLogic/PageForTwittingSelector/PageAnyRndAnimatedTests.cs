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
	/// у страницы есть набор аним.изображений, и выбираем именно случай "с аним.изображением"
	/// </summary>
	[TestClass]
	public class PageAnyRndAnimatedTests
	{

		[TestMethod]
		[Description("Страница найдена, и найдены аним.изображения, рандомизатор использует анимированное")]
		[TestCategory("PageForTwittingSelector")]
		public void PageFound_And_AnimatedFound_Random_Select_Animated()
		{
			//Тест-кейс: найдена не-новая страница и у нее есть внешние изображения. Рандомизированный выбор "решил" использовать одно из них
			SitePage page = new SitePage
			{
				URL = "https://kawaii-mobile.com/oregairu"
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
			IAnimatedSelector animatedSelectorForNewImages = animNewStub;

			IFindPageByBlobName findPageByBlobName = new Stubs.FindPageByBlobNameStub();

			var stubAnyPages = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = page
			};
			IPageSelector pageSelectorForAnyPages = stubAnyPages;

			var anim1 = new AnimatedImage
			{
				BlobName = "oregairu:img1.gif",
				TweetDate = new DateTime(2020, 04, 26, 00, 00, 00)
			};

			var anim2 = new AnimatedImage
			{
				BlobName = "oregairu:yukinoshita.gif",
				TweetDate = new DateTime(2020, 01, 10, 22, 10, 00)
			};

			var anim3 = new AnimatedImage
			{
				BlobName = "oregairu:hatiman.gif",
				TweetDate = new DateTime(2020, 02, 08, 08, 00, 00)
			};

			//аним.изображения найдены, тест использует всегда одно из них (anim2)
			AnimatedImage[] animatedImgs = new AnimatedImage[] { anim1, anim2, anim3 };

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
			stubRandomPageOrImg.UseExternalAnimatedImage = true;   //именно это важно для данного теста

			IPageOrExternalImageSelector pageOrExternalImageSelector = stubRandomPageOrImg;

			var animSelectorStub = new Stubs.AnimatedSelectorWithExcludeLastStub
			{
				DontThrowNotImpl = true,
				Result = anim2
			};

			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = animSelectorStub;

			var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, findPageByBlobName, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast);

			TwittData result = pageForTwittingSelector.GetPageForTwitting().Result;

			//проверяем что он вернул
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Page);

			//в тесте ожидается что вернут и изображение что мы указали
			Assert.IsNotNull(result.Image);
			Assert.AreSame(result.Image, anim2);

			Assert.AreSame(page, result.Page);
		}

	}
}
