using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.HtmlParsers
{
	/// <summary>
	/// Для окремої сторінки (поста) де кілька зображень обирає одне випадковим чином,
	/// та повертає її twitter-URL (посилання на повнорозмірне зображення)
	/// </summary>
	public class TwitterImageURL
	{
		/// <summary>
		/// (!) Описание сделать того что это будет именно случайное изображение
		/// </summary>
		/// <param name="client">HttpClient будет жить как статик внутри ф-ции один на всех</param>
		/// <param name="pageURL"></param>
		/// <returns></returns>
		public async Task<string> GetTwitterImageFileURL(HttpClient client, string pageURL)
		{
			TwitterImageExtractor extractor = new TwitterImageExtractor();

			string htmlBody = await client.GetStringAsync(pageURL);

			//Здесь мы проведем анализ - соберем все ссылки на аттачи-изображения на этой странице
			AttachPagesLoader attachPagesLoader = new AttachPagesLoader(pageURL, htmlBody);

			string[] imagePagesURLs = attachPagesLoader.GetAttachImagePagesURLs();

			if (imagePagesURLs != null && imagePagesURLs.Length > 0)
			{
				//извлечем случайно одну из них, и у нее пробуем взять твиттер-изображение
				Random r = new Random();
				int indexOfURL = r.Next(0, imagePagesURLs.Length);
				string urlToGetImage = imagePagesURLs[indexOfURL];

				//теперь извлечем оттуда тело html и его изображение
				string htmlBodySubPage = await client.GetStringAsync(urlToGetImage);

				string subPageImageURL = extractor.ExtractImageURL(htmlBodySubPage);
				if (!string.IsNullOrEmpty(subPageImageURL))
				{
					return subPageImageURL;
				}
			}

			//если что-то пошло не так - берем уже по-старому
			string result = extractor.ExtractImageURL(htmlBody);

			return result;
		}


	}
}
