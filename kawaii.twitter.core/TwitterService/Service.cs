using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using LinqToTwitter.OAuth;

namespace kawaii.twitter.core.TwitterService
{
	/// <summary>
	/// Створення твітів, працює з твіттер-API
	/// </summary>
	public class Service: IService
	{
		public const string GIF_EXT = ".gif";

		public const string ENV_API_KEY = "kawaii_twitter_api_key";
		public const string ENV_API_SECRET = "kawaii_twitter_api_secret";
		public const string ENV_API_TOKEN = "kawaii_twitter_api_token";
		public const string ENV_API_TOKEN_SECRET = "kawaii_twitter_api_token_secret";

		public Service()
		{
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

			var creds = new LinqToTwitter.OAuth.InMemoryCredentialStore
			{
				ConsumerKey = twitterAPIKey,
				ConsumerSecret = twitterAPISecret,
				OAuthToken = twitterAccessToken,
				OAuthTokenSecret = twitterAccessTokenSecret,
				ScreenName = "KawaiiMobile",
				UserID = 332240748   //ScreenName, UserID можливо не знадобиться
			};

			var auth = new PinAuthorizer();
			auth.CredentialStore= creds;

			await auth.AuthorizeAsync();

			var twitterCtx = new TwitterContext(auth);

			string mediaCategoryImage = "tweet_image";
			string mediaType = "image/jpg";
			string ext = Path.GetExtension(imageFileName).ToLower();
			if (ext == GIF_EXT)
			{
				mediaType = "image/gif";
			}

			var mediaImg = await twitterCtx.UploadMediaAsync(imageFileBody, mediaType, mediaCategoryImage);
			if (mediaImg == null)
				throw new ApplicationException("mediaImg == null");

			string[] medias = new string[] { mediaImg.MediaID.ToString() };

			var tw = await twitterCtx.TweetMediaAsync(tweetText, medias);
			if (tw == null)
				throw new ApplicationException("tw == null");
		}
	}
}
