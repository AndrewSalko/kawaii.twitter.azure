using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class FindPageByBlobNameStub : IFindPageByBlobName
	{
		public Task<SitePage> Find(string blobName)
		{
			throw new NotImplementedException();
		}
	}
}
