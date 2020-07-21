using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kawaii.twitter.core.Text
{
	class PostText
	{
		public PostText(string url, string title)
		{
			URL = url;
			Title = title;
		}

		public string URL
		{
			get;
			private set;
		}

		public string Title
		{
			get;
			private set;
		}

		/// <summary>
		/// Генерирует текст для твита для данной страницы
		/// </summary>
		/// <returns></returns>
		public string CreateTwitterText()
		{
			//формируем текст для твита данной страницы
			string urlAndTags = string.Format("{0}{1}{2}", URL, Environment.NewLine, _GetRandomHashTags());
			string complexText = string.Empty;
			if (!string.IsNullOrWhiteSpace(Title))
				complexText = Title;

			complexText += " ";
			complexText += urlAndTags;
			if (complexText.Length < 140)
			{
				return complexText;
			}
			else
			{
				//проверим, может без хеш-тегов выйдет?
				string complexText2 = string.Format("{0} {1}", Title, URL);
				if (complexText2.Length < 140)
				{
					return complexText2;
				}
			}

			return urlAndTags;
		}

		string _GetRandomHashTags()
		{
			string[] animeHashTags = new string[] { "#anime", "#animewallpaper", "#animegirl" };
			string[] hashTags = new string[] { "#otaku", "#animelover", "#smartphonewallpaper", "#iphone", "#smartphone" };
			Random rnd = new Random(Environment.TickCount);

			List<string> resultList = new List<string>();

			int ind = rnd.Next(animeHashTags.Length);
			string tag = animeHashTags[ind];
			resultList.Add(tag);

			//один хеш тег из одного списка, второй из другого
			ind = rnd.Next(hashTags.Length);
			tag = hashTags[ind];
			resultList.Add(tag);

			string result = string.Join(" ", resultList.ToArray());

			return result;
		}

	}
}
