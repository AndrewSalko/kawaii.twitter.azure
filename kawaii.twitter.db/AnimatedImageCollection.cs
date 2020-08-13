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
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException(nameof(connectionString));

			if (string.IsNullOrEmpty(dataBaseName))
				throw new ArgumentNullException(nameof(dataBaseName));

			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException(nameof(collectionName));

			var db = _InitializeDB(connectionString, useSSL, dataBaseName);

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
