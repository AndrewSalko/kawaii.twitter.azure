using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Images;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class AnimatedSelectorStub : IAnimatedSelector
	{
		public Task<AnimatedImage> GetAnimatedImageForTwitting()
		{
			throw new NotImplementedException();
		}
	}
}
