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

			SitePageCollection sitePageCollection = new SitePageCollection(database, null, true);

			var sitePages = sitePageCollection.SitePages;

			kawaii.twitter.core.DatabaseFromSitemapUpdater databaseFromSitemapUpdater = new kawaii.twitter.core.DatabaseFromSitemapUpdater(sitePages);

			IPostBodyLoader postBodyLoader = new PostBodyLoader(httpClient);

			kawaii.twitter.core.SiteMap.XMLSiteMapLoader loader = new kawaii.twitter.core.SiteMap.XMLSiteMapLoader(siteMapWebDownloader)
			{
				//якщо потрібно оновити лише "найновіші пости", наприклад 10 останніх - передати більше 0. Якщо 0 - то оновити усі
				LimitCount = limitUpdateCount
			};

			await databaseFromSitemapUpdater.UpdateFromSitemap(SITEMAP_POSTS_URL, loader, postBodyLoader, logger);
		}

		public bool UpdateTimeReached
		{
			get
			{
				//Щоб кожного часу не "перезапитувати" оновлення, ми будемо це робити 2 рази на добу
				int hour = DateTime.Now.Hour;

				if (hour == 1 || hour == 13)
				{
					return true;
				}

				return false;
			}
		}

	}
}
