using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db;

namespace kawaii.twitter.core.SelectLogic.Images.ExcludeUsed
{
	public class AnimatedSelectorWithExcludeLast: IAnimatedSelectorWithExcludeLast
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

			//особенность рандома - последний элемент не входит. Если у нас в массиве 5 элементов, то выборка будет для первых 4-х, последний не берем
			int max = images.Length - 1;

			int ind = rnd.Next(max);

			return sorted[ind];
		}

	}
}
