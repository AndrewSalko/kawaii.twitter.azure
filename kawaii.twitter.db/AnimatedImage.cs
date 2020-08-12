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
		/// Имя блоб-данных (где оно хранится). Формат: "slug:fileName"
		/// Например для поста https://kawaii-mobile.com/2012/10/accel-world/
		/// будет иметь вид:  accel-world:file1.gif
		/// </summary>
		[BsonRequired]
		public string BlobName
		{
			get;
			set;
		}

		/// <summary>
		/// Дата, когда мы твитили это изображение
		/// </summary>
		[BsonIgnoreIfNull]
		public DateTime? TweetDate
		{
			get;
			set;
		}

		public override string ToString()
		{
			if (TweetDate == null)
			{
				return BlobName;
			}

			string dispName = string.Format("{0} {1:yyyy_MM_dd__HH_mm_ss}", BlobName, TweetDate);
			return dispName;
		}

	}
}
