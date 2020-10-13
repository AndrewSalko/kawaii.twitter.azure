using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db;
using MongoDB.Driver;

namespace kawaii.twitter.core.tests.DatabaseFromSiteMapUpdate
{
	class TestDB
	{
		/// <summary>
		/// В тестах не применяется, но настоящий
		/// </summary>
		public const string SITEMAP_URL = "https://kawaii-mobile.com/post.xml";

		public IMongoCollection<SitePage> CreateSitePagesTestDB(string collectionName)
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";

			var db = new Database(connString, false, dbName);

			SitePageCollection sitePageCollection = new SitePageCollection(db, collectionName);
			var sitePages = sitePageCollection.SitePages;

			//коллекцию очистить от данных
			var delFilter = Builders<SitePage>.Filter.Exists(x => x.URL);
			sitePages.DeleteMany(delFilter);

			//Загружаем в базу тест данные

			var loader = new Stubs.XMLSiteMapLoaderStub
			{
				Result = SamplePostsDatabase.PostURLs //этот результат будет имитирован
			};

			var logger = new Stubs.LoggerStub();

			DatabaseFromSitemapUpdater dbUpd = new DatabaseFromSitemapUpdater(sitePages);
			dbUpd.UpdateFromSitemap(SITEMAP_URL, loader, logger).Wait();

			return sitePages;
		}

		public IMongoCollection<AnimatedImage> CreateAnimatedTestDB(string collectionName)
		{
			string connString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			string dbName = "unit-test-kawaii";

			var db = new Database(connString, false, dbName);

			AnimatedImageCollection collection = new AnimatedImageCollection(db, collectionName);
			var animatedCollection = collection.AnimatedImages;

			//коллекцию очистить от данных
			var delFilter = Builders<AnimatedImage>.Filter.Exists(x => x.BlobName);
			animatedCollection.DeleteMany(delFilter);

			var sampleImages = SamplePostsDatabase.Images;

			//Загружаем в базу тест данные
			foreach (var img in sampleImages)
			{
				var findResult = animatedCollection.FindAsync(x => x.BlobName == img.BlobName).Result;
				var foundRecord = findResult.FirstOrDefault();
				if (foundRecord != null)
					continue;

				animatedCollection.InsertOneAsync(img).Wait();
			}

			return animatedCollection;
		}

	}
}
