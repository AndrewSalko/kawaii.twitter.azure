using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public class SitePageCollection : BaseCollection
	{
		/// <summary>
		/// Коллекция документов - страницы сайта (посты сайта). Эту константу может использовать вызывающая сторона для передачи
		/// в аргумент collectionName, но для тестов тут может быть что-то другое.
		/// </summary>
		public const string COLLECTION_SITE_PAGES = "SitePages";

		public IMongoCollection<SitePage> Initialize(string connectionString, bool useSSL, string dataBaseName, string collectionName)
		{
			if (string.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException(nameof(connectionString));

			if (string.IsNullOrEmpty(dataBaseName))
				throw new ArgumentNullException(nameof(dataBaseName));

			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException(nameof(collectionName));

			var db = _InitializeDB(connectionString, useSSL, dataBaseName);

			SitePages = db.GetCollection<SitePage>(collectionName);

			return SitePages;
		}

		public IMongoCollection<SitePage> SitePages
		{
			get;
			private set;
		}

	}
}
