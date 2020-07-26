using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.console
{
	enum ArgumentTypes
	{
		Unspecified = 0,

		/// <summary>
		/// Папка, де знаходяться папки для кожного поста з gif-зображеннями
		/// </summary>
		Folder = 1,

		/// <summary>
		/// Строка підключення до Azure Blob, де зберігаються gif-зображення
		/// </summary>
		AnimatedBlobsConnectionString = 2,

		/// <summary>
		/// Виконати до-завантаження бази
		/// </summary>
		UpdateDatabase=4,

		/// <summary>
		/// Строка підключення до Azure бази сторінок сайту (постів)
		/// </summary>
		SitePagesConnectionString=8

	}
}
