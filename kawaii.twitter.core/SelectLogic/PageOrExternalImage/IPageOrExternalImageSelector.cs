using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.PageOrExternalImage
{
	public interface IPageOrExternalImageSelector
	{
		/// <summary>
		/// Если true значит нужно использовать внешнее изображение для поста (из таблицы Animated)
		/// </summary>
		bool UseExternalAnimatedImage
		{
			get;
		}
	}
}
