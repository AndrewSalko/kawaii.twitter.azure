using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.FindPageForBlob
{
	public interface IBlobNameToURLPart
	{
		/// <summary>
		/// По имени блоб-записи - разбивает ее на части, и формирует подстроку для поиска
		/// </summary>
		/// <param name="blobName">folder-name:imgname.gif</param>
		/// <returns>"folder-name/" - в таком виде можно делать поиск ends-with</returns>
		string GetURLPart(string blobName);
	}
}
