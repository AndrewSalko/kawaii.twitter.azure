using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.Now
{
	public interface IDateSupply
	{
		/// <summary>
		/// Поточна дата-час
		/// </summary>
		DateTime Now
		{
			get;
		}
	}
}
