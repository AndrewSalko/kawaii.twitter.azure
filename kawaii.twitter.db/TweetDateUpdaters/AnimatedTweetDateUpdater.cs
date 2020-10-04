using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db.TweetDateUpdaters
{
	/// <summary>
	/// Встановлює дату "твіта" для анімов.зображення (gif-файлу)
	/// </summary>
	public class AnimatedTweetDateUpdater: IAnimatedTweetDateUpdater
	{
		IMongoCollection<AnimatedImage> _AnimatedImages;

		public AnimatedTweetDateUpdater(IMongoCollection<AnimatedImage> animatedImages)
		{
			_AnimatedImages = animatedImages ?? throw new ArgumentNullException(nameof(animatedImages));
		}

		/// <summary>
		/// Встановити дату твіта для вказаного зображення.
		/// </summary>
		/// <param name="img">Зображення. Використовує img.Id як ключ</param>
		/// <param name="date">Дата твіта</param>
		public void UpdateTweetDateForBlob(AnimatedImage img, DateTime date)
		{
			if (img == null)
				throw new ArgumentNullException(nameof(img));

			if (img.Id == MongoDB.Bson.ObjectId.Empty)
				throw new ArgumentException("Id is ObjectId.Empty", nameof(img));

			if (date == DateTime.MinValue)
				throw new ArgumentException(nameof(date));

			var filter = Builders<AnimatedImage>.Filter.Eq(x => x.Id, img.Id);
			var update = Builders<AnimatedImage>.Update.Set(x => x.TweetDate, date);

			var updateResult = _AnimatedImages.UpdateOne(filter, update);
		}

	}
}
