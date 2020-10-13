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
	/// Тест случая "в базе нашлась новая страница" и ее возвращаем, либо нашлось аним.изображение - и мы его возвращаем
	/// </summary>
	[TestClass]
	public class NewPageOrAnimatedFoundTests
	{

		[TestMethod]
		[Description("Случай когда есть новая страница в базе, и ее возвращаем")]
		[TestCategory("PageForTwittingSelector")]
		public void NewPageFound_Returns_This_Page()
		{
			SitePage page = new SitePage
			{
				URL = "https://dummy/url"
			};

			//pageSelectorForNewPages в данном тесте имеет осмысленную реализацию и выдает всегда одну страницу
			var stub = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = page
			};

			IPageSelector pageSelectorForNewPages = stub;

			IAnimatedSelector animatedSelectorForNewImages = new Stubs.AnimatedSelectorStub();
			IFindPageByBlobName findPageByBlobName = new Stubs.FindPageByBlobNameStub();
			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, findPageByBlobName, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());

			TwittData result = pageForTwittingSelector.GetPageForTwitting().Result;

			//проверяем что он вернул
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Page);
			Assert.IsNull(result.Image, "Не очікувалося заповнення TwittData.Image");

			Assert.AreSame(page, result.Page);
		}


		[TestMethod]
		[Description("Новых страниц нет, но найдено новое аним.изображение - выдаем его и связанную с ним страницу для твита")]
		[TestCategory("PageForTwittingSelector")]
		public void NewAnimated_Returns_PageAndImage()
		{
			var stub = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null	//в этом тесте новых страниц нет, поэтому первый селектор вернет null
			};

			IPageSelector pageSelectorForNewPages = stub;

			//а главную "роль" в тесте исполнит селектор новых аним.изображений - он должен вернуть
			AnimatedImage animatedImage = new AnimatedImage
			{
				BlobName = "code-geass:img1.gif",
				TweetDate = null //гифка новая даты твита не должно быть
			};

			var animNewStub = new Stubs.AnimatedSelectorStub
			{
				DontThrowNotImpl = true,
				Result = animatedImage  //стаб вернет этот результат
			};

			IAnimatedSelector animatedSelectorForNewImages = animNewStub;

			//в этом тесте ожидается работа стаба FindPageByBlobNameStub - он должен "найти" страницу по имени блоба
			SitePage page = new SitePage
			{
				URL = "https://dummy/code-geass"
			};

			var findPageByBlob = new Stubs.FindPageByBlobNameStub
			{
				DontThrowNotImpl = true,
				Result = page
			};

			//имитируем найденную страницу...
			IFindPageByBlobName findPageByBlobName = findPageByBlob;

			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, findPageByBlobName, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());

			TwittData result = pageForTwittingSelector.GetPageForTwitting().Result;

			//проверяем что он вернул
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Page);
			Assert.IsNotNull(result.Image);

			//для поиска должны передать строго то имя блоба, что мы предусмотрели тестом
			Assert.IsTrue(findPageByBlob.UsedBlobNameForFind == animatedImage.BlobName);

			Assert.AreSame(page, result.Page);
			Assert.AreSame(animatedImage, result.Image);
		}

		[TestMethod]
		[Description("Новых страниц нет, но найдено новое аним.изображение, однако поиск страницы для него вернул null - выброс исключения")]
		[TestCategory("PageForTwittingSelector")]
		public void NewAnimated_FindPageByBlobName_Returns_Null_Exception()
		{
			var stub = new Stubs.PageSelectorStub
			{
				DontThrowNotImpl = true,
				Result = null   //в этом тесте новых страниц нет, поэтому первый селектор вернет null
			};

			IPageSelector pageSelectorForNewPages = stub;

			//это часть урла, а блоб всегда содержит в начале такое же (до двоеточия)
			string codeGeassURLPart = "code-geass";

			//а главную "роль" в тесте исполнит селектор новых аним.изображений - он должен вернуть
			AnimatedImage animatedImage = new AnimatedImage
			{
				BlobName = "code-geass:img1.gif",
				TweetDate = null //гифка новая даты твита не должно быть
			};

			var animNewStub = new Stubs.AnimatedSelectorStub
			{
				DontThrowNotImpl = true,
				Result = animatedImage  //стаб вернет этот результат
			};

			IAnimatedSelector animatedSelectorForNewImages = animNewStub;

			//в этом тесте ожидается работа стаба FindPageByBlobNameStub - он должен НЕ найти страницу по имени блоба (вернуть null)
			//и это приведет в итоге к исключению
			var findPageByBlob = new Stubs.FindPageByBlobNameStub
			{
				DontThrowNotImpl = true,
				Result = null
			};

			//имитируем найденную страницу...
			IFindPageByBlobName findPageByBlobName = findPageByBlob;

			IPageSelector pageSelectorForAnyPages = new Stubs.PageSelectorStub();
			IFindAnimatedByPage findAnimatedByPage = new Stubs.FindAnimatedByPageStub();
			IPageOrExternalImageSelector pageOrExternalImageSelector = new Stubs.PageOrExternalImageSelectorStub();
			IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast = new Stubs.AnimatedSelectorWithExcludeLastStub();

			var pageForTwittingSelector = new kawaii.twitter.core.SelectLogic.PageForTwittingSelector(pageSelectorForNewPages, animatedSelectorForNewImages, findPageByBlobName, pageSelectorForAnyPages, findAnimatedByPage, pageOrExternalImageSelector, animatedSelectorWithExcludeLast, new TweetCreator.Stubs.Logger());

			try
			{
				TwittData result = pageForTwittingSelector.GetPageForTwitting().Result;
			}
			catch (AggregateException aggrEx)
			{
				ApplicationException appEx = (ApplicationException)aggrEx.InnerExceptions[0];

				//это и должно было произойти. В тексте наш спец.текст
				Assert.IsTrue(appEx.Message.Contains("Find page by blob name failed for"));
				Assert.IsTrue(appEx.Message.Contains(codeGeassURLPart));
			}

		}

	}
}
