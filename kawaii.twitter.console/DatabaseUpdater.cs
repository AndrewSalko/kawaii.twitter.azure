using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.blob;

namespace kawaii.twitter.console
{
	class DatabaseUpdater
	{
		string _AzureBlobConnectionString;

		public DatabaseUpdater(string azureBlobConnectionString)
		{
			if (string.IsNullOrEmpty(azureBlobConnectionString))
				throw new ArgumentNullException(nameof(azureBlobConnectionString));

			_AzureBlobConnectionString = azureBlobConnectionString;
		}


		public void UpdateDataBase()
		{
			Console.WriteLine("UpdateDataBase started...");

			var animatedImagesBlobContainer = new AnimatedImagesBlobContainer(_AzureBlobConnectionString);
			string[] blobNames = animatedImagesBlobContainer.GetBlobNames();

			if (blobNames == null || blobNames.Length == 0)
			{
				Console.WriteLine("Blobs not found");
				return;
			}

			Console.WriteLine("Found {0} animated blobs", blobNames.Length);

			
			


			Console.WriteLine("UpdateDataBase finished");
		}
	}
}
