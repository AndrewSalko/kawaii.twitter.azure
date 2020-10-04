using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.SiteMap
{
	public interface ISiteMapWebDownloader
	{
		Task<string> DownloadSiteMapBody(string xmlSiteMapURL);
	}
}
