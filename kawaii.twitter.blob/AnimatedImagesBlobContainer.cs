using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Azure.Storage;

namespace kawaii.twitter.blob
{
	/// <summary>
	/// Контейнер для зберігання анімованих зображень (.gif) для різних постів
	/// Правило іменування: "Папка slug поста:імя файлу.gif"
	/// </summary>
	public class AnimatedImagesBlobContainer
	{
		public const string CONTAINER_NAME = "animated-images";

		BlobContainerClient _ContainerClient;

		public AnimatedImagesBlobContainer(string connectionStringAzureBlob)
		{
			BlobServiceClient blobServiceClient = new BlobServiceClient(connectionStringAzureBlob);
			_ContainerClient = blobServiceClient.GetBlobContainerClient(CONTAINER_NAME);
		}

		public string[] GetBlobNames()
		{
			var blobs = _ContainerClient.GetBlobs();

			List<string> names = new List<string>();

			if (blobs != null)
			{
				foreach (var blob in blobs)
				{
					if (blob.Deleted)
						continue;

					names.Add(blob.Name);
				}
			}

			return names.ToArray();
		}

		string _ComposeBlobName(string folderName, string fileName)
		{
			//Про найменування: https://docs.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata

			return string.Format("{0}:{1}", folderName, fileName);
		}

		public string UploadImage(string fileFullName, out bool upladedNewImage)
		{
			upladedNewImage = false;
			string fileName = Path.GetFileName(fileFullName);

			//нам потрібна остання папка у переліку
			DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(fileFullName));
			string lastDir = dir.Name;

			string blobName = _ComposeBlobName(lastDir, fileName);

			//знайдемо блоб - якщо знайдеться, нічого не робимо

			var blobClient = _ContainerClient.GetBlobClient(blobName);
			if (blobClient.Exists())
			{
				return blobName;
			}

			//наші файли невеликого розміру, завантажуємо у пам'ять одразу
			using (var memStream = new MemoryStream(File.ReadAllBytes(fileFullName)))
			{
				blobClient.Upload(memStream);
				upladedNewImage = true;
			}

			return blobName;
		}

	}
}
