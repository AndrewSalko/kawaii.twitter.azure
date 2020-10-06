using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SiteMap;

namespace kawaii.twitter.core.tests.SiteMap.Stubs
{
	class PostBodyLoaderStub : IPostBodyLoader
	{
		/// <summary>
		/// Заполните набор url => тело html
		/// </summary>
		public Dictionary<string, string> URLToBody = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

		public bool DontThrowException
		{
			get;
			set;
		}

		public async Task<string> GetHtmlBodyForURL(string url)
		{
			if (!DontThrowException)
				throw new NotImplementedException();

			Task<string> task = new Task<string>(() =>
			{
				return URLToBody[url];
			});

			task.Start();

			string result = await task;
			return result;
		}
	}
}
