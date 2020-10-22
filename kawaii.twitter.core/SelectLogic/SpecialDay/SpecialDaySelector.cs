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

		public string DetectSpecialDayName()
		{
			var now = _DateSupply.Now;

			string halloween = _DetectHalloween(now);
			if (halloween != null)
				return halloween;

			//TODO@: реализовать для Рождества

			return null;
		}
	}
}
