using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.PageOrExternalImage
{
	/// <summary>
	/// Класс логики которая случайным образом "решает" использовать ли изображения из поста, либо к этому посту
	/// брать внешние (анимированные) изображения для твита
	/// </summary>
	public class PageOrExternalImageSelector: IPageOrExternalImageSelector
	{
		const int _MAX_RANDOM = 10;

		Random _Rnd;

		public PageOrExternalImageSelector()
		{
			_Rnd = new Random(Environment.TickCount);
		}

		public bool UseExternalAnimatedImage
		{
			get
			{
				//будет от 0 до _MAX_RANDOM-1
				int rnd = _Rnd.Next(_MAX_RANDOM);

				if (rnd < 3)
				{
					//Считаем что где-то в 30% стоит использовать аним.гиф-изображение, а в остальных - изображения поста.
					//Обычно изображений в посте достаточно немало для случайного выбора (их может быть и 10-20)
					return true;
				}

				return false;
			}
		}

	}
}
