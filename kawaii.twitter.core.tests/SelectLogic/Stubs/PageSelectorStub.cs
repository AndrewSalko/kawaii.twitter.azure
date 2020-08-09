using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Page;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class PageSelectorStub : BaseStubWithImpl, IPageSelector
	{
		public SitePage Result
		{
			get;
			set;
		}

		public async Task<SitePage> GetPageForTwitting()
		{
			if (!DontThrowNotImpl)
				throw new NotImplementedException();

			return await Task.Run(() =>
			{
				return Result;
			});
		}
	}
}
