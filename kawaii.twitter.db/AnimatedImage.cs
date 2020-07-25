using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace kawaii.twitter.db
{
	/// <summary>
	/// Для хранения информации про аним. gif-файл. Каждый из них принадлежит отдельному посту
	/// </summary>
	public class AnimatedImage
	{

		/// <summary>
		/// URL сайта, ассоциированный с этим изображением
		/// </summary>
		[BsonRequired]
		public string URL
		{
			get;
			set;
		}

		/// <summary>
		/// Имя файла (без пути)
		/// </summary>
		[BsonRequired]
		public string FileName
		{
			get;
			set;
		}

		/// <summary>
		/// Имя блоб-данных (где оно хранится). В простейшем случае это у нас имя файла, т.к. не ожидается что оно будет повторяться,
		/// но мы добавим впереди "slug" от URL
		/// </summary>
		[BsonRequired]
		public string BlobName
		{
			get;
			set;
		}

		public static string GetBlobNameFromFileName(string fullFileName, string url)
		{
			Uri uri = new Uri(url);
			string slug = uri.Segments.Last();
			string fileName = Path.GetFileName(fullFileName);

			string result = slug + "_" + fileName;
			return result;
		}

	}
}
