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

				string mongoConnectionString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";

				//kawaii.twitter.db.Database database = new kawaii.twitter.db.Database(mongoConnectionString, false);

				kawaii.twitter.db.SitePageCollection sitePageCollection = new kawaii.twitter.db.SitePageCollection();
				sitePageCollection.Initialize(mongoConnectionString, false, null, null);
				var sitePages = sitePageCollection.SitePages;

				kawaii.twitter.core.DatabaseFromSitemapUpdater databaseFromSitemapUpdater = new kawaii.twitter.core.DatabaseFromSitemapUpdater(sitePages);

				kawaii.twitter.core.SiteMap.XMLSiteMapLoader loader = new kawaii.twitter.core.SiteMap.XMLSiteMapLoader(httpClient);

				await databaseFromSitemapUpdater.UpdateFromSitemap(postSiteMapURL, loader);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			Console.WriteLine("Done");
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
