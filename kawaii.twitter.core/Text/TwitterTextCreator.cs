using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.Text
{
	/// <summary>
	/// Генерує повний текст для твіта (с URL та тайтлом), також додає хеш-теги
	/// </summary>
	public class TwitterTextCreator: ITwitterTextCreator
	{
		public string CreateTwitterText(string url, string title)
		{
			PostText text = new PostText(url, title);
			return text.CreateTwitterText();
		}

	}
}
