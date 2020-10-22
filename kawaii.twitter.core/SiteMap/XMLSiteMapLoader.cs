using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace kawaii.twitter.core.SiteMap
{
	/// <summary>
	///	Завантажує та парсить карту сайта (зазвичай це карта постів)
	/// https://kawaii-mobile.com/post.xml
	/// </summary>
	public class XMLSiteMapLoader: IXMLSiteMapLoader
	{
		ISiteMapWebDownloader _SiteMapWebDownloader;
		IPostBodyLoader _PostBodyLoader;

		public XMLSiteMapLoader(ISiteMapWebDownloader siteMapWebDownloader, IPostBodyLoader postBodyLoader)
		{
			_SiteMapWebDownloader = siteMapWebDownloader ?? throw new ArgumentNullException(nameof(siteMapWebDownloader));
			_PostBodyLoader = postBodyLoader ?? throw new ArgumentNullException(nameof(postBodyLoader));
		}

		public async Task<URLInfo[]> Load(string xmlSiteMapURL)
		{
			List<URLInfo> result=new List<URLInfo>();

			string xmlBody = await _SiteMapWebDownloader.DownloadSiteMapBody(xmlSiteMapURL);

			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlBody);

			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
			nsmgr.AddNamespace("x", "http://www.sitemaps.org/schemas/sitemap/0.9");

			//выбираем все ноды <url>
			XmlNodeList urlNodes = xmlDoc.SelectNodes("//x:url", nsmgr);
			if (urlNodes.Count > 0)
			{
				for (int i = 0; i < urlNodes.Count; i++)
				{
					var node = urlNodes[i];

					var page = new URLInfo();

					var locNode = node.SelectSingleNode("x:loc", nsmgr);
					if (locNode != null)
					{
						page.URL = locNode.InnerText;
					}

					var dateNode = node.SelectSingleNode("x:lastmod", nsmgr);
					if (dateNode != null)
					{
						DateTime dt;
						string dateVal = dateNode.InnerText;
						if (!string.IsNullOrWhiteSpace(dateVal))
						{
							if (DateTime.TryParse(dateVal, out dt))
							{
								page.LastModified = dt;
							}
						}
					}

					if (page.URL == null || page.LastModified == DateTime.MinValue)
						continue;

					//получить тайтл
					string title = await _GetTitle(page.URL);
					if (title != null)
					{
						page.Title = title;
					}

					result.Add(page);
					

					//если лимит равен 0, то условие не выполнится никогда и цикл идет целиком
					if (i + 1 == LimitCount)
					{
						break;
					}

				}//for
			}//if

			return result.ToArray();
		}

		public int LimitCount
		{
			get;
			set;
		}

		async Task<string> _GetTitle(string url)
		{
			string result = null;
			try
			{
				string htmlBody = await _PostBodyLoader.GetHtmlBodyForURL(url);

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

			}
			catch (Exception)
			{
			}
			return result;
		}

	}
}
