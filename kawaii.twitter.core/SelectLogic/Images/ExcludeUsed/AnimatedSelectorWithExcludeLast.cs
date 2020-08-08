using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db;

namespace kawaii.twitter.core.SelectLogic.Images.ExcludeUsed
{
	class AnimatedSelectorWithExcludeLast: IAnimatedSelectorWithExcludeLast
	{
		public AnimatedSelectorWithExcludeLast()
		{
		}

		public AnimatedImage SelectImage(AnimatedImage[] images)
		{
			if (images == null || images.Length == 0)
				throw new ArgumentNullException(nameof(images));

			if (images.Length == 1)
				return images[0];   //нечего выбирать она единственная

			//надо сортировать их так, чтобы та которую твитили последний раз оказалась в конце списка
			var sorted = images.OrderBy(x => x.TweetDate).ToArray();

			Random rnd = new Random(Environment.TickCount);

			//особенность рандома - последний элемент не входит
			int ind = rnd.Next(images.Length - 1);

			return images[ind];
		}

	}
}
