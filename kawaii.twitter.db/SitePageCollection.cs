using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public class SitePageCollection
	{
		/// <summary>
		/// Коллекция документов - страницы сайта (посты сайта). Эту константу может использовать вызывающая сторона для передачи
		/// в аргумент collectionName, но для тестов тут может быть что-то другое.
		/// </summary>
		public const string COLLECTION_SITE_PAGES = "SitePages";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="useSSL"></param>
		/// <param name="dataBaseName">База данных. Если передать null, будет использована константа</param>
		/// <param name="collectionName">Имя коллекции. Если передать null, будет использована константа COLLECTION_SITE_PAGES</param>
		/// <returns></returns>
		public SitePageCollection(IDatabase database, string collectionName)
		{
			if (database == null)
				throw new ArgumentNullException(nameof(database));

			var db = database.DB;

			if (string.IsNullOrEmpty(collectionName))
				collectionName = COLLECTION_SITE_PAGES;

			SitePages = db.GetCollection<SitePage>(collectionName);

			//готовим индексы для необходимых полей
			var keysBlocked = Builders<SitePage>.IndexKeys.Ascending(x => x.Blocked);
			var modelBlocked = new CreateIndexModel<SitePage>(keysBlocked);
			
			var keysLastModified = Builders<SitePage>.IndexKeys.Ascending(x => x.LastModified);
			var modelLastModified = new CreateIndexModel<SitePage>(keysLastModified);

			var keysSpecialDay = Builders<SitePage>.IndexKeys.Ascending(x => x.SpecialDay);
			var modelSpecialDay = new CreateIndexModel<SitePage>(keysSpecialDay);

			var keysTweetDate = Builders<SitePage>.IndexKeys.Ascending(x => x.TweetDate);
			var modelTweetDate = new CreateIndexModel<SitePage>(keysTweetDate);

			var keysURL = Builders<SitePage>.IndexKeys.Ascending(x => x.URL);
			var modelURL = new CreateIndexModel<SitePage>(keysURL);

			CreateIndexModel<SitePage>[] indexModels = { modelBlocked, modelLastModified, modelSpecialDay, modelTweetDate, modelURL };
			SitePages.Indexes.CreateMany(indexModels);
		}

		public IMongoCollection<SitePage> SitePages
		{
			get;
			private set;
		}

	}
}
