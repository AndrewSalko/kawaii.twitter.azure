using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core
{
	/// <summary>
	/// Генерація твітів (з бази та анімованих зображень)
	/// </summary>
	public class TweetCreator
	{

		public async Task Execute()
		{

			/*
			var page = _GetPageForTwitting();
			if (page == null)
				throw new ApplicationException("_GetPageForTwitting returns null");


			//получить "скачанный" файл-картинку для твита (или null если с этим проблемы)
			string fileImgName = page.ImageFileName;
			if (string.IsNullOrWhiteSpace(fileImgName))
			{
				Log("Не вдалося отримати зображення з {0}", page);
				continue;
			}

			//делаем твит...
			string tweetText = page.CreateTwitterText();

			//твит с медиа и текстом
			try
			{
				_Service.TweetWithMedia(tweetText, fileImgName);
			}
			catch (Exception ex)
			{
				Log("Помилка для {0}: {1}", page, ex.Message);

				//спим 1 минуту, и пробуем еще раз
				System.Threading.Thread.Sleep(6000);

				continue;
			}

			page.TweetDate = dt;

			//сохранить в базу сведения о том, что мы уже твитили
			_Data.SubmitChanges();
			_DataImages.SubmitChanges();
			*/

			string imageFileName = "";		//TODO@: имя файла (может быть gif, jpg...)
			byte[] imageBody = null;		//TODO@: тело файла загрузить в память целиком (он все же, не более пару МБ, переживет)


			string url = "https://........";	//TODO@: урл поста
			string postTitle = "Some title";    //TODO@: тайтл поста

			var textCreator = new Text.TwitterTextCreator();
			string tweetText = textCreator.CreateTwitterText(url, postTitle);

			var service = new TwitterService.Service();

			await service.TweetWithMedia(tweetText, imageFileName, imageBody);


		}

		///// <summary>
		///// Получить случайную страницу для твита-сообщения. В первую очередь берем случайно
		///// из тех, кого не твитили вообще, затем - те что твитили но давно.
		///// Также берет из базы gif-изображения
		///// </summary>
		///// <returns></returns>
		//ITwittable _GetPageForTwitting()
		//{
		//	//получить список страниц которые ни разу не твиттили
		//	var pagesNotTwitted = (from page in _Data.Pages where (!page.Blocked && page.TweetDate == null) select page).ToArray();
		//	var gifsNotTwitted = (from pageGif in _DataImages.Images where (pageGif.TweetDate == null) select pageGif).ToArray();

		//	//взять случайную страницу из тех, кого еще ни разу НЕ пускали в твиттер
		//	if (pagesNotTwitted != null && pagesNotTwitted.Length > 0)
		//	{
		//		return _GetRandomPage(pagesNotTwitted, 0);
		//	}

		//	//Если попали сюда значит нет новых страниц, но может есть новые gif-файлы? (которые НИ разу не твитили)
		//	if (gifsNotTwitted != null && gifsNotTwitted.Length > 0)
		//	{
		//		return _GetRandomPage(gifsNotTwitted, 0);
		//	}

		//	//на этом этапе у нас все страницы и все гифки уже твитились. Собрать в кучу, выбрать кого твитили давно, выбрать случаное
		//	List<ITwittable> normPagesAndGifs = new List<ITwittable>();

		//	var twittedPages = (from page in _Data.Pages where (!page.Blocked && page.TweetDate != null) orderby page.TweetDate.Value select page).ToArray();
		//	if (twittedPages != null && twittedPages.Length > 0)
		//	{
		//		normPagesAndGifs.AddRange(twittedPages);
		//	}

		//	var twittedImages = (from page in _DataImages.Images where (page.TweetDate != null) orderby page.TweetDate.Value select page).ToArray();
		//	if (twittedImages != null && twittedImages.Length > 0)
		//	{
		//		normPagesAndGifs.AddRange(twittedImages);
		//	}

		//	var result = normPagesAndGifs.OrderBy(pg => pg.TweetDate).ToArray();

		//	return _GetRandomPage(result, 30);//выбор из топ-30
		//}

		//ITwittable _GetRandomPage(ITwittable[] pages, int maxRandomIndex)
		//{
		//	if (pages == null || pages.Length == 0)
		//		throw new ArgumentNullException("pages");

		//	if (maxRandomIndex <= 0)
		//	{
		//		maxRandomIndex = pages.Length;
		//	}
		//	else
		//	{
		//		if (pages.Length < maxRandomIndex)
		//			maxRandomIndex = pages.Length;
		//	}

		//	Random rnd = new Random(Environment.TickCount);
		//	int ind = rnd.Next(maxRandomIndex);

		//	ITwittable result = pages[ind];
		//	IMediaImage resultImg = result as IMediaImage;
		//	if (resultImg != null && resultImg.Page == null)
		//	{
		//		//найти связанный пост для изображения. 
		//		string urlSlug = resultImg.GetURLSlug();

		//		var foundPages = (from page in _Data.Pages where (page.URL.EndsWith(urlSlug)) select page).ToArray();
		//		if (foundPages == null || foundPages.Length == 0)
		//		{
		//			throw new ApplicationException(string.Format("Не знайдено сторінку для зображення: slug={0}", urlSlug));
		//		}
		//		else
		//		{
		//			if (foundPages.Length > 1)
		//			{
		//				throw new ApplicationException(string.Format("Знайдено більш однієї сторінки для зображення: slug={0}", urlSlug));
		//			}

		//			resultImg.Page = foundPages[0];
		//		}

		//	}

		//	return result;
		//}

	}
}
