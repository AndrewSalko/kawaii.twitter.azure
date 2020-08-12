using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.BlobName
{
	public interface IFormatter
	{
		/// <summary>
		/// Вернет префикс имени блоба для заданной папки (поста)
		/// </summary>
		/// <param name="postFolderName">Имя папки можно получить через IFolderFromURL</param>
		/// <returns></returns>
		string GetBlobNamePrefix(string postFolderName);

	}
}
