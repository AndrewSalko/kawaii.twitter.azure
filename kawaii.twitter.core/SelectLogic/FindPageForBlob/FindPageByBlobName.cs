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
		IMongoQueryable<SitePage> _Pages;
		IBlobNameToURLPart _BlobNameToURLPart;

		//IMongoCollection<SitePage> pages - old way

		public FindPageByBlobName(IMongoQueryable<SitePage> pages, IBlobNameToURLPart blobNameToURLPart)
		{
			_Pages = pages ?? throw new ArgumentNullException(nameof(pages));
			_BlobNameToURLPart = blobNameToURLPart ?? throw new ArgumentNullException(nameof(blobNameToURLPart));
		}

		public async Task<SitePage> Find(string blobName)
		{
			string slug = _BlobNameToURLPart.GetURLPart(blobName);

			SitePage result = await (from page in _Pages where (page.URL.EndsWith(slug)) select page).FirstOrDefaultAsync();
			return result;
		}

	}
}
