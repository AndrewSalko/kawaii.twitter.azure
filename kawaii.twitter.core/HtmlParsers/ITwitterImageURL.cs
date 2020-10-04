using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.HtmlParsers
{
	public interface ITwitterImageURL
	{
		/// <summary>
		/// Для окремої сторінки (поста) де кілька зображень обирає одне випадковим чином,
		/// та повертає її twitter-URL (посилання на повнорозмірне зображення)
		/// </summary>
		/// <param name="pageURL"></param>
		/// <returns></returns>
		Task<string> GetTwitterImageFileURL(string pageURL);
	}
}
