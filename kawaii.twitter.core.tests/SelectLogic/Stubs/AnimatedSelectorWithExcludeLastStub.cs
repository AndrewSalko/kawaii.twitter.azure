using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.Images.ExcludeUsed;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class AnimatedSelectorWithExcludeLastStub : IAnimatedSelectorWithExcludeLast
	{
		public AnimatedImage SelectImage(AnimatedImage[] images)
		{
			throw new NotImplementedException();
		}
	}
}
