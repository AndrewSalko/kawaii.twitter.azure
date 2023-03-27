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
		public Dictionary<string, PostInfo> URLToBody = new Dictionary<string, PostInfo>(StringComparer.CurrentCultureIgnoreCase);

		public bool DontThrowException
		{
			get;
			set;
		}

		public async Task<PostInfo> GetHtmlBodyForURL(string url)
		{
			if (!DontThrowException)
				throw new NotImplementedException();

			Task<PostInfo> task = new Task<PostInfo>(() =>
			{
				return URLToBody[url];
			});

			task.Start();

			var result = await task;
			return result;
		}
	}
}
