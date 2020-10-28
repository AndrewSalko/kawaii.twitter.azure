using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using kawaii.twitter.core.SelectLogic.Images;
using kawaii.twitter.core.SelectLogic.Images.ExcludeUsed;
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.core.SelectLogic.Page;
using kawaii.twitter.core.SelectLogic.PageOrExternalImage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kawaii.twitter.core.tests.SelectLogic.PageForTwittingSelector
{
	[TestClass]
	public class PageForTwittingSelectorCtorTests
	{
		const string _FAIL_MESSAGE_ARGUMENTNULL_EXPECTED = "Очікувалося виключення ArgumentNullException";

		[TestMethod]
		[Description("Все аргументы не null тест проходит успешно")]
		[TestCategory("PageForTwittingSelector.Конструтор")]
		public void Ctor_All_Arguments_Not_Null()
		{
			IPageSelector pageSelectorForNewPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage animatedSelectorForNewImages = new Stubs.AnimatedSelectorStub();
			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());
		}

		[TestMethod]
		[Description("Тест проверки аргументов конструтора")]
		[TestCategory("PageForTwittingSelector.Конструтор")]
		public void Ctor_Log_Null_Fail()
		{
			IPageSelector pageSelectorForNewPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage animatedSelectorForNewImages = new Stubs.AnimatedSelectorStub();
			IFindPageByBlobName findPageByBlobName = new Stubs.FindPageByBlobNameStub();
			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			try
			{
				var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, null);
				Assert.Fail(_FAIL_MESSAGE_ARGUMENTNULL_EXPECTED);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName != null && ex.ParamName == "log");
			}
		}


		[TestMethod]
		[Description("Тест проверки аргументов конструтора")]
		[TestCategory("PageForTwittingSelector.Конструтор")]
		public void Ctor_PageSelectorForNewPages_null_Fail()
		{
			IPageSelector pageSelectorForNewPages = null;
			IFindAnimatedByPage animatedSelectorForNewImages = new Stubs.AnimatedSelectorStub();
			IFindPageByBlobName findPageByBlobName = new Stubs.FindPageByBlobNameStub();
			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector=new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			try
			{
				var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());
				Assert.Fail(_FAIL_MESSAGE_ARGUMENTNULL_EXPECTED);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName != null && ex.ParamName == "pageSelectorForNewPages");
			}
		}


		[TestMethod]
		[Description("Тест проверки аргументов конструтора")]
		[TestCategory("PageForTwittingSelector.Конструтор")]
		public void Ctor_AnimatedSelectorForNewImages_null_Fail()
		{
			IPageSelector pageSelectorForNewPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findNewAnimatedByPage = null;
			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			try
			{
				var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, findNewAnimatedByPage, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());
				Assert.Fail(_FAIL_MESSAGE_ARGUMENTNULL_EXPECTED);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName != null && ex.ParamName == "findNewAnimatedByPage");
			}
		}

		[TestMethod]
		[Description("Тест проверки аргументов конструтора")]
		[TestCategory("PageForTwittingSelector.Конструтор")]
		public void Ctor_PageSelectorForAnyPages_null_Fail()
		{
			IPageSelector pageSelectorForNewPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage animatedSelectorForNewImages = new Stubs.AnimatedSelectorStub();
			IPageSelector pageSelectorForAnyPages = null;
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			try
			{
				var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());
				Assert.Fail(_FAIL_MESSAGE_ARGUMENTNULL_EXPECTED);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName != null && ex.ParamName == "pageSelectorForAnyPages");
			}
		}

		[TestMethod]
		[Description("Тест проверки аргументов конструтора")]
		[TestCategory("PageForTwittingSelector.Конструтор")]
		public void Ctor_FindAnimatedByPage_null_Fail()
		{
			IPageSelector pageSelectorForNewPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage animatedSelectorForNewImages = new Stubs.AnimatedSelectorStub();
			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = null;
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			try
			{
				var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());
				Assert.Fail(_FAIL_MESSAGE_ARGUMENTNULL_EXPECTED);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName != null && ex.ParamName == "findAnimatedByPage");
			}
		}

		[TestMethod]
		[Description("Тест проверки аргументов конструтора")]
		[TestCategory("PageForTwittingSelector.Конструтор")]
		public void Ctor_PageOrExternalImageSelector_null_Fail()
		{
			IPageSelector pageSelectorForNewPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage animatedSelectorForNewImages = new Stubs.AnimatedSelectorStub();
			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = null;
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			try
			{
				var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());
				Assert.Fail(_FAIL_MESSAGE_ARGUMENTNULL_EXPECTED);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName != null && ex.ParamName == "pageOrExternalImageSelector");
			}
		}

		[TestMethod]
		[Description("Тест проверки аргументов конструтора")]
		[TestCategory("PageForTwittingSelector.Конструтор")]
		public void Ctor_AnimatedSelectorWithExcludeLast_null_Fail()
		{
			IPageSelector pageSelectorForNewPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage animatedSelectorForNewImages = new Stubs.AnimatedSelectorStub();
			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = null;

			try
			{
				var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());
				Assert.Fail(_FAIL_MESSAGE_ARGUMENTNULL_EXPECTED);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName != null && ex.ParamName == "animatedSelectorWithExcludeLast");
			}
		}


	}
}
