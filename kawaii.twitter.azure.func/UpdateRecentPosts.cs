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
		/// <summary>
		/// Оновлювати останні пости (стільки шт)
		/// </summary>
		public const int RECENT_POSTS_COUNT = 10;

		public UpdateRecentPosts()
		{
		}

		public async Task UpdateFromSiteMap(HttpClient httpClient, kawaii.twitter.Logs.ILogger logger, IDatabase database, bool dontCreateIndexes, int limitUpdateCount)
		{
			ISiteMapWebDownloader siteMapWebDownloader = new SiteMapWebDownloader(httpClient);

			SitePageCollection sitePageCollection = new SitePageCollection(database, null, true);

			var sitePages = sitePageCollection.SitePages;

			kawaii.twitter.core.DatabaseFromSitemapUpdater databaseFromSitemapUpdater = new kawaii.twitter.core.DatabaseFromSitemapUpdater(sitePages);

			IPostBodyLoader postBodyLoader = new PostBodyLoader(httpClient);

			kawaii.twitter.core.SiteMap.XMLSiteMapLoader loader = new kawaii.twitter.core.SiteMap.XMLSiteMapLoader(siteMapWebDownloader, postBodyLoader)
			{
				//якщо потрібно оновити лише "найновіші пости", наприклад 10 останніх - передати більше 0. Якщо 0 - то оновити усі
				LimitCount = limitUpdateCount
			};

			await databaseFromSitemapUpdater.UpdateFromSitemap(Schema.SITEMAP_POSTS_URL, loader, logger);
		}


		public bool UpdateTimeReached
		{
			get
			{
				//Щоб кожного часу не "перезапитувати" оновлення, ми будемо це робити випадково (будет приблизно раз-два на добу)
				var r = new Random(Environment.TickCount);
				int v = r.Next(23);
				if (v == 0 || v == 12)
				{
					return true;
				}

				return false;
			}
		}

	}
}
