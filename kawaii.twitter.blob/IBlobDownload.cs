using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.blob
{
	public interface IBlobDownload
	{
		/// <summary>
		/// Загрузить тело блоба по его имени
		/// </summary>
		/// <param name="blobName"></param>
		/// <returns></returns>
		Task<byte[]> GetBlobBody(string blobName);
	}
}
