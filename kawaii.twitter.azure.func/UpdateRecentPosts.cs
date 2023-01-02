using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure;
using kawaii.twitter.core.SiteMap;
using kawaii.twitter.db;
using MongoDB.Driver;

namespace kawaii.twitter.azure.func
{
	class UpdateRecentPosts
	{
		public const string SITEMAP_INDEX_URL = "https://kawaii-mobile.com/sitemapindex.xml";

		/// <summary>
		/// Оновлювати останні пости (стільки шт)
		/// </summary>
		public const int RECENT_POSTS_COUNT = 10;

		/// <summary>
		/// Карта сайту (постів) WordPress
		/// </summary>
		public const string SITEMAP_POSTS_URL = "https://kawaii-mobile.com/post.xml";


		public UpdateRecentPosts()
		{
		}

		public async Task UpdateFromSiteMap(HttpClient httpClient, kawaii.twitter.Logs.ILogger logger, IDatabase database, bool dontCreateIndexes, int limitUpdateCount)
		{
			ISiteMapWebDownloader siteMapWebDownloader = new SiteMapWebDownloader(httpClient);
			XMLSiteMapIndexLoader siteMapInd = new XMLSiteMapIndexLoader(siteMapWebDownloader);
			URLInfo postXML = await siteMapInd.GetPostXML(SITEMAP_INDEX_URL);

			if (postXML == null)
			{
				logger.LogError($"post.xml not found in '{SITEMAP_INDEX_URL}'");
				return;
			}

			//нас волнует дата модификации, ее надо сверить с записанной в базе - если post.xml обновлялся, значит
			//нужно запустить процедуру "обновить недавние посты"
			var configCollection = new kawaii.twitter.db.ConfigurationCollection(database, null, !dontCreateIndexes);
			var configs = configCollection.Configurations;

			//ищем основную конфигурацию (такой документ будет один)

			var findResult = await configs.FindAsync(x => x.UniqueName == Configuration.MAIN_CONFIG_UNIQUE_NAME);
			var cfg = findResult.FirstOrDefault();
			if (cfg == null)
			{
				//видимо это первый раз - создаем конфиг...
				cfg = new Configuration
				{
					UniqueName = Configuration.MAIN_CONFIG_UNIQUE_NAME
				};

				await configs.InsertOneAsync(cfg);
			}

			//проверим дату обновления...
			DateTime lastUpd = cfg.PostXMLSiteMapLastModified.GetValueOrDefault();

			if (postXML.LastModified <= lastUpd)
			{
				logger.Log("UpdateFromSiteMap: no update, exiting");
				return;	//апдейт не требуется
			}

			//делаем вычитку N последних постов из post.xml, и обновляем базу

			SitePageCollection sitePageCollection = new SitePageCollection(database, null, true);

			var sitePages = sitePageCollection.SitePages;

			kawaii.twitter.core.DatabaseFromSitemapUpdater databaseFromSitemapUpdater = new kawaii.twitter.core.DatabaseFromSitemapUpdater(sitePages);

			IPostBodyLoader postBodyLoader = new PostBodyLoader(httpClient);

			kawaii.twitter.core.SiteMap.XMLSiteMapLoader loader = new kawaii.twitter.core.SiteMap.XMLSiteMapLoader(siteMapWebDownloader, postBodyLoader)
			{
				//якщо потрібно оновити лише "найновіші пости", наприклад 10 останніх - передати більше 0. Якщо 0 - то оновити усі
				LimitCount = limitUpdateCount
			};

			await databaseFromSitemapUpdater.UpdateFromSitemap(SITEMAP_POSTS_URL, loader, logger);

			//и запишем дату в базу...
			_UpdatePostXMLModificationDateInDB(cfg.UniqueName, postXML.LastModified, configs);
		}


		/// <summary>
		/// Записати дату оновлення post.xml у базу 
		/// </summary>
		/// <param name="uniqName"></param>
		/// <param name="postXMLLastModified"></param>
		/// <param name="configCollection"></param>
		void _UpdatePostXMLModificationDateInDB(string uniqName, DateTime postXMLLastModified, IMongoCollection<Configuration> configCollection)
		{
			if (string.IsNullOrEmpty(uniqName))
				throw new ArgumentNullException(nameof(uniqName));

			if (postXMLLastModified == DateTime.MinValue)
				throw new ArgumentException(nameof(postXMLLastModified), nameof(postXMLLastModified));

			if (configCollection == null)
				throw new ArgumentNullException(nameof(configCollection));

			var filter = Builders<Configuration>.Filter.Eq(x => x.UniqueName, uniqName);
			var update = Builders<Configuration>.Update.Set(x => x.PostXMLSiteMapLastModified, postXMLLastModified);

			var updateResult = configCollection.UpdateOne(filter, update);
		}


	}
}
