using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.Now;

namespace kawaii.twitter.core.Env
{
	public class DateSupply : IDateSupply
	{
		public DateSupply()
		{
		}

		public DateTime Now => DateTime.Now;
	}
}
