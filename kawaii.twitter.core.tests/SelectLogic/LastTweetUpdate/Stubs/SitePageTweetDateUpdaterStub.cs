using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db;
using kawaii.twitter.db.TweetDateUpdaters;

namespace kawaii.twitter.core.tests.SelectLogic.LastTweetUpdate.Stubs
{
	class SitePageTweetDateUpdaterStub: ISitePageTweetDateUpdater
	{
		public bool DontThrowNotImpl
		{
			get;
			set;
		}

		public SitePage CalledPage
		{
			get;
			private set;
		}

		public DateTime CalledDate
		{
			get;
			private set;
		}


		public void UpdateTweetDateForPage(SitePage page, DateTime date)
		{
			if (!DontThrowNotImpl)
				throw new NotImplementedException();

			CalledPage = page;
			CalledDate = date;
		}
	}
}
