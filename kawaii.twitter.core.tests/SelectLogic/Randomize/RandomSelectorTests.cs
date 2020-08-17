using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.Randomize;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kawaii.twitter.core.tests.SelectLogic.Randomize
{
	[TestClass]
	public class RandomSelectorTests
	{


		[TestMethod]
		[Description("Тест работы случайного генератора")]
		[TestCategory("RandomSelector")]

		public void RandomSelector_Normal_Test()
		{
			int length = 10;
			int attempts = 1000;

			//на большом числе проходов мы "убеждаемся" что получены все числа от 0...length-1
			//Тест не-дерерминирован, и на самом деле интеграционный

			Dictionary<int, object> randomResults = new Dictionary<int, object>();

			var selector = new RandomSelector();

			for (int i = 0; i < attempts; i++)
			{
				int result = selector.GetRandomIndex(length);
				randomResults[result] = null;
			}

			//теперь проверка что все варианты "вошли" в набор
			for (int i = 0; i < length; i++)
			{
				Assert.IsTrue(randomResults.ContainsKey(i), $"Очікувалося що в результаті буде {i}");
			}
		}


	}
}
