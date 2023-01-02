using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.SiteMap
{
	public interface IXMLSiteMapIndexLoader
	{
		Task<URLInfo> GetPostXML(string xmlSiteMapURL);
	}
}
