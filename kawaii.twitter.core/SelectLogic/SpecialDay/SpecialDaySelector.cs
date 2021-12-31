using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.Now;

namespace kawaii.twitter.core.SelectLogic.SpecialDay
{
	public class SpecialDaySelector: ISpecialDaySelector
	{
		readonly IDateSupply _DateSupply;
		readonly IRandomSelector _RandomSelector;

		public SpecialDaySelector(IDateSupply dateSupply, IRandomSelector randomSelector)
		{
			_DateSupply = dateSupply ?? throw new ArgumentNullException(nameof(dateSupply));
			_RandomSelector = randomSelector ?? throw new ArgumentNullException(nameof(randomSelector));
		}

		string _DetectHalloween(DateTime now)
		{
			var halloweenDate = new DateTime(now.Year, 11, 01, 0, 0, 0);    //сам хеллоуин - вечер 31.10, следовательно анализ идет от 1.11
																			//считаем что период когда можно твитить про Хеллоуин - за день до
			var diff = halloweenDate - now;

			var hoursDiffHalloween = Math.Abs(diff.TotalHours);
			//за 20 часов до наступления Хелловина начинаем случайно выброс постов
			if (hoursDiffHalloween < 20 && halloweenDate > now)
			{
				//для хеллоуина за n часов до полуночи можно начинать выброс постов, за 2 часа один случайно
				var ind = _RandomSelector.GetRandomIndex(2);
				if (ind == 1)
				{
					return kawaii.twitter.db.SpecialDays.HALLOWEEN;
				}
			}

			return null;
		}

		string _DetectNewYearAndChristmas(DateTime now, DateTime holiday)
		{
			var diff = holiday - now;

			var hoursDiffHalloween = Math.Abs(diff.TotalHours);
			//за 48 часов до наступления Нов.года начинаем случайно выброс постов
			if (hoursDiffHalloween < 48 && holiday > now)
			{
				var ind = _RandomSelector.GetRandomIndex(2);
				if (ind == 1)
				{
					return kawaii.twitter.db.SpecialDays.CHRISTMAS;
				}
			}

			return null;
		}

		DateTime _GetNewYearDay(DateTime now)
		{
			return new DateTime(now.Year, 12, 31, 0, 0, 0);
		}

		DateTime _GetXmasDay(DateTime now)
		{
			return new DateTime(now.Year, 12, 25, 0, 0, 0);
		}


		public string DetectSpecialDayName()
		{
			var now = _DateSupply.Now;

			string halloween = _DetectHalloween(now);
			if (halloween != null)
				return halloween;

			//Новый год
			string newYearDay = _DetectNewYearAndChristmas(now, _GetNewYearDay(now));
			if (newYearDay != null)
				return newYearDay;

			//Рождество
			string xmasDay = _DetectNewYearAndChristmas(now, _GetXmasDay(now));
			if (xmasDay != null)
				return xmasDay;


			return null;
		}
	}
}
