using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.BlobName
{
	public class Formatter: IFormatter
	{
		public Formatter()
		{
		}

		/// <summary>
		/// Вернет префикс имени блоба для заданной папки (поста)
		/// </summary>
		/// <param name="postFolderName">Имя папки можно получить через IFolderFromURL</param>
		/// <returns></returns>
		public string GetBlobNamePrefix(string postFolderName)
		{
			string blobPrefix = string.Format("{0}:", postFolderName);
			return blobPrefix;
		}

	}
}
