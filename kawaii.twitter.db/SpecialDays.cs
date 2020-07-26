using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.db
{
	public class SpecialDays
	{
		public const string HALLOWEEN = "Halloween";

		public const string CHRISTMAS = "Christmas";

		public static string DetectSpecialDay(string url)
		{
			if (string.IsNullOrEmpty(url))
				throw new ArgumentNullException(nameof(url));

			var urlLowCase = url.ToLower();

			if (urlLowCase.Contains("christmas"))
			{
				return CHRISTMAS;
			}

			if (urlLowCase.Contains("halloween") || urlLowCase.Contains("helloween"))
			{
				return HALLOWEEN;
			}

			return null;
		}

	}
}
