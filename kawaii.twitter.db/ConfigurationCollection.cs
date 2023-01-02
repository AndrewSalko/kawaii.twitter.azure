using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public class ConfigurationCollection
	{
		/// <summary>
		/// Коллекция документов - конфиг.данные (в этой коллекции будет один документ)
		/// </summary>
		public const string COLLECTION_CONFIGURATION = "Configuration";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="useSSL"></param>
		/// <param name="dataBaseName">База данных. Если передать null, будет использована константа</param>
		/// <param name="collectionName">Имя коллекции. Если передать null, будет использована константа COLLECTION_CONFIGURATION</param>
		/// <param name="createIndexes">Создать (обновить) индексы</param>
		/// <returns></returns>
		public ConfigurationCollection(IDatabase database, string collectionName, bool createIndexes)
		{
			if (database == null)
				throw new ArgumentNullException(nameof(database));

			var db = database.DB;

			if (string.IsNullOrEmpty(collectionName))
				collectionName = COLLECTION_CONFIGURATION;

			Configurations = db.GetCollection<Configuration>(collectionName);

			if (createIndexes)
			{
				//готовим индексы для необходимых полей
				var keysUniqName = Builders<Configuration>.IndexKeys.Ascending(x => x.UniqueName);
				var modelUniqName = new CreateIndexModel<Configuration>(keysUniqName);

				CreateIndexModel<Configuration>[] indexModels = { modelUniqName };
				Configurations.Indexes.CreateMany(indexModels);
			}
		}

		public IMongoCollection<Configuration> Configurations
		{
			get;
			private set;
		}

	}
}
