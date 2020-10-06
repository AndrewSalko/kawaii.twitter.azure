using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
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
		[Description("Аргумент httpClient==null - выброс исключения")]
		[TestCategory("PostBodyLoader")]
		public void PostBodyLoader_Ctor_Null_Exception()
		{
			try
			{
				PostBodyLoader postBodyLoader = new PostBodyLoader(null);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "httpClient");
			}
		}

		[TestMethod]
		[Description("Аргумент httpClient==null - выброс исключения")]
		[TestCategory("SiteMapWebDownloader")]
		public void SiteMapWebDownloader_Ctor_Null_Exception()
		{
			try
			{
				var siteMapDownloader = new SiteMapWebDownloader(null);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "httpClient");
			}
		}


		[TestMethod]
		[Description("Аргумент siteMapDownloader==null - выброс исключения")]
		[TestCategory("XMLSiteMapLoader")]
		public void XMLSiteMapLoader_SiteMapDownloader_Null_Exception()
		{
			try
			{
				var loader = new XMLSiteMapLoader(null, new Stubs.PostBodyLoaderStub());
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "siteMapWebDownloader");
			}
		}

		[TestMethod]
		[Description("Аргумент siteMapDownloader==null - выброс исключения")]
		[TestCategory("XMLSiteMapLoader")]
		public void XMLSiteMapLoader_PostBodyLoader_Null_Exception()
		{
			try
			{
				var loader = new XMLSiteMapLoader(new Stubs.SiteMapWebDownloaderStub(), null);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "postBodyLoader");
			}
		}

		const string _SITEMAP_FOLDER = "SiteMap";
		const string _SITEMAP_FILE_NAME = "sitemap-test.xml";

		const string _URL_1 = "https://kawaii-mobile.com/2020/10/uchuu-no-stellvia/";
		const string _URL_1_BODY = "<title>Uchuu no Stellvia</title>";
		const string _URL_1_BODY_UPPER = "<TITLE>Uchuu no Stellvia</TITLE>";
		const string _URL_1_TITLE = "Uchuu no Stellvia";

		const string _URL_2 = "https://kawaii-mobile.com/2019/06/date-a-live-3/";
		const string _URL_2_BODY = "<title>Date a Live 3</title>";
		const string _URL_2_BODY_UPPER = "<TITLE>Date a Live 3</TITLE>";
		const string _URL_2_TITLE = "Date a Live 3";

		const string _URL_3 = "https://kawaii-mobile.com/2019/06/sword-art-online-alicization/";
		const string _URL_3_BODY = "<title>Sword Art Online Alicization</title>";
		const string _URL_3_BODY_UPPER = "<TITLE>Sword Art Online Alicization</TITLE>";
		const string _URL_3_TITLE = "Sword Art Online Alicization";

		void _Test_LoadSiteMap(bool upperCaseHTMLBodyForPosts)
		{
			string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string fileName = Path.Combine(dir, _SITEMAP_FOLDER, _SITEMAP_FILE_NAME);

			//имитация карты сайта (здесь будет xml-тело карты для нескольких урл)
			string body = File.ReadAllText(fileName);


			var siteMapDown = new Stubs.SiteMapWebDownloaderStub
			{
				DontThrowException = true,
				ResultBody = body
			};

			var postBodyDown = new Stubs.PostBodyLoaderStub
			{
				DontThrowException = true
			};
			postBodyDown.URLToBody[_URL_1] = upperCaseHTMLBodyForPosts ? _URL_1_BODY_UPPER : _URL_1_BODY;
			postBodyDown.URLToBody[_URL_2] = upperCaseHTMLBodyForPosts ? _URL_2_BODY_UPPER : _URL_2_BODY;
			postBodyDown.URLToBody[_URL_3] = upperCaseHTMLBodyForPosts ? _URL_3_BODY_UPPER : _URL_3_BODY;

			Dictionary<string, string> urlToTitle = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
			{
				[_URL_1] = _URL_1_TITLE,
				[_URL_2] = _URL_2_TITLE,
				[_URL_3] = _URL_3_TITLE
			};

			var loader = new XMLSiteMapLoader(siteMapDown, postBodyDown);
			var postInfos = loader.Load(_SITEMAP_POSTS_URL).Result;

			Assert.IsNotNull(postInfos);
			Assert.IsTrue(postInfos.Length == postBodyDown.URLToBody.Count);

			//в реальной карте сайта что-то должно быть: сверить что урл и тайтл передаются
			foreach (var pi in postInfos)
			{
				Assert.IsNotNull(pi);

				Assert.IsTrue(pi.LastModified != DateTime.MinValue);
				Assert.IsFalse(string.IsNullOrEmpty(pi.URL));
				Assert.IsFalse(string.IsNullOrEmpty(pi.Title));

				Assert.IsTrue(pi.Title == urlToTitle[pi.URL]);
			}
		}

		[TestMethod]
		[Description("Тест загрузки карты сайта и тайтлов постов, случай низ.регистра")]
		[TestCategory("XMLSiteMapLoader")]
		public void XMLSiteMapLoader_LoadSiteMap_PostBodyLowerCase()
		{
			_Test_LoadSiteMap(false);
		}

		[TestMethod]
		[Description("Тест загрузки карты сайта и тайтлов постов, случай верх.регистра")]
		[TestCategory("XMLSiteMapLoader")]
		public void XMLSiteMapLoader_LoadSiteMap_PostBodyUpperCase()
		{
			_Test_LoadSiteMap(true);
		}



	}
}
