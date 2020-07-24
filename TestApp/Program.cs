using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("kawaii.twitter.Test Application");

			try
			{
				string postSiteMapURL = "https://kawaii-mobile.com/post.xml";

				HttpClient httpClient = new HttpClient();

				kawaii.twitter.core.SiteMap.XMLSiteMapLoader loader = new kawaii.twitter.core.SiteMap.XMLSiteMapLoader(httpClient);
				//loader.LimitCount = 4;

				kawaii.twitter.core.SiteMap.URLInfo[] resultURLs = await loader.Load(postSiteMapURL);

				if (resultURLs != null && resultURLs.Length > 0)
				{

					//теперь заливаем в базу
					await _LoadToMongo(resultURLs);
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			Console.WriteLine("Done");
		}

		static async Task _LoadToMongo(kawaii.twitter.core.SiteMap.URLInfo[] urls)
		{
			string mongoConnectionString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";

			kawaii.twitter.db.Database database = new kawaii.twitter.db.Database(mongoConnectionString, false);

			var sitePages = database.SitePages;

			foreach (var url in urls)
			{
				kawaii.twitter.db.SitePage page = new kawaii.twitter.db.SitePage
				{
					LastModified = url.LastModified,
					Title = url.Title,
					URL = url.URL
				};

				await sitePages.InsertOneAsync(page);
			}

			//kawaii.twitter.db.SitePage p1 = new kawaii.twitter.db.SitePage();
			//p1.Title = "t1";
			//p1.URL = "https://test1";
			//p1.LastModified = new DateTime(2020, 04, 26);
			//await database.SitePages.InsertOneAsync(p1);
		}

		static async Task _TestTwitterImage()
		{
			HttpClient httpClient = new HttpClient();

			string url = "https://kawaii-mobile.com/2020/07/gleipnir/";

			var twitterImageURL = new kawaii.twitter.core.HtmlParsers.TwitterImageURL();
			string imageURL = await twitterImageURL.GetTwitterImageFileURL(httpClient, url);

			Console.WriteLine(imageURL);
		}

	}
}
