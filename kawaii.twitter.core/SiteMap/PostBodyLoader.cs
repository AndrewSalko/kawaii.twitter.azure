using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

		public async Task<PostInfo> GetHtmlBodyForURL(string url)
		{
			PostInfo result = new PostInfo();

			string body= await _HttpClient.GetStringAsync(url);

			result.HtmlBody = body;
			result.Title = _GetTitle(body);

			return result;
		}

		string _GetTitle(string htmlBody)
		{
			string result = null;

			Match m = Regex.Match(htmlBody, @"<title>(.*?)</title>");
			if (m.Success)
			{
				result = m.Groups[1].Value;
			}

			if (result == null)
			{
				Match m2 = Regex.Match(htmlBody, @"<TITLE>(.*?)</TITLE>");
				if (m2.Success)
				{
					result = m2.Groups[1].Value;
				}
			}

			return result;
		}


	}
}
