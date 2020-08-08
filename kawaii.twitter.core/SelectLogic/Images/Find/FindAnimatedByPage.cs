using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace kawaii.twitter.core.SelectLogic.Images.Find
{
	/// <summary>
	/// Получает связанный с заданным url набор аним.изображений
	/// </summary>
	public class FindAnimatedByPage
	{
		IMongoCollection<AnimatedImage> _AnimatedImages;
		IRandomSelector _RandomSelector;

		public FindAnimatedByPage(IMongoCollection<AnimatedImage> animatedImages, int topQueryCount, IRandomSelector randomSelector)
		{
			if (topQueryCount <= 0)
				throw new ArgumentException("topQueryCount повинно бути більше ніж 0", nameof(topQueryCount));

			_AnimatedImages = animatedImages ?? throw new ArgumentNullException(nameof(animatedImages));
			_RandomSelector = randomSelector ?? throw new ArgumentNullException(nameof(randomSelector));
		}


		public async Task<AnimatedImage[]> GetAnimatedImagesForPage(string pageURL)
		{
			if (string.IsNullOrEmpty(pageURL))
				throw new ArgumentNullException(nameof(pageURL));

			//имя блоб-записей в таблице - в первой части содержит "slug:" поста

			Uri uri = new Uri(pageURL);
			var segments = uri.Segments;
			if (segments == null || segments.Length == 0)
				throw new ApplicationException("segments empty");

			//взять послед.часть
			string slug = uri.Segments[uri.Segments.Length - 1];

			string blobPrefix = string.Format("{0}:", slug);

			var gifsForPage = await (from gif in _AnimatedImages.AsQueryable() where (gif.BlobName.StartsWith(blobPrefix)) select gif).ToListAsync();
			if (gifsForPage != null)
			{
				return gifsForPage.ToArray();
			}

			return null;
		}


	}
}
