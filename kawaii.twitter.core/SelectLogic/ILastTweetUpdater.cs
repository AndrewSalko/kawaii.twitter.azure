using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic
{
	public interface ILastTweetUpdater
	{
		/// <summary>
		/// Оновити дату останньго твіта для сторінки (та gif-зображення, якщо воно було використане)
		/// </summary>
		/// <param name="data"></param>
		void UpdateLastTweetDate(TwittData data);
	}
}
