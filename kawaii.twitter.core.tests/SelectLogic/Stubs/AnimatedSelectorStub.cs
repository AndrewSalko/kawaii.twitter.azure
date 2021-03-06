using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Images;
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class AnimatedSelectorStub : BaseStubWithImpl, IFindAnimatedByPage
	{
		public AnimatedImage[] Result
		{
			get;
			set;
		}

		public async Task<AnimatedImage[]> GetAnimatedImagesForPage(string pageURL)
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
