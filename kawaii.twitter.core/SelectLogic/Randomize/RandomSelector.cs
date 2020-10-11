using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.Randomize
{
	public class RandomSelector: IRandomSelector
	{
		Random _Rnd;

		public RandomSelector()
		{
			_Rnd = new Random(Environment.TickCount);
		}

		public int GetRandomIndex(int length)
		{
			int result = _Rnd.Next(length);
			return result;
		}

	}
}
