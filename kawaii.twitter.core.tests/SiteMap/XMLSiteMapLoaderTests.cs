using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using kawaii.twitter.core.SiteMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kawaii.twitter.core.tests.SiteMap
{
	[TestClass]
	public class XMLSiteMapLoaderTests
	{
		/// <summary>
		/// Карта сайту (постів) WordPress
		/// </summary>
		const string _SITEMAP_POSTS_URL = "https://kawaii-mobile.com/post.xml";


		[TestMethod]
		[Description("Аргумент null - выброс исключения")]
		[TestCategory("XMLSiteMapLoader")]
		public void XMLSiteMapLoader_HttpClient_Null_Exception()
		{
			try
			{
				var loader = new XMLSiteMapLoader(null);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "httpClient");
			}
		}


		[TestMethod]
		[Description("Аргумент null - выброс исключения")]
		[TestCategory("XMLSiteMapLoader")]
		public void XMLSiteMapLoader_LoadSiteMap()
		{
			HttpClient client = new HttpClient();
			var loader = new XMLSiteMapLoader(client);
			var postInfos = loader.Load(_SITEMAP_POSTS_URL).Result;

			//в реальной карте сайта что-то должно быть...

			Assert.IsNotNull(postInfos);
			Assert.IsTrue(postInfos.Length > 0);

			//TODO@: можно что то еще проверить...
		}




	}
}
