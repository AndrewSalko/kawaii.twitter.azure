using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class FindAnimatedByPageStub : IFindAnimatedByPage
	{
		public Task<AnimatedImage[]> GetAnimatedImagesForPage(string pageURL)
		{
			throw new NotImplementedException();
		}
	}
}
