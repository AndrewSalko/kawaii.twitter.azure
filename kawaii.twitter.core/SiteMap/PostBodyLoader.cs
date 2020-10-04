using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.SiteMap
{
	public class PostBodyLoader: IPostBodyLoader
	{
		HttpClient _HttpClient;

		public PostBodyLoader(HttpClient httpClient)
		{
			_HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}

		public async Task<string> GetHtmlBodyForURL(string url)
		{
			string htmlBody = await _HttpClient.GetStringAsync(url);
			return htmlBody;
		}
	}
}
