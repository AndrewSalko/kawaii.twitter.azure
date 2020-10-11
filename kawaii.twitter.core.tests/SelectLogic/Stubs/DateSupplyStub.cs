using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.Now;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class DateSupplyStub : IDateSupply
	{
		public DateTime Now
		{
			get;
			set;
		}
	}
}
