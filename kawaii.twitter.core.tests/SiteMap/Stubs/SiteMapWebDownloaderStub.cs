using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SiteMap;

namespace kawaii.twitter.core.tests.SiteMap.Stubs
{
	class SiteMapWebDownloaderStub : ISiteMapWebDownloader
	{
		public string ResultBody
		{
			get;
			set;
		}

		public bool DontThrowException
		{
			get;
			set;
		}

		public async Task<string> DownloadSiteMapBody(string xmlSiteMapURL)
		{
			if (!DontThrowException)
				throw new NotImplementedException();

			Task<string> task = new Task<string>(() =>
			{
				return ResultBody;
			});

			string result = await task;
			return result;
		}
	}
}
