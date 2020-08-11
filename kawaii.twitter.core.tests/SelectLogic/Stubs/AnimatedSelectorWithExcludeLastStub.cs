using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.Images.ExcludeUsed;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class AnimatedSelectorWithExcludeLastStub : BaseStubWithImpl, IAnimatedSelectorWithExcludeLast
	{
		public AnimatedImage Result
		{
			get;
			set;
		}

		public AnimatedImage SelectImage(AnimatedImage[] images)
		{
			if (!DontThrowNotImpl)
				throw new NotImplementedException();

			return Result;
		}
	}
}
