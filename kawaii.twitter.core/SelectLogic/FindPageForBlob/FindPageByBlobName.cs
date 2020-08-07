using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core.SelectLogic.FindPageForBlob
{
	class FindPageByBlobName: IFindPageByBlobName
	{
		IMongoCollection<SitePage> _Pages;

		public FindPageByBlobName(IMongoCollection<SitePage> pages)
		{
			_Pages = pages ?? throw new ArgumentNullException(nameof(pages));
		}

		public async Task<SitePage> Find(string blobName)
		{
			//формат blobName виглядає як "slug поста:ім'я файлу"
			//Ми зможемо знайти такий пост через url

			string[] parts = blobName.Split(':');
			if (parts.Length != 2)
				throw new ArgumentException("blobName має невірний формат", nameof(blobName));

			string slug = string.Format("/{0}", parts[0]);

			SitePage result = await (from page in _Pages.AsQueryable() where (page.URL.EndsWith(slug)) select page).FirstOrDefaultAsync();
			return result;
		}

	}
}
