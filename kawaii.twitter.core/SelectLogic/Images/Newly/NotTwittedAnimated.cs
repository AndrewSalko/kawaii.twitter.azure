using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.core.SelectLogic.URL;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core.SelectLogic.Images.Newly
{
	public class NotTwittedAnimated : IFindAnimatedByPage
	{
		IMongoCollection<AnimatedImage> _AnimatedImages;
		int _TopQueryCount;
		IRandomSelector _RandomSelector;

		IFolderFromURL _FolderFromURL;
		BlobName.IFormatter _Formatter;


		public NotTwittedAnimated(IMongoCollection<AnimatedImage> animatedImages, IRandomSelector randomSelector, int topQueryCount, IFolderFromURL folderFromURL, BlobName.IFormatter formatter)
		{
			_AnimatedImages = animatedImages ?? throw new ArgumentNullException(nameof(animatedImages));
			_RandomSelector = randomSelector ?? throw new ArgumentNullException(nameof(randomSelector));

			if (topQueryCount <= 0)
				throw new ArgumentException("topQueryCount повинно бути більше ніж 0", nameof(topQueryCount));

			_TopQueryCount = topQueryCount;

			_FolderFromURL = folderFromURL ?? throw new ArgumentNullException(nameof(folderFromURL));
			_Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
		}


		public async Task<AnimatedImage[]> GetAnimatedImagesForPage(string pageURL)
		{
			//получаем папку поста из его url
			string postFolder = _FolderFromURL.GetFolderFromURL(pageURL);

			//получаем префикс блоба, и по нему будем искать уже выборку
			string blobPrefix = _Formatter.GetBlobNamePrefix(postFolder);

			var gifsNotTwitted = (from gif in _AnimatedImages.AsQueryable() where (gif.BlobName.StartsWith(blobPrefix) && gif.TweetDate == null) select gif).Take(_TopQueryCount);
			
			var list = await gifsNotTwitted.ToListAsync();

			if (list.Count > 0)
			{
				int ind = _RandomSelector.GetRandomIndex(list.Count);
				AnimatedImage img = list[ind];
				AnimatedImage[] result = new AnimatedImage[] { img };
				return result;
			}

			return null;
		}
	}
}
