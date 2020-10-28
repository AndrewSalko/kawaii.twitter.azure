using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Logic.QueryParameters;

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

			var userClient = new TwitterClient(twitterAPIKey, twitterAPISecret, twitterAccessToken, twitterAccessTokenSecret);

			var uploadTweetImageParameters = new Tweetinvi.Parameters.UploadTweetImageParameters(imageFileBody);

			string ext = Path.GetExtension(imageFileName).ToLower();
			if (ext == GIF_EXT)
			{
				uploadTweetImageParameters.MediaCategory = Tweetinvi.Models.MediaCategory.Gif;
			}

			var uploadedImage = await userClient.Upload.UploadTweetImageAsync(uploadTweetImageParameters);

			//TODO@: рассмотреть вопрос Alt-текста для изображения
			//await userClient.Upload.AddMediaMetadataAsync(new MediaMetadata(uploadedImage)
			//{
			//	AltText = "Chaos;Head Rimi Sakihata Nanami Nishijo"
			//});

			if (uploadedImage.Id == null)
				throw new ApplicationException("uploadedImage.Id == null");

			long mediaID = uploadedImage.Id.Value;

			var publishTweetParameters = new Tweetinvi.Parameters.PublishTweetParameters(tweetText);
			publishTweetParameters.MediaIds.Add(mediaID);

			await userClient.Tweets.PublishTweetAsync(publishTweetParameters);
		}
	}
}
