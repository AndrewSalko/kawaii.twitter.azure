using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.PageOrExternalImage;

namespace kawaii.twitter.core.tests.SelectLogic.Stubs
{
	class PageOrExternalImageSelectorStub : BaseStubWithImpl, IPageOrExternalImageSelector
	{
		bool _UseExternalAnimatedImage;

		public bool UseExternalAnimatedImage
		{
			get
			{
				if (!DontThrowNotImpl)
					throw new NotImplementedException();

				return _UseExternalAnimatedImage;
			}
			set
			{
				_UseExternalAnimatedImage = value;
			}
		}
	}
}
