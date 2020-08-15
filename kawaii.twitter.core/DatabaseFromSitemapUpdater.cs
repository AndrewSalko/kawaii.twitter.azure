using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

		public async Task UpdateFromSitemap(string postSiteMapURL, XMLSiteMapLoader siteMapLoader)
		{
			//Здесь мы не даем лимиты, по идее каждый раз сканировать всю карту сайта не нужно, а лишь последние 10 постов

			URLInfo[] urls = await siteMapLoader.Load(postSiteMapURL);
			if (urls == null || urls.Length == 0)
				throw new ApplicationException("urls null or empty");

			//по каждому урлу надо уточнить, есть ли он в базе, если нет - добавляем

			foreach (var urlInfo in urls)
			{
				SitePage sitePage = new SitePage
				{
					LastModified = urlInfo.LastModified,
					Title = urlInfo.Title,
					URL = urlInfo.URL,
					SpecialDay = SpecialDays.DetectSpecialDay(urlInfo.URL)
				};

				await _SitePages.InsertOneAsync(sitePage);
			}//foreach

		}

	}
}
