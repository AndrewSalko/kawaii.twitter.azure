using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.core.SelectLogic.URL;
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
		IFolderFromURL _FolderFromURL;
		BlobName.IFormatter _Formatter;

		public FindAnimatedByPage(IMongoCollection<AnimatedImage> animatedImages, int topQueryCount, IFolderFromURL folderFromURL, BlobName.IFormatter formatter)
		{
			_AnimatedImages = animatedImages ?? throw new ArgumentNullException(nameof(animatedImages));

			if (topQueryCount <= 0)
				throw new ArgumentException("topQueryCount повинно бути більше ніж 0", nameof(topQueryCount));

			_FolderFromURL = folderFromURL ?? throw new ArgumentNullException(nameof(folderFromURL));
			_Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
		}

		public async Task<AnimatedImage[]> GetAnimatedImagesForPage(string pageURL)
		{
			//получаем папку поста из его url
			string postFolder = _FolderFromURL.GetFolderFromURL(pageURL);

			//получаем префикс блоба, и по нему будем искать уже выборку
			string blobPrefix = _Formatter.GetBlobNamePrefix(postFolder);

			var gifsForPage = await (from gif in _AnimatedImages.AsQueryable() where (gif.BlobName.StartsWith(blobPrefix)) select gif).ToListAsync();
			if (gifsForPage != null)
			{
				return gifsForPage.ToArray();
			}

			return null;
		}


	}
}
