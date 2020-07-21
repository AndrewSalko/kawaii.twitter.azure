using System;

namespace kawaii.twitter.db
{
	/// <summary>
	/// Страница сайта
	/// </summary>
	public class SitePage
	{
	
		public string URL
		{
			get;
			set;
		}

		/// <summary>
		/// Дата модификации страницы (по карте сайта)
		/// </summary>
		public DateTime LastModified
		{
			get;
			set;
		}

		/// <summary>
		/// Тайтл страницы
		/// </summary>
		public string Title
		{
			get;
			set;
		}

		/// <summary>
		/// Дата, когда мы твитили последний раз эту страницу
		/// </summary>
		public DateTime TweetDate
		{
			get;
			set;
		}

		/// <summary>
		/// Блокировать (не твитить)
		/// </summary>
		public bool Blocked
		{
			get;
			set;
		}

		public override string ToString()
		{
			return URL;
		}
	}
}
