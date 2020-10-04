using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.SiteMap
{
	public class SiteMapWebDownloader: ISiteMapWebDownloader
	{
		HttpClient _HttpClient;

		public SiteMapWebDownloader(HttpClient httpClient)
		{
			_HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}

		public async Task<string> DownloadSiteMapBody(string xmlSiteMapURL)
		{
			string xmlBody = await _HttpClient.GetStringAsync(xmlSiteMapURL);
			return xmlBody;
		}

	}
}
