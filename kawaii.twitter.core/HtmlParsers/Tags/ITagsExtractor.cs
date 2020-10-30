using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.HtmlParsers.Tags
{
	public interface ITagsExtractor
	{
		/// <summary>
		/// Из тела html-поста извлечь теги
		/// </summary>
		/// <param name="htmlBody"></param>
		/// <returns>Массив тегов или null если не найдены</returns>
		TagInfo[] LoadTagsFromHtmlBody(string htmlBody);
	}
}
