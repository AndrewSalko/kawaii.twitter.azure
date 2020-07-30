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
	public class Service
	{
		const string _MEDIA_CATEGORY_IMAGE = "tweet_image";
		const string _MEDIA_CATEGORY_GIF = "tweet_gif";

		const string _GIF_EXT = "gif";

		const string _ENV_PREFIX = "env:";

		const string _ENV_API_KEY = "env:kawaii_twitter_api_key";
		const string _ENV_API_SECRET = "env:kawaii_twitter_api_secret";
		const string _ENV_API_TOKEN = "env:kawaii_twitter_api_token";
		const string _ENV_API_TOKEN_SECRET = "env:kawaii_twitter_api_token_secret";

		TwitterContext _Context;

		public Service(string twitterAPIKey, string twitterAPISecret, string twitterAccessToken, string twitterAccessTokenSecret)
		{
			//string twitterAPIKey = _GetRealConfigValue(ConfigurationManager.AppSettings["TwitterAPIKey"]);
			//string twitterAPISecret = _GetRealConfigValue(ConfigurationManager.AppSettings["TwitterAPISecret"]);

			//string twitterAccessToken = _GetRealConfigValue(ConfigurationManager.AppSettings["TwitterAccessToken"]);
			//string twitterAccessTokenSecret = _GetRealConfigValue(ConfigurationManager.AppSettings["TwitterAccessTokenSecret"]);

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

			auth.AuthorizeAsync().Wait();

			_Context = new TwitterContext(auth);
		}

		string _GetRealConfigValue(string valueToParse)
		{
			if (string.IsNullOrEmpty(valueToParse))
				throw new ArgumentException("valueToParse IsNullOrEmpty", nameof(valueToParse));

			//если строка значения начинается с текста env: то значит надо брать переменную среды
			if (valueToParse.StartsWith(_ENV_PREFIX))
			{
				string envName = valueToParse.Replace(_ENV_PREFIX, string.Empty);
				return	Environment.GetEnvironmentVariable(envName);
			}

			return valueToParse;
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

		public async Task TweetWithMedia(string tweetText, string imageFileName)
		{
			byte[] fileBody = File.ReadAllBytes(imageFileName);

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

			var uploadTaskResult = await _Context.UploadMediaAsync(fileBody, mediaType, mediaCategory);
			var mediaID = uploadTaskResult.MediaID;

			ulong[] mediaIDs = new ulong[] { mediaID };

			//TODO@: можно даже задать alt-текст? неплохо изучить позже
			//mediaIds.ForEach(async id => await twitterCtx.CreateMediaMetadataAsync(id, $"Test Alt Text for Media ID: {id}"));

			var result = await _Context.TweetAsync(tweetText, mediaIDs);
		}
	}
}
