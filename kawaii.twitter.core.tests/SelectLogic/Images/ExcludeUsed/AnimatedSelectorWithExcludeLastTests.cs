using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using kawaii.twitter.core.SelectLogic.Images.ExcludeUsed;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Images.ExcludeUsed
{
	[TestClass]
	public class AnimatedSelectorWithExcludeLastTests
	{

		[TestMethod]
		[Description("Тест проверки аргумента на null")]
		[TestCategory("AnimatedSelectorWithExcludeLast")]
		public void AnimatedSelectorWithExcludeLast_SelectImage_Null_Exception()
		{
			try
			{
				var selector = new AnimatedSelectorWithExcludeLast();
				selector.SelectImage(null);

				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "images");
			}
		}


		[TestMethod]
		[Description("Тест проверки аргумента на null")]
		[TestCategory("AnimatedSelectorWithExcludeLast")]
		public void AnimatedSelectorWithExcludeLast_SelectImage_EmptyArray_Exception()
		{
			try
			{
				var selector = new AnimatedSelectorWithExcludeLast();
				selector.SelectImage(new db.AnimatedImage[0]);

				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "images");
			}
		}

		[TestMethod]
		[Description("Тест проверки аргумента когда в массиве один элемент")]
		[TestCategory("AnimatedSelectorWithExcludeLast")]
		public void AnimatedSelectorWithExcludeLast_SelectImage_ArrayOneItem_Result()
		{
			var img = new db.AnimatedImage
			{
				BlobName = "test:slug",
				TweetDate = null
			};

			var imgs = new db.AnimatedImage[] { img };

			var selector = new AnimatedSelectorWithExcludeLast();
			var result = selector.SelectImage(imgs);

			Assert.AreSame(img, result, "Очікувалось повернення об'єкту img");
		}


		AnimatedImage[] _PrepareTestArray(int arrayLength, out DateTime mostFreshDate)
		{
			AnimatedImage[] result = new AnimatedImage[arrayLength];

			DateTime startTime = new DateTime(2020, 04, 26, 0, 0, 0);

			mostFreshDate = DateTime.MinValue;

			List<AnimatedImage> rndList = new List<AnimatedImage>(arrayLength);

			//тестовый набор заполняется всегда с датой твита с шагом 10 минут
			for (int i = 0; i < arrayLength; i++)
			{
				string timeStr = string.Format("{0:yyyy_MM_dd___HH_mm_ss}", startTime);

				AnimatedImage img = new AnimatedImage
				{
					BlobName = $"prefix:blob{i}",
					TweetDate = startTime
				};

				mostFreshDate = startTime;  //мы идем по увеличению времени, следовательно самый последний - будет "самый недавно использованный"

				startTime = startTime.AddMinutes(10);
				
				rndList.Add(img);
			}

			//теперь делаем случайные выборки из листа, по очереди добавляем в массив пока лист не кончится
			Random rnd = new Random();

			for (int i = 0; i < arrayLength; i++)
			{
				int listLen = rndList.Count;
				int ind = rnd.Next(listLen);

				AnimatedImage img = rndList[ind];

				result[i] = img;

				//и теперь удаляем его из листа
				rndList.RemoveAt(ind);
			}

			return result;
		}


		[TestMethod]
		[Description("Тест проверки случайной выборки - последний элемент по дате твита не должен быть возвращен")]
		[TestCategory("AnimatedSelectorWithExcludeLast")]
		public void AnimatedSelectorWithExcludeLast_SelectImage_Most_Recent_Excluded()
		{
			//тест масиви готуємо від 2 до 10, це надасть більше надійності

			for (int len = 2; len < 10; len++)
			{
				AnimatedImage[] imgs = _PrepareTestArray(len, out DateTime freshDate);

				//важный момент чтобы мы не перемудрили - и дата была в массиве
				int freshCount = 0;
				for (int e = 0; e < imgs.Length; e++)
				{
					if (imgs[e].TweetDate == freshDate)
						freshCount++;
				}

				Assert.IsTrue(freshCount == 1);
				Assert.IsTrue(imgs.Length == len);

				var selector = new AnimatedSelectorWithExcludeLast();

				int testCount = 100;
				for (int i = 0; i < testCount; i++)
				{
					var result = selector.SelectImage(imgs);

					Assert.IsNotNull(result);

					Assert.IsTrue(result.TweetDate != freshDate, "Дата не не повинна бути найновішою датою у масиві");
				}
			}

		}



	}
}
