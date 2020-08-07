using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.db;

namespace kawaii.twitter.core.SelectLogic
{
	public class TwittData
	{
		/// <summary>
		/// Сторінка про яку будемо створювати твіт
		/// </summary>
		public SitePage Page
		{
			get;
			set;
		}

		/// <summary>
		/// Якщо null, буде використано зображення з посту, інакше - це анімоване зображення
		/// </summary>
		public AnimatedImage Image
		{
			get;
			set;
		}

	}
}
