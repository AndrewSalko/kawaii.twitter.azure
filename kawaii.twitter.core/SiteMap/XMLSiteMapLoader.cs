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
	public class XMLSiteMapLoader
	{
		HttpClient _HttpClient;

		public XMLSiteMapLoader(HttpClient httpClient)
		{
			_HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		}

		public async Task<URLInfo[]> Load(string xmlSiteMapURL)
		{
			List<URLInfo> result=new List<URLInfo>();

			string xmlBody = await _HttpClient.GetStringAsync(xmlSiteMapURL);

			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlBody);

			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
			nsmgr.AddNamespace("x", "http://www.sitemaps.org/schemas/sitemap/0.9");

			//выбираем все ноды <url>
			XmlNodeList urlNodes = xmlDoc.SelectNodes("//x:url", nsmgr);
			if (urlNodes.Count > 0)
			{
				//int totalNodes = urlNodes.Count;
				//int processed = 0;

				for (int i = 0; i < urlNodes.Count; i++)
				{
					//if (worker.CancellationPending)
					//{
					//	log.Log("Загрузка отменена");
					//	_ResultPages.Clear();
					//	return;
					//}

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

					if (page.URL != null && page.LastModified != DateTime.MinValue)
					{
						//получить тайтл
						string title = await _GetTitle(page.URL);
						if (title != null)
						{
							page.Title = title;
						}

						result.Add(page);

						//processed++;
						//log.Log("Загружено {0} из {1} страниц ({2})", processed, totalNodes, page.Title);
						//if (DelayIntervalTitleRequest != 0)
						//{
						//	System.Threading.Thread.Sleep(DelayIntervalTitleRequest);
						//}
					}

					//если лимит равен 0, то условие не выполнится никогда и цикл идет целиком
					if (i + 1 == LimitCount)
					{
						break;
					}

				}//for
			}//if

			return result.ToArray();
		}

		//string _FileName;
		//List<SitePage> _ResultPages = new List<SitePage>();

		/// <summary>
		/// Задержка между запросами к сайту по тайтлу (в мс)
		/// </summary>
		public int DelayIntervalTitleRequest
		{
			get;
			set;
		}

		//public XMLSiteMapLoader(string fileName)
		//{
		//	if (string.IsNullOrWhiteSpace(fileName))
		//		throw new ArgumentException("fileName");

		//	_FileName = fileName;
		//}

		//public SitePage[] Pages
		//{
		//	get
		//	{
		//		if (_ResultPages.Count == 0)
		//			return null;

		//		return _ResultPages.ToArray();
		//	}
		//}

		public int LimitCount
		{
			get;
			set;
		}

		//internal void DoWork(BackgroundWorker worker, ILog log)
		//{
		//	var xmlDoc = new XmlDocument();
		//	xmlDoc.Load(_FileName);

		//	XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
		//	nsmgr.AddNamespace("x", "http://www.sitemaps.org/schemas/sitemap/0.9");

		//	//выбираем все ноды <url>						
		//	XmlNodeList urlNodes = xmlDoc.SelectNodes("//x:url", nsmgr);
		//	if (urlNodes.Count > 0)
		//	{
		//		int totalNodes = urlNodes.Count;
		//		int processed = 0;

		//		for (int i = 0; i < urlNodes.Count; i++)
		//		{
		//			if (worker.CancellationPending)
		//			{
		//				log.Log("Загрузка отменена");
		//				_ResultPages.Clear();
		//				return;
		//			}

		//			var node = urlNodes[i];

		//			SitePage page = new SitePage();

		//			var locNode = node.SelectSingleNode("x:loc", nsmgr);
		//			if (locNode != null)
		//			{
		//				page.URL = locNode.InnerText;
		//			}

		//			var dateNode = node.SelectSingleNode("x:lastmod", nsmgr);
		//			if (dateNode != null)
		//			{
		//				DateTime dt;
		//				string dateVal = dateNode.InnerText;
		//				if (!string.IsNullOrWhiteSpace(dateVal))
		//				{
		//					if (DateTime.TryParse(dateVal, out dt))
		//					{
		//						page.LastModified = dt;
		//					}
		//				}
		//			}

		//			if (page.URL != null && page.LastModified != DateTime.MinValue)
		//			{
		//				//получить тайтл
		//				string title = _GetTitle(page.URL);
		//				if (title != null)
		//				{
		//					page.Title = title;
		//				}

		//				_ResultPages.Add(page);

		//				processed++;
		//				log.Log("Загружено {0} из {1} страниц ({2})", processed, totalNodes, page.Title);

		//				if (DelayIntervalTitleRequest != 0)
		//				{
		//					System.Threading.Thread.Sleep(DelayIntervalTitleRequest);
		//				}
		//			}

		//			//если лимит равен 0, то условие не выполнится никогда и цикл идет целиком
		//			if (i + 1 == LimitCount)
		//			{
		//				break;
		//			}

		//		}//for
		//	}//if
		//}

		async Task<string> _GetTitle(string url)
		{
			string result = null;
			try
			{
				string htmlBody = await _HttpClient.GetStringAsync(url);

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