using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db;

namespace kawaii.twitter.core.SelectLogic.AnimatedExcludeLast
{
	public interface IAnimatedSelectorWithExcludeLast
	{
		/// <summary>
		/// Выбирает из массива случайный элемент, кроме самого "свежего" по твит-дате (поле TweetDate)
		/// </summary>
		/// <param name="images"></param>
		/// <returns></returns>
		AnimatedImage SelectImage(AnimatedImage[] images);
	}
}
