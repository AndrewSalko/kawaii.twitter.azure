using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.Randomize
{
	public interface IRandomSelector
	{
		/// <summary>
		/// Отримати випадковий індекс від 0 до length-1
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		int GetRandomIndex(int length);
	}
}
