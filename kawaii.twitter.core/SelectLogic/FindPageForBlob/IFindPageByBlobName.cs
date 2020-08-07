using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.db;

namespace kawaii.twitter.core.SelectLogic.FindPageForBlob
{
	public interface IFindPageByBlobName
	{
		Task<SitePage> Find(string blobName);
	}
}
