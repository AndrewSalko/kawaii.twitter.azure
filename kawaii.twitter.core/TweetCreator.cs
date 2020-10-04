using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.blob;
using kawaii.twitter.core.HtmlParsers;
using kawaii.twitter.core.SelectLogic;
using kawaii.twitter.core.Text;
using kawaii.twitter.core.TwitterService;

namespace kawaii.twitter.core
{
	/// <summary>
	/// Генерація твітів (з бази та анімованих зображень)
	/// </summary>
	public class TweetCreator
	{
		IPageForTwittingSelector _Selector;
		IService _TwitterService;

		ITwitterTextCreator _TwitterTextCreator;

		ITwitterImageURL _TwitterImageURL;

		IImageOnWeb _ImageOnWeb;

		IBlobDownload _BlobDownload;

		ILastTweetUpdater _LastTweetUpdater;

		public TweetCreator(IPageForTwittingSelector selector, ITwitterTextCreator twitterTextCreator, ITwitterImageURL twitterImageURL, IImageOnWeb imageOnWeb, IBlobDownload blobDownload, IService twitterService, ILastTweetUpdater lastTweetUpdater)
		{
			_Selector = selector ?? throw new ArgumentNullException(nameof(selector));
			_TwitterTextCreator = twitterTextCreator ?? throw new ArgumentNullException(nameof(twitterTextCreator));
			_TwitterImageURL = twitterImageURL ?? throw new ArgumentNullException(nameof(twitterImageURL));
			_ImageOnWeb = imageOnWeb ?? throw new ArgumentNullException(nameof(imageOnWeb));
			_BlobDownload = blobDownload ?? throw new ArgumentNullException(nameof(blobDownload));
			_TwitterService = twitterService ?? throw new ArgumentNullException(nameof(twitterService));
			_LastTweetUpdater = lastTweetUpdater ?? throw new ArgumentNullException(nameof(lastTweetUpdater));
		}

		public async Task Execute()
		{
			//получаем вариант твита из селектора...
			TwittData data = await _Selector.GetPageForTwitting();

			string imageFileName;
			byte[] imageBody;

			string url = data.Page.URL;

			if (data.Image != null)
			{
				//нужно сделать твит но изображение берем из указанного,тело файла - его надо получить из блоб-сервиса
				imageFileName = data.Image.GetFileName();
				imageBody = await _BlobDownload.GetBlobBody(data.Image.BlobName);
			}
			else
			{
				//изображение твита будем брать случайное прямо из поста
				string imgURL = await _TwitterImageURL.GetTwitterImageFileURL(url);
				if (string.IsNullOrEmpty(imgURL))
					throw new ApplicationException("Can't found image url for post:" + url);

				//скачать из инета изображение.. нужно его имя файла и тело
				ImageInfo imgInfo = await _ImageOnWeb.Download(imgURL);

				imageFileName = imgInfo.FileName;
				imageBody = imgInfo.Body;
			}
			
			string postTitle = data.Page.Title;

			string tweetText = _TwitterTextCreator.CreateTwitterText(url, postTitle);

			await _TwitterService.TweetWithMedia(tweetText, imageFileName, imageBody);

			//после успешного твита обновить в базе даты
			_LastTweetUpdater.UpdateLastTweetDate(data);
		}


	}
}
