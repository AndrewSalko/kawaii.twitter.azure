using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.SpecialDay
{
	interface ISpecialDaySelector
	{
		/// <summary>
		/// Анализирует текущую дату, и с нек.вероятностью если "специальный день" близко - Хелловин или Рождество, может вернуть его спец.имя (см. класс SpecialDays)
		/// </summary>
		/// <returns>Если возвращает null, спец. день не применяется.</returns>
		string DetectSpecialDayName();

	}
}
