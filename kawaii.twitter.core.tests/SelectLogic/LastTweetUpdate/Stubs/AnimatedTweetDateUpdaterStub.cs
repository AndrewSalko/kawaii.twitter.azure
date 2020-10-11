using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db;
using kawaii.twitter.db.TweetDateUpdaters;

namespace kawaii.twitter.core.tests.SelectLogic.LastTweetUpdate.Stubs
{
	class AnimatedTweetDateUpdaterStub : IAnimatedTweetDateUpdater
	{
		public bool DontThrowNotImpl
		{
			get;
			set;
		}

		public AnimatedImage CalledImage
		{
			get;
			private set;
		}

		public DateTime CalledDate
		{
			get;
			private set;
		}

		public void UpdateTweetDateForBlob(AnimatedImage img, DateTime date)
		{
			if (!DontThrowNotImpl)
				throw new NotImplementedException();

			CalledImage = img;
			CalledDate = date;
		}
	}
}
