using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.db.TweetDateUpdaters
{
	public interface IAnimatedTweetDateUpdater
	{
		/// <summary>
		/// Встановити дату твіта для вказаного зображення.
		/// </summary>
		/// <param name="img">Зображення. Використовує img.Id як ключ</param>
		/// <param name="date">Дата твіта</param>
		void UpdateTweetDateForBlob(AnimatedImage img, DateTime date);

	}
}
