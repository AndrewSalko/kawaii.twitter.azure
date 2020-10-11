using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.tests.TweetCreator.Stubs
{
	class TwitterServiceStub: TwitterService.IService
	{
		public TwitterServiceStub()
		{
		}

		public string ResultText
		{
			get;
			private set;
		}

		public string ResultImageFileName
		{
			get;
			private set;
		}

		public byte[] ResultImageFileBody
		{
			get;
			private set;
		}


		public async Task TweetWithMedia(string tweetText, string imageFileName, byte[] imageFileBody)
		{
			Task task = new Task(() =>
			{
				ResultText = tweetText;
				ResultImageFileName = imageFileName;
				ResultImageFileBody = imageFileBody;
			});
			task.Start();

			await task;
		}
	}
}
