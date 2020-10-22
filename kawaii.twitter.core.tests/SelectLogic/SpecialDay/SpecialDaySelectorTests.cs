using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.SpecialDay;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kawaii.twitter.core.tests.SelectLogic.SpecialDay
{
	[TestClass]
	public class SpecialDaySelectorTests
	{

		[TestMethod]
		[Description("Тест конструктора")]
		[TestCategory("SpecialDaySelector")]

		public void SpecialDaySelector_Ctor_DateSupply_Null_Exception()
		{
			try
			{
				var spec = new SpecialDaySelector(null, new Stubs.RandomSelectorStub());
				Assert.Fail("Ожидалось ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "dateSupply");
			}
		}

		[TestMethod]
		[Description("Тест конструктора")]
		[TestCategory("SpecialDaySelector")]

		public void SpecialDaySelector_Ctor_RandomSelector_Null_Exception()
		{
			try
			{
				var spec = new SpecialDaySelector(new Stubs.DateSupplyStub(), null);
				Assert.Fail("Ожидалось ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "randomSelector");
			}
		}

		[TestMethod]
		[Description("Тест нормальной работы Halloween-дня")]
		[TestCategory("SpecialDaySelector")]
		public void SpecialDaySelector_Halloween_Test()
		{
			//За 12 часов до наступления Хелловина выбрасываем с нек.вероятностью признак спец.дня

			var dateSupply = new Stubs.DateSupplyStub();

			var spec = new SpecialDaySelector(dateSupply, new kawaii.twitter.core.SelectLogic.Randomize.RandomSelector());

			int halloweenCount = 0;
			int normalCount = 0;

			int passCount = 10;    //тест-проходы для большей точности

			for (int q = 0; q < passCount; q++)
			{
				DateTime now = new DateTime(2020, 10, 31, 4, 0, 0);
				for (int i = 0; i < 12; i++)
				{
					var dt = now.AddHours(i);

					dateSupply.Now = dt;

					string result = spec.DetectSpecialDayName();
					if (result == kawaii.twitter.db.SpecialDays.HALLOWEEN)
					{
						halloweenCount++;
					}
					else
					{
						normalCount++;
					}

				}//for
			}

			//проверяем результаты: должно быть и то и другое (в норм.режиме примерно 50-на-50)
			//Мы проверим тут порог 30 (с учетом дикой случайности, но меньше быть не должно)
			Assert.IsTrue(halloweenCount > 30, "SpecialDays.HALLOWEEN должен быть возвращен");
			Assert.IsTrue(normalCount > 30, "normalCount должен быть возвращен");
		}


	}
}
