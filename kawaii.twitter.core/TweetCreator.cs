using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;  //https://mongodb-documentation.readthedocs.io/en/latest/ecosystem/tutorial/use-linq-queries-with-csharp-driver.html#gsc.tab=0

namespace kawaii.twitter.core
{
	/// <summary>
	/// Генерація твітів (з бази та анімованих зображень)
	/// </summary>
	public class TweetCreator
	{

		public TweetCreator()
		{
		}

		//public async Task Execute()
		//{

		//	string imageFileName = "";		//TODO@: имя файла (может быть gif, jpg...)
		//	byte[] imageBody = null;		//TODO@: тело файла загрузить в память целиком (он все же, не более пару МБ, переживет)


		//	string url = "https://........";	//TODO@: урл поста
		//	string postTitle = "Some title";    //TODO@: тайтл поста

		//	var textCreator = new Text.TwitterTextCreator();
		//	string tweetText = textCreator.CreateTwitterText(url, postTitle);

		//	var service = new TwitterService.Service();

		//	await service.TweetWithMedia(tweetText, imageFileName, imageBody);
		//}


	}
}
