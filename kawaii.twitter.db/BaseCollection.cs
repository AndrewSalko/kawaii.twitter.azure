using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public abstract class BaseCollection
	{
		protected virtual IMongoDatabase _InitializeDB(string connectionString, bool useSSL, string dataBaseName)
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


	}
}
