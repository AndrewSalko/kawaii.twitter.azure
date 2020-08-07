using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.SelectLogic
{
	public interface IPageForTwittingSelector
	{
		/// <summary>
		/// Выбирает, какую страницу будем твитить, и если нужно, использует внеш.изображение (.gif) - см.TwittData
		/// </summary>
		/// <returns></returns>
		Task<TwittData> GetPageForTwitting();
	}
}
