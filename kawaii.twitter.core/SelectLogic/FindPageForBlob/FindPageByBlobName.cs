using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core.SelectLogic.FindPageForBlob
{
	public class FindPageByBlobName: IFindPageByBlobName
	{
		IQueryable<SitePage> _Pages;
		IBlobNameToURLPart _BlobNameToURLPart;

		public FindPageByBlobName(IQueryable<SitePage> pages, IBlobNameToURLPart blobNameToURLPart)
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
