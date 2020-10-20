using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.Images.Newly;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.core.tests.SelectLogic.Stubs;
using kawaii.twitter.db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace kawaii.twitter.core.tests.SelectLogic.Images.Newly
{
	[TestClass]
	public class NotTwittedAnimatedTests
	{
		const int _TOP_QUERY_COUNT = 3;

		public void NotTwittedAnimated_()
		{
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент pages == null")]
		[TestCategory("NotTwittedAnimated")]
		public void NotTwittedAnimated_Ctor_Argument_Pages_Null()
		{
			try
			{
				var pg = new NotTwittedAnimated(null, new RandomSelector(), _TOP_QUERY_COUNT);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "animatedImages");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент randomSelector == null")]
		[TestCategory("NotTwittedAnimated")]
		public void NotTwittedAnimated_Ctor_Argument_RandomSelector_Null()
		{
			try
			{
				var pg = new NotTwittedAnimated(new AnimatedCollectionStub(), null, _TOP_QUERY_COUNT);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "randomSelector");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент topQueryCount == 0")]
		[TestCategory("NotTwittedAnimated")]
		public void NotTwittedAnimated_Ctor_Argument_TopQueryCount_Zero()
		{
			try
			{
				var pg = new NotTwittedAnimated(new AnimatedCollectionStub(), new RandomSelector(), 0);
				Assert.Fail("Очікувалося ArgumentException");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "topQueryCount");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, все аргументы в нормальном виде")]
		[TestCategory("NotTwittedAnimated")]
		public void NotTwittedAnimated_Ctor_Arguments_Normal()
		{
			var pg = new NotTwittedAnimated(new AnimatedCollectionStub(), new RandomSelector(), _TOP_QUERY_COUNT);
		}

		static readonly AnimatedImage _Img1 = new AnimatedImage { BlobName = "codegeass:x1.gif" };
		static readonly DateTime _TweetDate1 = new DateTime(2020, 01, 01);

		static readonly AnimatedImage _Img2 = new AnimatedImage { BlobName = "codegeass:x2.gif" };
		static readonly DateTime _TweetDate2 = new DateTime(2020, 01, 02);

		static readonly AnimatedImage _Img3 = new AnimatedImage { BlobName = "shuumatsu-no-izetta:izetta1.gif" };
		static readonly DateTime _TweetDate3 = new DateTime(2020, 04, 26);

		static readonly AnimatedImage _Img4 = new AnimatedImage { BlobName = "shuumatsu-no-izetta:izetta2.gif", TweetDate = new DateTime(2020, 04, 27) };
		static readonly AnimatedImage _Img5 = new AnimatedImage { BlobName = "deathnote:light1.gif", TweetDate = new DateTime(2020, 02, 10) };

		IMongoCollection<AnimatedImage> _PrepareAnimatedCollection(bool doEmptyCollection, bool doAllWithTweetDate)
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";
			string collName = "not-twitted-animated";

			var db = new Database(connString, false, dbName);

			var animatedCollection = new AnimatedImageCollection(db, collName, true);
			var pages = animatedCollection.AnimatedImages;

			//удаляем все записи, заполняем тест данными
			var delFilter = Builders<AnimatedImage>.Filter.Exists(x => x.BlobName);
			pages.DeleteMany(delFilter);

			if (doEmptyCollection)
				return pages;   //на этом все - пустая коллекция

			AnimatedImage[] imgsToAdd = new AnimatedImage[] { _Img1, _Img2, _Img3, _Img4, _Img5 };

			if (doAllWithTweetDate)
			{
				//здесь чуть сложнее - клонируем исходные данные, чтобы потенциально не мешать другим, и прошьем дату всем (порядок и разм.массива достаточна для тех, у кого нет своей даты твита)

				DateTime[] tweedDates = new DateTime[] { _TweetDate1, _TweetDate2, _TweetDate3 };

				AnimatedImage[] imgsToAddCloned = new AnimatedImage[imgsToAdd.Length];
				for (int i = 0; i < imgsToAdd.Length; i++)
				{
					var srcImg = imgsToAdd[i];

					var img = new AnimatedImage
					{
						BlobName=srcImg.BlobName,
						TweetDate = srcImg.TweetDate
					};

					if (img.TweetDate == null)
					{
						img.TweetDate = tweedDates[i];
					}

					imgsToAddCloned[i] = img;

				}//for i

				imgsToAdd = imgsToAddCloned;
			}

			pages.InsertMany(imgsToAdd);

			return pages;
		}

		[TestMethod]
		[Description("Тест нормальной работы получения новых пустой результат")]
		[TestCategory("NotTwittedAnimated")]
		public void NotTwittedAnimated_Find_Empty_Result()
		{
			var anim = _PrepareAnimatedCollection(true, false);	//будет пустая коллекция

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 0
			};

			var notTwittedAnimated = new NotTwittedAnimated(anim, rndStub, _TOP_QUERY_COUNT);

			//случайный селектор работает по 3 блобам
			var resultImg = notTwittedAnimated.GetAnimatedImageForTwitting().Result;

			Assert.IsNull(resultImg, "Результат повинен бути null");
		}


		[TestMethod]
		[Description("Тест нормальной работы получения новых страниц, которые ни разу не твитили")]
		[TestCategory("NotTwittedAnimated")]
		public void NotTwittedAnimated_Normal_Find_Result_Index_0()
		{
			var anim = _PrepareAnimatedCollection(false, false);

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 0  //он будет выдавать индекс 0 для выбора
			};

			//в нашей тест-коллекции есть страницы с null-полем TweetDate.
			//Их ровно 4 шт, и две заполненные.

			var notTwittedAnimated = new NotTwittedAnimated(anim, rndStub, _TOP_QUERY_COUNT);

			//случайный селектор работает по 3 блобам
			var resultImg = notTwittedAnimated.GetAnimatedImageForTwitting().Result;

			Assert.IsNotNull(resultImg, "Результат не повинен бути null");
			Assert.IsTrue(resultImg.BlobName == _Img1.BlobName, "Очікувався результат _Img1.BlobName");
		}

		[TestMethod]
		[Description("Тест нормальной работы получения новых страниц, которые ни разу не твитили")]
		[TestCategory("NotTwittedAnimated")]
		public void NotTwittedAnimated_Normal_Find_Result_Index_1()
		{
			var anim = _PrepareAnimatedCollection(false, false);

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 1  //он будет выдавать индекс 1 для выбора
			};

			var notTwittedAnimated = new NotTwittedAnimated(anim, rndStub, _TOP_QUERY_COUNT);

			//случайный селектор работает по 3 блобам
			var resultImg = notTwittedAnimated.GetAnimatedImageForTwitting().Result;

			Assert.IsNotNull(resultImg, "Результат не повинен бути null");
			Assert.IsTrue(resultImg.BlobName == _Img2.BlobName, "Очікувався результат _Img2.BlobName");
		}

		[TestMethod]
		[Description("Тест нормальной работы получения новых страниц, которые ни разу не твитили")]
		[TestCategory("NotTwittedAnimated")]
		public void NotTwittedAnimated_Normal_Find_Result_Index_2()
		{
			var anim = _PrepareAnimatedCollection(false, false);

			//Здесь нам нужен особенный (не рандомный) селектор, который будет давать те индексы которые нужно для граничных условий
			var rndStub = new RandomSelectorStub
			{
				Result = 2  //он будет выдавать индекс 2 для выбора
			};

			var notTwittedAnimated = new NotTwittedAnimated(anim, rndStub, _TOP_QUERY_COUNT);

			//случайный селектор работает по 3 блобам
			var resultImg = notTwittedAnimated.GetAnimatedImageForTwitting().Result;

			Assert.IsNotNull(resultImg, "Результат не повинен бути null");
			Assert.IsTrue(resultImg.BlobName == _Img3.BlobName, "Очікувався результат _Img3.BlobName");
		}



	}
}
