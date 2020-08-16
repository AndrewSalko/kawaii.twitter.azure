using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.Randomize;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class RandomSelectorStub : IRandomSelector
	{
		/// <summary>
		/// Этот индекс будет выдан как результат
		/// </summary>
		public int Result
		{
			get;
			set;
		}

		/// <summary>
		/// Для проверок - аргумент, переданный в GetRandomIndex
		/// </summary>
		public int LengthArg
		{
			get;
			private set;
		}

		public int GetRandomIndex(int length)
		{
			LengthArg = length;
			return Result;
		}
	}
}
