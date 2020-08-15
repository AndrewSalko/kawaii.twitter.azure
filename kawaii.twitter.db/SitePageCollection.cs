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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="useSSL"></param>
		/// <param name="dataBaseName">База данных. Если передать null, будет использована константа</param>
		/// <param name="collectionName">Имя коллекции. Если передать null, будет использована константа COLLECTION_SITE_PAGES</param>
		/// <returns></returns>
		public IMongoCollection<SitePage> Initialize(string connectionString, bool useSSL, string dataBaseName, string collectionName)
		{
			var db = _Initialize(connectionString, useSSL, dataBaseName);

			if (string.IsNullOrEmpty(collectionName))
				collectionName = COLLECTION_SITE_PAGES;

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
