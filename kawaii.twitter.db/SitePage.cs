using System;
using MongoDB.Bson.Serialization.Attributes;

namespace kawaii.twitter.db
{
	/// <summary>
	/// Страница сайта
	/// </summary>
	public class SitePage
	{

		[BsonIgnoreIfNull]
		public string URL
		{
			get;
			set;
		}

		/// <summary>
		/// Дата модификации страницы (по карте сайта)
		/// </summary>
		[BsonIgnoreIfNull]
		public DateTime? LastModified
		{
			get;
			set;
		}

		/// <summary>
		/// Тайтл страницы
		/// </summary>
		[BsonIgnoreIfNull]
		public string Title
		{
			get;
			set;
		}

		/// <summary>
		/// Дата, когда мы твитили последний раз эту страницу
		/// </summary>
		[BsonIgnoreIfNull]
		public DateTime? TweetDate
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
