using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core.SelectLogic.ExternalImages
{
	public class NotTwittedAnimated : IAnimatedSelector
	{
		IMongoCollection<AnimatedImage> _AnimatedImages;
		int _TopQueryCount;
		IRandomSelector _RandomSelector;

		public NotTwittedAnimated(IMongoCollection<AnimatedImage> animatedImages, int topQueryCount, IRandomSelector randomSelector)
		{
			if (topQueryCount <= 0)
				throw new ArgumentException("topQueryCount повинно бути більше ніж 0", nameof(topQueryCount));

			_AnimatedImages = animatedImages ?? throw new ArgumentNullException(nameof(animatedImages));
			_TopQueryCount = topQueryCount;
			_RandomSelector = randomSelector ?? throw new ArgumentNullException(nameof(randomSelector));
		}


		public async Task<AnimatedImage> GetAnimatedImageForTwitting()
		{
			var gifsNotTwitted = (from gif in _AnimatedImages.AsQueryable() where (gif.TweetDate == null) select gif).Take(_TopQueryCount);
			if (gifsNotTwitted != null)
			{
				var list = await gifsNotTwitted.ToListAsync();

				if (list.Count > 0)
				{
					int ind = _RandomSelector.GetRandomIndex(list.Count);
					AnimatedImage img = list[ind];
					return img;
				}
			}

			return null;
		}
	}
}
