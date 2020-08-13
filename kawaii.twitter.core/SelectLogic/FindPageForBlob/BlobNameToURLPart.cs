using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SelectLogic.FindPageForBlob
{
	public class BlobNameToURLPart: IBlobNameToURLPart
	{
		public BlobNameToURLPart()
		{
		}

		public string GetURLPart(string blobName)
		{
			if (string.IsNullOrEmpty(blobName))
				throw new ArgumentNullException(nameof(blobName));

			//формат blobName виглядає як "slug поста:ім'я файлу"
			//Ми зможемо знайти такий пост через url

			//Наприклад для урл: https://kawaii-mobile.com/2017/01/shuumatsu-no-izetta/
			//його блоб має вигляд: shuumatsu-no-izetta:img1.gif

			string[] parts = blobName.Split(':');
			if (parts.Length != 2)
				throw new ArgumentException("blobName має невірний формат", nameof(blobName));

			string slug = string.Format("/{0}/", parts[0]);
			return slug;
		}

	}
}
