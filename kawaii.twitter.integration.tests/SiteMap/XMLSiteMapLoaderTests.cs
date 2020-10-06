using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using kawaii.twitter.core.SiteMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kawaii.twitter.integration.tests.SiteMap
{
	[TestClass]
	public class XMLSiteMapLoaderTests
	{
		/// <summary>
		/// Карта сайту (постів) WordPress
		/// </summary>
		const string _SITEMAP_POSTS_URL = "https://kawaii-mobile.com/post.xml";

		[TestMethod]
		[Description("Тестування завантаження карти сайту на реальному сайті")]
		[TestCategory("XMLSiteMapLoader")]
		public void XMLSiteMapLoader_LoadSiteMap_RealSite()
		{
			HttpClient client = new HttpClient();

			kawaii.twitter.core.SiteMap.PostBodyLoader postBodyLoader = new PostBodyLoader(client);
			kawaii.twitter.core.SiteMap.SiteMapWebDownloader siteMapWebDownloader = new SiteMapWebDownloader(client);

			var loader = new XMLSiteMapLoader(siteMapWebDownloader, postBodyLoader);
			var postInfos = loader.Load(_SITEMAP_POSTS_URL).Result;

			//в реальной карте сайта что-то должно быть - можем проверить наполнение

			Assert.IsNotNull(postInfos);
			Assert.IsTrue(postInfos.Length > 0);
		}


	}
}
