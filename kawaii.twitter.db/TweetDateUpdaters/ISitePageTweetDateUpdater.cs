using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.db.TweetDateUpdaters
{
	public interface ISitePageTweetDateUpdater
	{
		void UpdateTweetDateForPage(SitePage page, DateTime date);
	}
}
