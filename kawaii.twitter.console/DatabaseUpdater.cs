using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using kawaii.twitter.blob;
using kawaii.twitter.db;
using MongoDB.Driver;
using kawaii.twitter.Logs;

namespace kawaii.twitter.console
{
	class DatabaseUpdater
	{
		string _AzureBlobConnectionString;
		string _AzureSiteDBConnectionString;
		ILogger _Log;

		public DatabaseUpdater(string azureBlobConnectionString, string azureSiteDBConnectionString, ILogger log)
		{
			if (string.IsNullOrEmpty(azureBlobConnectionString))
				throw new ArgumentNullException(nameof(azureBlobConnectionString));

			if (string.IsNullOrEmpty(azureSiteDBConnectionString))
				throw new ArgumentNullException(nameof(azureSiteDBConnectionString));

			_AzureBlobConnectionString = azureBlobConnectionString;
			_AzureSiteDBConnectionString = azureSiteDBConnectionString;
			_Log = log ?? throw new ArgumentNullException(nameof(log));
		}


		/// <summary>
		/// Вичитує блоби, аналізує чи є вони у базі, якщо ні - додає
		/// </summary>
		public async Task UpdateAnimatedBlobDataBase()
		{
			_Log.Log("UpdateAnimatedBlobDataBase started...");

			var animatedImagesBlobContainer = new AnimatedImagesBlobContainer(_AzureBlobConnectionString);
			string[] blobNames = animatedImagesBlobContainer.GetBlobNames();

			if (blobNames == null || blobNames.Length == 0)
			{
				_Log.Log("Blobs not found");
				return;
			}

			_Log.Log("Found {0} animated blobs", blobNames.Length);

			AnimatedImageCollection animatedImageCollection = new AnimatedImageCollection();
			animatedImageCollection.Initialize(_AzureSiteDBConnectionString, true, null, null);

			var imagesCollection = animatedImageCollection.AnimatedImages;

			int added = 0;

			foreach (var blobName in blobNames)
			{
				FilterDefinitionBuilder<AnimatedImage> def = new FilterDefinitionBuilder<AnimatedImage>();
				var filter = def.Eq(x => x.BlobName, blobName);

				bool recordFound = false;

				using (var cursor = await imagesCollection.FindAsync(filter))
				{
					while (cursor.MoveNext())
					{
						foreach (var img in cursor.Current)
						{
							//что-то нашлось, пропуск...
							recordFound = true;
							break;
						}
					}
				}

				if (!recordFound)
				{
					//создаем новую запись в базе
					AnimatedImage animatedImage = new AnimatedImage
					{
						BlobName = blobName
					};

					await imagesCollection.InsertOneAsync(animatedImage);

					added++;

					_Log.Log("Added: {0}", blobName);
				}
			}

			_Log.Log("UpdateAnimatedBlobDataBase finished, added: {0}", added);
		}
	}
}
