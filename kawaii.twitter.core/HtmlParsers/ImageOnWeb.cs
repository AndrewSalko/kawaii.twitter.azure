using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace kawaii.twitter.core.HtmlParsers
{
	public class ImageOnWeb: IImageOnWeb
	{
		HttpClient _HttpClient;

		public ImageOnWeb(HttpClient httpClient)
		{
			_HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}

		public async Task<ImageInfo> Download(string url)
		{
			Uri uri = new Uri(url);
			string fileName = uri.Segments.Last();

			//и скачать в память это изображение...
			await _HttpClient.GetStreamAsync(url);

			byte[] imgBody;

			using (HttpResponseMessage response = await _HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
			{
				if (response.StatusCode != HttpStatusCode.OK)
				{
					if (response.StatusCode != HttpStatusCode.Accepted)
					{
						throw new ApplicationException("HttpStatusCode:" + response.StatusCode.ToString());
					}
				}

				imgBody = await response.Content.ReadAsByteArrayAsync();
			}

			ImageInfo result = new ImageInfo
			{
				FileName = fileName,
				Body = imgBody
			};

			return result;
		}
	}
}
