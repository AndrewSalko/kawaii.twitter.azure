using System;
using System.IO;

namespace kawaii.twitter.blob.uploader
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("kawaii-mobile.com - upload animated gif to Azure blob");

			if (args == null || args.Length == 0)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("-folder <path to folder with images>. Each subfolder in this folder scanned for images");
				return;
			}

			try
			{
				ArgumentsParser argumentsParser = new ArgumentsParser(args);

				string sourceFolder = argumentsParser.Folder;
				string azureBlobConnectionString = argumentsParser.BlobGifsConnectionString;

				var animatedImagesBlobContainer = new AnimatedImagesBlobContainer(azureBlobConnectionString);

				var files = Directory.EnumerateFiles(sourceFolder, "*.gif", SearchOption.AllDirectories);

				int tick = Environment.TickCount;

				int count = 0;
				int counterForLog = 0;
				foreach (var fileName in files)
				{
					int t2 = Environment.TickCount;
					string blobName = animatedImagesBlobContainer.UploadImage(fileName, out bool upladedNewImage);
					t2 = Environment.TickCount - t2;

					if (upladedNewImage)
					{
						Console.WriteLine("{0} - uploaded to blob: {1} ({2} ms)", fileName, blobName, t2);
					}

					count++;
					counterForLog++;
					if (counterForLog >= 25)
					{
						Console.WriteLine("Processed {0} files...", count);
						counterForLog = 0;
					}

				}//foreach

				Console.WriteLine("Processed {0} files", count);

				tick = Environment.TickCount - tick;

				Console.WriteLine("Done for {0} ms", tick);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return;
			}

		}//Main
	}
}
