using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.HtmlParsers
{
	/// <summary>
	/// По заданному URL для основного поста получает из его html-кода все ссылки на другие изображения,
	/// а затем - выберет случайное изображение оттуда
	/// </summary>
	class AttachPagesLoader
	{
		const string _HREF_PART = "href='";
		const char _END_APOSTROF = '\'';
		const char _END_QUOTE = '\"';

		string _URL;
		string _HTMLBody;

		public AttachPagesLoader(string url, string htmlBody)
		{
			_URL = url;
			_HTMLBody = htmlBody;
		}

		public string[] GetAttachImagePagesURLs()
		{
			//нужно найти все ссылки с базовым url ?
			string searchPattern = $"href='{_URL}";

			int searchIndex = 0;
			int blockIndex = _HTMLBody.IndexOf(searchPattern, searchIndex);

			List<string> attachURLs = new List<string>();

			while (blockIndex >= 0)
			{
				//завершить блок до первого апострофа или кавычки

				int urlIndex = blockIndex + _HREF_PART.Length;
				bool stopFound = false;

				StringBuilder sb = new StringBuilder();
				while (!stopFound)
				{
					char c = _HTMLBody[urlIndex];
					if (c == _END_APOSTROF || c == _END_QUOTE)
					{
						break;
					}

					sb.Append(c);
					urlIndex++;
				}

				string attachURL = sb.ToString();
				attachURLs.Add(attachURL);

				//теперь ищем след.блок
				searchIndex = urlIndex + 1;
				blockIndex = _HTMLBody.IndexOf(searchPattern, searchIndex);

			}//while

			if (attachURLs.Count > 0)
			{
				return attachURLs.ToArray();
			}

			return null;
		}

	}
}
