using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.URL
{
	public class FolderFromURL: IFolderFromURL
	{
		public FolderFromURL()
		{
		}

		/// <summary>
		/// Для полного url вернет часть отвечающую за "папку" страницы (это важно т.к. она будет префиксом в блоб-именах)
		/// </summary>
		/// <param name="pageURL"></param>
		/// <returns></returns>
		public string GetFolderFromURL(string pageURL)
		{
			if (string.IsNullOrEmpty(pageURL))
				throw new ArgumentNullException(nameof(pageURL));

			Uri uri = new Uri(pageURL);
			var segments = uri.Segments;

			//взять послед.часть
			string slug = segments[segments.Length - 1];

			if (slug.EndsWith("/"))
			{
				slug = slug.TrimEnd('/');
			}

			if (string.IsNullOrEmpty(slug))
				throw new ApplicationException("Invalid url, can't detect folder:" + pageURL);

			return slug;
		}

	}
}
