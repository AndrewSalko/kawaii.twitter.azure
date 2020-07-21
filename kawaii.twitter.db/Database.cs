using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public class Database
	{
		/// <summary>
		/// Коллекция документов - страницы сайта (посты сайта)
		/// </summary>
		public const string COLLECTION_SITE_PAGES = "SitePages";

		IMongoDatabase _SitePagesDB;

		public Database(string connectionString)
		{
			MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
			settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
			var mongoClient = new MongoClient(settings);

			_SitePagesDB = mongoClient.GetDatabase(COLLECTION_SITE_PAGES);
		}

	}
}
