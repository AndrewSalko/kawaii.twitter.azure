using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public class AnimatedImageCollection
	{
		/// <summary>
		/// Коллекция анимированных gif-изображений (в прод-режиме использовать в аргумент collectionName)
		/// </summary>
		public const string COLLECTION_ANIMATED_IMAGES = "AnimatedImages";

		public AnimatedImageCollection(IDatabase database, string collectionName)
		{
			if (database == null)
				throw new ArgumentNullException(nameof(database));

			var db = database.DB;

			if (string.IsNullOrEmpty(collectionName))
				collectionName = COLLECTION_ANIMATED_IMAGES;

			AnimatedImages = db.GetCollection<AnimatedImage>(collectionName);

			//применим индекс (имя блоба)
			var keysBlobName = Builders<AnimatedImage>.IndexKeys.Ascending(x => x.BlobName);
			var modelBlobName = new CreateIndexModel<AnimatedImage>(keysBlobName);

			var keysTweetDate = Builders<AnimatedImage>.IndexKeys.Ascending(x => x.TweetDate);
			var modelTweetDate = new CreateIndexModel<AnimatedImage>(keysTweetDate);

			CreateIndexModel<AnimatedImage>[] indexModels = { modelBlobName, modelTweetDate };
			AnimatedImages.Indexes.CreateMany(indexModels);
		}

		public IMongoCollection<AnimatedImage> AnimatedImages
		{
			get;
			private set;
		}
	}
}
