using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.tests.DatabaseFromSiteMapUpdate;
using kawaii.twitter.db.TweetDateUpdaters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace kawaii.twitter.core.tests.SelectLogic.LastTweetUpdate
{
	[TestClass]
	public class AnimatedTweetDateUpdaterTests
	{

		[TestMethod]
		[Description("Тест конструктора")]
		[TestCategory("AnimatedTweetDateUpdater")]

		public void AnimatedTweetDateUpdater_Ctor_Test()
		{
			try
			{
				var upd = new AnimatedTweetDateUpdater(null);
				Assert.Fail("Очікувалося виключення ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "animatedImages");
			}
		}


		[TestMethod]
		[Description("Тест нормальной работы")]
		[TestCategory("AnimatedTweetDateUpdater")]

		public void AnimatedTweetDateUpdater_UpdateTweetDateForBlob_Test()
		{
			TestDB testDB = new TestDB();

			var animatedCollection = testDB.CreateAnimatedTestDB("animatedtweetdateupdater_test1");

			//ищем точно заданную запись, обновляем. Работает поиск по Id, нужно помнить про это
			var sampleAnim1 = SamplePostsDatabase.Images[0];

			var findResult = animatedCollection.FindAsync(x => x.BlobName == sampleAnim1.BlobName).Result;
			var record = findResult.FirstOrDefault();

			var upd = new AnimatedTweetDateUpdater(animatedCollection);

			DateTime dtNow = new DateTime(2020, 04, 26, 0, 0, 0);

			upd.UpdateTweetDateForBlob(record, dtNow);

			//теперь снова поиск и тест:

			var findResult2 = animatedCollection.FindAsync(x => x.BlobName == sampleAnim1.BlobName).Result;
			var record2 = findResult2.FirstOrDefault();

			Assert.IsTrue(record2.TweetDate == dtNow);
		}


		[TestMethod]
		[Description("Тест аргументов")]
		[TestCategory("AnimatedTweetDateUpdater")]

		public void AnimatedTweetDateUpdater_UpdateTweetDateForBlob_ArgNull_Test()
		{
			TestDB testDB = new TestDB();
			var animatedCollection = testDB.CreateAnimatedTestDB("animatedtweetdateupdater_test1");
			var upd = new AnimatedTweetDateUpdater(animatedCollection);

			try
			{
				upd.UpdateTweetDateForBlob(null, new DateTime(2020, 04, 26, 0, 0, 0));
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "img");
			}
		}

		[TestMethod]
		[Description("Тест аргументов")]
		[TestCategory("AnimatedTweetDateUpdater")]

		public void AnimatedTweetDateUpdater_UpdateTweetDateForBlob_Arg_No_Id_Test()
		{
			TestDB testDB = new TestDB();
			var animatedCollection = testDB.CreateAnimatedTestDB("animatedtweetdateupdater_test1");
			var upd = new AnimatedTweetDateUpdater(animatedCollection);

			try
			{
				db.AnimatedImage img = new db.AnimatedImage();
				img.BlobName = "samplepost:img1.gif";

				upd.UpdateTweetDateForBlob(img, new DateTime(2020, 04, 26, 0, 0, 0));
				Assert.Fail("Очікувалося ArgumentException");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "img");
				Assert.IsTrue(ex.Message.Contains("Id"));
			}
		}

		[TestMethod]
		[Description("Тест аргументов дата неправильная")]
		[TestCategory("AnimatedTweetDateUpdater")]

		public void AnimatedTweetDateUpdater_UpdateTweetDateForBlob_Arg_Date_Test()
		{
			TestDB testDB = new TestDB();

			var animatedCollection = testDB.CreateAnimatedTestDB("animatedtweetdateupdater_test1");

			//ищем точно заданную запись, обновляем. Работает поиск по Id, нужно помнить про это
			var sampleAnim1 = SamplePostsDatabase.Images[0];

			var findResult = animatedCollection.FindAsync(x => x.BlobName == sampleAnim1.BlobName).Result;
			var record = findResult.FirstOrDefault();

			var upd = new AnimatedTweetDateUpdater(animatedCollection);


			try
			{
				upd.UpdateTweetDateForBlob(record, DateTime.MinValue);

				Assert.Fail("Очікувалося ArgumentException");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "date");
			}
		}



	}
}
