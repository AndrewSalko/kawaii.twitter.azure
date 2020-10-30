using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.HtmlParsers.Tags
{
	public class TagsExtractor: ITagsExtractor
	{
		public const string REL_TAG = "rel=\"tag\"";
		public const char OPEN_NODE = '<';
		public const char CLOSE_NODE = '>';

		public TagInfo[] LoadTagsFromHtmlBody(string htmlBody)
		{
			if (string.IsNullOrEmpty(htmlBody))
				throw new ArgumentNullException(nameof(htmlBody));

			//найти все теги, по <a href="https://kawaii-mobile.com/tag/chizuru-mizuhara/" rel="tag">Chizuru Mizuhara</a>

			int searchIndex = 0;
			int blockIndex = htmlBody.IndexOf(REL_TAG, searchIndex);

			List<TagInfo> tags = new List<TagInfo>();

			while (blockIndex >= 0)
			{
				//завершить блок 

				int urlIndex = blockIndex + REL_TAG.Length;
				bool stopFound = false;

				bool beginTagBody = false;

				StringBuilder sb = new StringBuilder();
				while (!stopFound)
				{
					char c = htmlBody[urlIndex];
					if (c == CLOSE_NODE)
					{
						//начать накопление текста (имя тега)
						beginTagBody = true;
						urlIndex++;
						continue;
					}
					
					if (c == OPEN_NODE)
					{
						urlIndex++;
						break;  //нод открывается - мы закончили
					}

					if (beginTagBody)
					{
						sb.Append(c);
					}

					urlIndex++;
				}

				if (beginTagBody)
				{
					string tagName = sb.ToString();
					TagInfo tagInfo = new TagInfo(tagName);
					tags.Add(tagInfo);
				}

				//теперь ищем след.блок
				searchIndex = urlIndex + 1;
				blockIndex = htmlBody.IndexOf(REL_TAG, searchIndex);

			}//while

			if (tags.Count == 0)
				return null;

			return tags.ToArray();
		}

	}
}
