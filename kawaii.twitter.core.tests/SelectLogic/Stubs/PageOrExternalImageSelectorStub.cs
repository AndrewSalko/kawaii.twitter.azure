using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.PageOrExternalImage;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class PageOrExternalImageSelectorStub : IPageOrExternalImageSelector
	{
		public bool UseExternalAnimatedImage => throw new NotImplementedException();
	}
}
