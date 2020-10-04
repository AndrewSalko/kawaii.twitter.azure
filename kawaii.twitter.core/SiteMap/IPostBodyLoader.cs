using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.SiteMap
{
	public interface IPostBodyLoader
	{
		Task<string> GetHtmlBodyForURL(string url);
	}
}
