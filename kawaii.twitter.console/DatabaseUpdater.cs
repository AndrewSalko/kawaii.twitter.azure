using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using kawaii.twitter.blob;
using kawaii.twitter.db;
using MongoDB.Driver;

namespace kawaii.twitter.console
{
	class DatabaseUpdater
	{
		string _AzureBlobConnectionString;
		string _AzureSiteDBConnectionString;

		public DatabaseUpdater(string azureBlobConnectionString, string azureSiteDBConnectionString)
		{
			if (string.IsNullOrEmpty(azureBlobConnectionString))
				throw new ArgumentNullException(nameof(azureBlobConnectionString));

			if (string.IsNullOrEmpty(azureSiteDBConnectionString))
				throw new ArgumentNullException(nameof(azureSiteDBConnectionString));

			_AzureBlobConnectionString = azureBlobConnectionString;
			_AzureSiteDBConnectionString = azureSiteDBConnectionString;
		}


		/// <summary>
		/// Вичитує блоби, аналізує чи є вони у базі, якщо ні - додає
		/// </summary>
		public async Task UpdateAnimatedBlobDataBase()
		{
			Console.WriteLine("UpdateAnimatedBlobDataBase started...");

			var animatedImagesBlobContainer = new AnimatedImagesBlobContainer(_AzureBlobConnectionString);
			string[] blobNames = animatedImagesBlobContainer.GetBlobNames();

			if (blobNames == null || blobNames.Length == 0)
			{
				Console.WriteLine("Blobs not found");
				return;
			}

			Console.WriteLine("Found {0} animated blobs", blobNames.Length);

			Database db = new db.Database(_AzureSiteDBConnectionString, true);
			var imagesCollection = db.AnimatedImages;

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
				}
			}


			Console.WriteLine("UpdateAnimatedBlobDataBase finished");
		}
	}
}
