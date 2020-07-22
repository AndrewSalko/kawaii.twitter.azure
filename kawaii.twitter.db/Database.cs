using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public class Database
	{
		public const string DATABASE_NAME = "kawaii-twitter-db";

		/// <summary>
		/// Коллекция документов - страницы сайта (посты сайта)
		/// </summary>
		public const string COLLECTION_SITE_PAGES = "SitePages";

		public Database(string connectionString, bool useSSL)
		{
			MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

			if (useSSL)
			{
				settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
			}

			var mongoClient = new MongoClient(settings);

			DB = mongoClient.GetDatabase(DATABASE_NAME);

			SitePages = DB.GetCollection<SitePage>(COLLECTION_SITE_PAGES);
		}

		public IMongoDatabase DB
		{
			get;
			private set;
		}

		public IMongoCollection<SitePage> SitePages
		{
			get;
			private set;
		}

	}
}
