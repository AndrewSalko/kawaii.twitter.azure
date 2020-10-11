using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.blob;

namespace kawaii.twitter.core.tests.TweetCreator.Stubs
{
	class BlobDownloadStub : IBlobDownload
	{
		public Task<byte[]> GetBlobBody(string blobName)
		{
			throw new NotImplementedException();
		}
	}
}
