using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db.TweetDateUpdaters
{
	public class SitePageTweetDateUpdater: ISitePageTweetDateUpdater
	{
		IMongoCollection<SitePage> _Pages;

		public SitePageTweetDateUpdater(IMongoCollection<SitePage> pages)
		{
			_Pages = pages ?? throw new ArgumentNullException(nameof(pages));
		}

		/// <summary>
		/// Встановити дату твіта для сторіник
		/// </summary>
		/// <param name="img">Зображення. Використовує Id як ключ</param>
		/// <param name="date">Дата твіта</param>
		public void UpdateTweetDateForPage(SitePage page, DateTime date)
		{
			if (page == null)
				throw new ArgumentNullException(nameof(page));

			if (page.Id == MongoDB.Bson.ObjectId.Empty)
				throw new ArgumentException("Id is ObjectId.Empty", nameof(page));

			if (date == DateTime.MinValue)
				throw new ArgumentException(nameof(date), nameof(date));

			var filter = Builders<SitePage>.Filter.Eq(x => x.Id, page.Id);
			var update = Builders<SitePage>.Update.Set(x => x.TweetDate, date);

			var updateResult = _Pages.UpdateOne(filter, update);
		}

	}
}
