using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class FindAnimatedByPageStub : BaseStubWithImpl, IFindAnimatedByPage
	{
		public AnimatedImage[] Result
		{
			get;
			set;
		}

		public string RequestedURL
		{
			get;
			set;
		}

		public async Task<AnimatedImage[]> GetAnimatedImagesForPage(string pageURL)
		{
			if (!DontThrowNotImpl)
				throw new NotImplementedException();

			RequestedURL = pageURL;

			return await Task.Run(() =>
			{
				return Result;
			});
		}
	}
}
