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
				HttpClient httpClient = new HttpClient();

				string url = "https://kawaii-mobile.com/2020/07/gleipnir/";

				var twitterImageURL = new kawaii.twitter.core.HtmlParsers.TwitterImageURL();
				string imageURL = await twitterImageURL.GetTwitterImageFileURL(httpClient, url);

				Console.WriteLine(imageURL);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			Console.WriteLine("Done");
		}
	}
}
