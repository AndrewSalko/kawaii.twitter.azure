using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.blob;
using kawaii.twitter.core.HtmlParsers;
using kawaii.twitter.core.SelectLogic;
using kawaii.twitter.core.Text;
using kawaii.twitter.core.TwitterService;
using kawaii.twitter.Logs;

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

		ILogger _Log;

		public TweetCreator(IPageForTwittingSelector selector, ITwitterTextCreator twitterTextCreator, ITwitterImageURL twitterImageURL, IImageOnWeb imageOnWeb, IBlobDownload blobDownload, IService twitterService, ILastTweetUpdater lastTweetUpdater, ILogger log)
		{
			_Selector = selector ?? throw new ArgumentNullException(nameof(selector));
			_TwitterTextCreator = twitterTextCreator ?? throw new ArgumentNullException(nameof(twitterTextCreator));
			_TwitterImageURL = twitterImageURL ?? throw new ArgumentNullException(nameof(twitterImageURL));
			_ImageOnWeb = imageOnWeb ?? throw new ArgumentNullException(nameof(imageOnWeb));
			_BlobDownload = blobDownload ?? throw new ArgumentNullException(nameof(blobDownload));
			_TwitterService = twitterService ?? throw new ArgumentNullException(nameof(twitterService));
			_LastTweetUpdater = lastTweetUpdater ?? throw new ArgumentNullException(nameof(lastTweetUpdater));
			_Log = log ?? throw new ArgumentNullException(nameof(log));
		}

		public async Task Execute()
		{
			//получаем вариант твита из селектора...
			TwittData data = await _Selector.GetPageForTwitting();

			_Log.Log("_Selector.GetPageForTwitting done: {0}", DateTime.Now);

			string imageFileName;
			byte[] imageBody;

			string url = data.Page.URL;

			if (data.Image != null)
			{
				_Log.Log("External gif image will be used: {0}", DateTime.Now);

				//нужно сделать твит но изображение берем из указанного,тело файла - его надо получить из блоб-сервиса
				imageFileName = data.Image.GetFileName();
				imageBody = await _BlobDownload.GetBlobBody(data.Image.BlobName);

				_Log.Log("External gif image downloaded: {0}", DateTime.Now);
			}
			else
			{
				_Log.Log("Post image will be used: {0}", DateTime.Now);

				//изображение твита будем брать случайное прямо из поста
				string imgURL = await _TwitterImageURL.GetTwitterImageFileURL(url);
				if (string.IsNullOrEmpty(imgURL))
					throw new ApplicationException("Can't found image url for post:" + url);

				_Log.Log("Post image - _ImageOnWeb.Download start: {0}", DateTime.Now);

				//скачать из инета изображение.. нужно его имя файла и тело
				ImageInfo imgInfo = await _ImageOnWeb.Download(imgURL);

				imageFileName = imgInfo.FileName;
				imageBody = imgInfo.Body;

				_Log.Log("Post image downloaded: {0}", DateTime.Now);
			}
			
			string postTitle = data.Page.Title;

			//тайтл берем из базы, но изначально он был взят из html-тела, убираем спец.коды html
			string postTitleDecoded = WebUtility.HtmlDecode(postTitle);

			string tweetText = _TwitterTextCreator.CreateTwitterText(url, postTitleDecoded);

			await _TwitterService.TweetWithMedia(tweetText, imageFileName, imageBody);

			_Log.Log("TweetWithMedia done: {0}", DateTime.Now);

			//после успешного твита обновить в базе даты
			_LastTweetUpdater.UpdateLastTweetDate(data);

			_Log.Log("UpdateLastTweetDate done: {0}", DateTime.Now);
		}


	}
}
