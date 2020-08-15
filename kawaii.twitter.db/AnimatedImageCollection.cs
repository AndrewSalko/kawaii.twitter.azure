using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public class AnimatedImageCollection: BaseCollection
	{
		/// <summary>
		/// Коллекция анимированных gif-изображений (в прод-режиме использовать в аргумент collectionName)
		/// </summary>
		public const string COLLECTION_ANIMATED_IMAGES = "AnimatedImages";

		public IMongoCollection<AnimatedImage> Initialize(string connectionString, bool useSSL, string dataBaseName, string collectionName)
		{
			var db = _Initialize(connectionString, useSSL, dataBaseName);

			if (string.IsNullOrEmpty(collectionName))
				collectionName = COLLECTION_ANIMATED_IMAGES;

			AnimatedImages = db.GetCollection<AnimatedImage>(collectionName);

			return AnimatedImages;
		}

		public IMongoCollection<AnimatedImage> AnimatedImages
		{
			get;
			private set;
		}


	}
}
