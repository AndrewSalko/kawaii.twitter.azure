using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.Logs;
using kawaii.twitter.core.SiteMap;
using kawaii.twitter.db;
using MongoDB.Driver;

namespace kawaii.twitter.core
{
	/// <summary>
	/// Оновлює базу з карти сайту. Якщо ліміта не давати (через налаштування siteMapLoader) то може працювати досить довго (1-2 хв).
	/// На практиці оновлення потребує лише перші 10 постів з карти, або навіть один
	/// </summary>
	public class DatabaseFromSitemapUpdater
	{
		IMongoCollection<SitePage> _SitePages;

		public DatabaseFromSitemapUpdater(IMongoCollection<SitePage> sitePages)
		{
			_SitePages = sitePages ?? throw new ArgumentNullException(nameof(sitePages));
		}

		public async Task UpdateFromSitemap(string postSiteMapURL, IXMLSiteMapLoader siteMapLoader, IPostBodyLoader bodyLoader, ILogger log)
		{
			//Здесь мы не даем лимиты, по идее каждый раз сканировать всю карту сайта не нужно, а лишь последние 10 постов

			URLInfo[] urls = await siteMapLoader.Load(postSiteMapURL);
			if (urls == null || urls.Length == 0)
				throw new ApplicationException("urls null or empty");

			log.Log("Found {0} urls from sitemap...", urls.Length);

			//по каждому урлу надо уточнить, есть ли он в базе, если нет - добавляем
			int added = 0;

			foreach (var urlInfo in urls)
			{
				//поиск по URL - это достаточно надежный "ключ" уникальности записи
				string url = urlInfo.URL;

				var findResult = await _SitePages.FindAsync(x => x.URL == url);
				var foundRecord = findResult.FirstOrDefault();
				if (foundRecord != null)
					continue;

				var postBody = await bodyLoader.GetHtmlBodyForURL(url);

				SitePage sitePage = new SitePage
				{
					LastModified = urlInfo.LastModified,
					Title = postBody.Title,
					URL = url,
					SpecialDay = SpecialDays.DetectSpecialDay(url)
				};

				await _SitePages.InsertOneAsync(sitePage);

				added++;

				log.Log("Added new url: {0}", url);

			}//foreach

			log.Log("Done updating from sitemap, added {0}", added);
		}

	}
}
