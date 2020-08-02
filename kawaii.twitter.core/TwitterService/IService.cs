using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.TwitterService
{
	public interface IService
	{
		/// <summary>
		/// Створити (запостити) твіт
		/// </summary>
		/// <param name="tweetText">Текст твіта</param>
		/// <param name="imageFileName">Ім'я файлу (без шляху, для визначення розширення)</param>
		/// <param name="imageFileBody">Тіло файлу</param>
		/// <returns></returns>
		Task TweetWithMedia(string tweetText, string imageFileName, byte[] imageFileBody);

	}
}
