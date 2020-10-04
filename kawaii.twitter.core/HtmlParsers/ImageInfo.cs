using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.HtmlParsers
{
	public class ImageInfo
	{
		/// <summary>
		/// Имя файла (без пути)
		/// </summary>
		public string FileName
		{
			get;
			set;
		}

		/// <summary>
		/// Тело файла
		/// </summary>
		public byte[] Body
		{
			get;
			set;
		}

	}
}
