using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public abstract class BaseCollection
	{
		public const string DATABASE_NAME = "kawaii-twitter-db";

		IMongoDatabase _InitializeDB(string connectionString, bool useSSL, string dataBaseName)
		{
			MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

			if (useSSL)
			{
				settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
			}

			var mongoClient = new MongoClient(settings);

			var db = mongoClient.GetDatabase(dataBaseName);
			return db;
		}

		protected IMongoDatabase _Initialize(string connectionString, bool useSSL, string dataBaseName)
		{
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException(nameof(connectionString));

			if (string.IsNullOrEmpty(dataBaseName))
				dataBaseName = DATABASE_NAME;

			var db = _InitializeDB(connectionString, useSSL, dataBaseName);
			return db;
		}


	}
}
