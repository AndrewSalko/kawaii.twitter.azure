using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db.TweetDateUpdaters;
using kawaii.twitter.Now;

namespace kawaii.twitter.core.SelectLogic
{
	public class LastTweetUpdater : ILastTweetUpdater
	{
		IDateSupply _DateSupply;
		IAnimatedTweetDateUpdater _AnimatedTweetDateUpdater;
		ISitePageTweetDateUpdater _SitePageTweetDateUpdater;

		public LastTweetUpdater(IDateSupply dateSupply, IAnimatedTweetDateUpdater animatedTweetDateUpdater, ISitePageTweetDateUpdater sitePageTweetDateUpdater)
		{
			_DateSupply = dateSupply ?? throw new ArgumentNullException(nameof(dateSupply));
			_AnimatedTweetDateUpdater = animatedTweetDateUpdater ?? throw new ArgumentNullException(nameof(animatedTweetDateUpdater));
			_SitePageTweetDateUpdater = sitePageTweetDateUpdater ?? throw new ArgumentNullException(nameof(sitePageTweetDateUpdater));
		}

		public void UpdateLastTweetDate(TwittData data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (data.Page == null)
				throw new ArgumentNullException("data.Page==null", nameof(data));

			var page = data.Page;

			var now = _DateSupply.Now;
			page.TweetDate = now;

			//обновить пост в базе...
			_SitePageTweetDateUpdater.UpdateTweetDateForPage(page, now);

			//если было использовано изображение
			var img = data.Image;
			if (img != null)
			{
				img.TweetDate = now;

				//обновить в базе изображений...
				_AnimatedTweetDateUpdater.UpdateTweetDateForBlob(img, now);
			}

		}
	}
}
