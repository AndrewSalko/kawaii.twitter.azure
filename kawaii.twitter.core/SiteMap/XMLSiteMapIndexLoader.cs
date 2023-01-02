using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace kawaii.twitter.core.SiteMap
{
	/// <summary>
	///	Завантажує та парсить індекс карти сайта, щоб дізнатися коли змінено карту post.xml (по даті - це додавання нового поста)
	/// https://kawaii-mobile.com/sitemapindex.xml
	/// </summary>

	public class XMLSiteMapIndexLoader: IXMLSiteMapIndexLoader
	{
		public const string POST_XML = "post.xml";

		ISiteMapWebDownloader _SiteMapWebDownloader;

		public XMLSiteMapIndexLoader(ISiteMapWebDownloader siteMapWebDownloader)
		{
			_SiteMapWebDownloader = siteMapWebDownloader ?? throw new ArgumentNullException(nameof(siteMapWebDownloader));
		}

		public async Task<URLInfo> GetPostXML(string xmlSiteMapURL)
		{
			string xmlBody = await _SiteMapWebDownloader.DownloadSiteMapBody(xmlSiteMapURL);

			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlBody);

			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
			nsmgr.AddNamespace("x", "http://www.sitemaps.org/schemas/sitemap/0.9");

			//выбираем все ноды <sitemap>, нас волнует тот где post.xml
			XmlNodeList urlNodes = xmlDoc.SelectNodes("//x:sitemap", nsmgr);
			if (urlNodes.Count > 0)
			{
				for (int i = 0; i < urlNodes.Count; i++)
				{
					var node = urlNodes[i];

					string url = null;

					var locNode = node.SelectSingleNode("x:loc", nsmgr);
					if (locNode != null)
					{
						url = locNode.InnerText;
					}

					if (string.IsNullOrEmpty(url))
						continue;

					if (!url.EndsWith(POST_XML))
						continue;

					var dateNode = node.SelectSingleNode("x:lastmod", nsmgr);
					if (dateNode != null)
					{
						string dateVal = dateNode.InnerText;
						if (!string.IsNullOrWhiteSpace(dateVal))
						{
							if (DateTime.TryParse(dateVal, out DateTime dt))
							{
								var page = new URLInfo();
								page.URL = url;
								page.LastModified = dt;
								return page;
							}
						}
					}

				}//for
			}//if

			return null;
		}


	}
}
