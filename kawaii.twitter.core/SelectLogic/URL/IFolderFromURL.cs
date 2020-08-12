using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.URL
{
	public interface IFolderFromURL
	{
		/// <summary>
		/// Для полного url вернет часть отвечающую за "папку" страницы (это важно т.к. она будет префиксом в блоб-именах)
		/// </summary>
		/// <param name="pageURL">Полный url страницы (например: https://kawaii-mobile.com/2020/08/princess-connect-redive/)</param>
		/// <returns>Последняя часть - princess-connect-redive</returns>
		string GetFolderFromURL(string pageURL);

	}
}
