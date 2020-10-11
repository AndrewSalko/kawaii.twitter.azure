using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SiteMap;

namespace kawaii.twitter.core.tests.DatabaseFromSiteMapUpdate.Stubs
{
	class XMLSiteMapLoaderStub : IXMLSiteMapLoader
	{
		public URLInfo[] Result
		{
			get;
			set;
		}

		public async Task<URLInfo[]> Load(string xmlSiteMapURL)
		{
			Task<URLInfo[]> task = new Task<URLInfo[]>(() =>
			{
				return Result;
			});

			task.Start();

			var res = await task;
			return res;
		}
	}
}
