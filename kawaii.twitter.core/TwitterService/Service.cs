using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;


namespace kawaii.twitter.core.TwitterService
{
	/// <summary>
	/// Створення твітів, працює з твіттер-API
	/// </summary>
	public class Service: IService
	{
		const string _MEDIA_CATEGORY_IMAGE = "tweet_image";
		const string _MEDIA_CATEGORY_GIF = "tweet_gif";

		const string _GIF_EXT = "gif";

		public const string ENV_API_KEY = "kawaii_twitter_api_key";
		public const string ENV_API_SECRET = "kawaii_twitter_api_secret";
		public const string ENV_API_TOKEN = "kawaii_twitter_api_token";
		public const string ENV_API_TOKEN_SECRET = "kawaii_twitter_api_token_secret";

		public Service()
		{
		}

		/// <summary>
		/// Если вернул true, надо ждать минут 15 до след.вызова любого метода
		/// </summary>
		public bool LimitReached
		{
			get
			{
				//TODO@: реализовать проверку лимитов 

				//var limits = API.Response.RateLimitStatus;
				//if (limits.RemainingHits <= 2)
				//{
				//	//превышен лимит, нужно подождать минут 15
				//	return true;
				//}

				return false;
			}
		}

		/// <summary>
		/// Створити (запостити) твіт
		/// </summary>
		/// <param name="tweetText">Текст твіта</param>
		/// <param name="imageFileName">Ім'я файлу (без шляху, для визначення розширення)</param>
		/// <param name="imageFileBody">Тіло файлу</param>
		/// <returns></returns>
		public async Task TweetWithMedia(string tweetText, string imageFileName, byte[] imageFileBody)
		{
			//Azure-функція має налаштування, які доступні як Environment-змінні

			string twitterAPIKey = Environment.GetEnvironmentVariable(ENV_API_KEY);
			if (string.IsNullOrEmpty(twitterAPIKey))
				throw new KeyNotFoundException(ENV_API_KEY);

			string twitterAPISecret = Environment.GetEnvironmentVariable(ENV_API_SECRET);
			if (string.IsNullOrEmpty(twitterAPISecret))
				throw new KeyNotFoundException(ENV_API_SECRET);

			string twitterAccessToken = Environment.GetEnvironmentVariable(ENV_API_TOKEN);
			if (string.IsNullOrEmpty(twitterAccessToken))
				throw new KeyNotFoundException(ENV_API_TOKEN);

			string twitterAccessTokenSecret = Environment.GetEnvironmentVariable(ENV_API_TOKEN_SECRET);
			if (string.IsNullOrEmpty(twitterAccessTokenSecret))
				throw new KeyNotFoundException(ENV_API_TOKEN_SECRET);

			IAuthorizer auth = new SingleUserAuthorizer
			{
				CredentialStore = new SingleUserInMemoryCredentialStore
				{
					ConsumerKey = twitterAPIKey,
					ConsumerSecret = twitterAPISecret,
					AccessToken = twitterAccessToken,
					AccessTokenSecret = twitterAccessTokenSecret
				}
			};

			await auth.AuthorizeAsync();

			var context = new TwitterContext(auth);

			string mediaType = null;

			string mediaCategory = _MEDIA_CATEGORY_IMAGE;

			string ext = Path.GetExtension(imageFileName);
			if (!string.IsNullOrEmpty(ext))
			{
				ext = ext.ToLower().Replace(".", string.Empty);
				if (ext == _GIF_EXT)
				{
					mediaCategory = _MEDIA_CATEGORY_GIF;
				}

				mediaType = string.Format("image/{0}", ext);
			}

			if (string.IsNullOrEmpty(mediaType))
			{
				string msg = $"Неможливо визначити тип зображення для файлу {imageFileName}";
				throw new ApplicationException(msg);
			}

			var uploadTaskResult = await context.UploadMediaAsync(imageFileBody, mediaType, mediaCategory);
			var mediaID = uploadTaskResult.MediaID;

			ulong[] mediaIDs = new ulong[] { mediaID };

			//TODO@: можно даже задать alt-текст? неплохо изучить позже
			//mediaIds.ForEach(async id => await twitterCtx.CreateMediaMetadataAsync(id, $"Test Alt Text for Media ID: {id}"));

			var result = await context.TweetAsync(tweetText, mediaIDs);
		}
	}
}
