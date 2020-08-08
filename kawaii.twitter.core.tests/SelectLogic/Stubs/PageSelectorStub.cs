using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Page;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class PageSelectorStub : IPageSelector
	{
		public Task<SitePage> GetPageForTwitting()
		{
			throw new NotImplementedException();
		}
	}
}
