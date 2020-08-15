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
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.core.SelectLogic.URL;
using kawaii.twitter.core.SelectLogic.BlobName;

namespace kawaii.twitter.core.tests.SelectLogic.Images.Find
{
	[TestClass]
	public class FindAnimatedByPageTests
	{

		[TestMethod]
		[Description("Тест аргумента конструктора  animatedImages==null")]
		[TestCategory("FindAnimatedByPage")]
		public void FindAnimatedByPage_Ctor_AnimatedImages_Null_Exception()
		{
			try
			{
				var find = new FindAnimatedByPage(null, new FolderFromURL(), new Formatter());
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "animatedImages");
			}
		}

		[TestMethod]
		[Description("Тест аргумента конструктора folderFromURL==null")]
		[TestCategory("FindAnimatedByPage")]
		public void FindAnimatedByPage_Ctor_FolderFromURL_Null_Exception()
		{
			try
			{
				var find = new FindAnimatedByPage(new Stubs.QueryableStub<AnimatedImage>(), null, new Formatter());
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "folderFromURL");
			}
		}

		[TestMethod]
		[Description("Тест аргумента конструктора folderFromURL==null")]
		[TestCategory("FindAnimatedByPage")]
		public void FindAnimatedByPage_Ctor_Formatter_Null_Exception()
		{
			try
			{
				var find = new FindAnimatedByPage(new Stubs.QueryableStub<AnimatedImage>(), new FolderFromURL(), null);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "formatter");
			}
		}

		[TestMethod]
		[Description("Создает тест-коллекцию AnimatedImage, ищет в ней по url страницы")]
		[TestCategory("Integration.FindAnimatedByPage")]
		public void FindAnimatedByPage_Normal_Find()
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";
			string collName = "animatedimage-find-animated-by-page";

			var animatedImageCollection = new AnimatedImageCollection();
			var animated = animatedImageCollection.Initialize(connString, false, dbName, collName);

			//коллекцию очистить от данных
			var delFilter = Builders<AnimatedImage>.Filter.Exists(x => x.BlobName);
			animated.DeleteMany(delFilter);

			AnimatedImage img1 = new AnimatedImage { BlobName = "codegeass:x1.gif", TweetDate = new DateTime(2020, 01, 01) };
			AnimatedImage img2 = new AnimatedImage { BlobName = "codegeass:x2.gif", TweetDate = new DateTime(2020, 01, 02) };
			AnimatedImage img3 = new AnimatedImage { BlobName = "shuumatsu-no-izetta:izetta1.gif", TweetDate = new DateTime(2020, 04, 26) };
			AnimatedImage img4 = new AnimatedImage { BlobName = "shuumatsu-no-izetta:izetta2.gif", TweetDate = new DateTime(2020, 04, 27) };
			AnimatedImage img5 = new AnimatedImage { BlobName = "deathnote:light1.gif", TweetDate = new DateTime(2020, 02, 10) };

			AnimatedImage[] testImages = new AnimatedImage[] { img1, img2, img3, img4, img5 };

			animated.InsertMany(testImages);

			//Теперь отбираем изображения для поста
			string url = "https://kawaii-mobile.com/2017/01/shuumatsu-no-izetta/";

			var find = new FindAnimatedByPage(animated.AsQueryable(), new FolderFromURL(), new Formatter());
			var result = find.GetAnimatedImagesForPage(url).Result;

			Assert.IsNotNull(result);
			Assert.IsTrue(result.Length == 2, "Должно быть найдено 2 записи");

			var r3 = result.SingleOrDefault(x => x.BlobName == img3.BlobName);
			Assert.IsNotNull(r3, "Тест запись img3 не найдена");
			Assert.IsTrue(r3.TweetDate==img3.TweetDate, "Тест запись r3.TweetDate==img3.TweetDate");

			var r4 = result.SingleOrDefault(x => x.BlobName == img4.BlobName);
			Assert.IsNotNull(r4, "Тест запись img4 не найдена");
			Assert.IsTrue(r4.TweetDate == img4.TweetDate, "Тест запись r4.TweetDate==img4.TweetDate");


			//и еще не большой тест на не-существующую запись - поиск который ничего не найдет:
			string urlNotExistsAnimated = "https://kawaii-mobile.com/2019/11/maou-sama-retry/";
			var result2 = find.GetAnimatedImagesForPage(urlNotExistsAnimated).Result;

			Assert.IsTrue(result2.Length==0, "Ожидалось что result2 будет пустым массивом");
		}

	}
}
