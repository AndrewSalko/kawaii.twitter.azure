using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class FindPageByBlobNameStub : BaseStubWithImpl, IFindPageByBlobName
	{
		public SitePage Result
		{
			get;
			set;
		}

		public string UsedBlobNameForFind
		{
			get;
			private set;
		}

		public async Task<SitePage> Find(string blobName)
		{
			if (!DontThrowNotImpl)
				throw new NotImplementedException();

			UsedBlobNameForFind = blobName;	//для само-проверки

			return await Task.Run(() =>
			{
				return Result;
			});
		}
	}
}
