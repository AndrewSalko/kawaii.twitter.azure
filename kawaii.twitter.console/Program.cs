using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using kawaii.twitter.blob;

namespace kawaii.twitter.console
{
	class Program
	{
		static HttpClient _HttpClient = new HttpClient();


		static async Task Main(string[] args)
		{
			Console.WriteLine("kawaii-mobile.com - console app to manual update");

			if (args == null || args.Length == 0)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("-folder <path to folder with images>. Each subfolder in this folder scanned for images");
				return;
			}

			try
			{
				int tick = Environment.TickCount;

				ArgumentsParser argumentsParser = new ArgumentsParser(args);

				string azureBlobConnectionString = argumentsParser.BlobGifsConnectionString;
				string azureDBSitePagesConnectionString = argumentsParser.SitePagesConnectionString;

				string sourceFolder = argumentsParser.Folder;


				if (!string.IsNullOrEmpty(sourceFolder))
				{
					var animatedImagesBlobContainer = new AnimatedImagesBlobContainer(azureBlobConnectionString);

					var files = Directory.EnumerateFiles(sourceFolder, "*.gif", SearchOption.AllDirectories);

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
				}

				if (!string.IsNullOrEmpty(argumentsParser.DBUpdate))
				{
					string dbUpdateMode = argumentsParser.DBUpdate;

					if (dbUpdateMode == ArgumentsParser.ARG_VALUE_ANIMATED)
					{
						Console.WriteLine("Updating database from blob objects...");
						//виконати до-вантаження (або створити) базу анімованих зображень з наявних блоб-об'єктів

						DatabaseUpdater upd = new DatabaseUpdater(azureBlobConnectionString, azureDBSitePagesConnectionString);
						await upd.UpdateAnimatedBlobDataBase();
					}
					else
					{
						if (dbUpdateMode == ArgumentsParser.ARG_VALUE_POSTS_ALL)
						{
							//оновити за усіма постами
							await _ReloadFromAllPosts(azureDBSitePagesConnectionString);
						}
						else
						{
							if (dbUpdateMode == ArgumentsParser.ARG_VALUE_POSTS_RECENT)
							{
								//TODO@: оновити недавні пости
							}
						}
					}

				}

				tick = Environment.TickCount - tick;

				Console.WriteLine("Done for {0} ms", tick);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return;
			}

		}//Main


		static async Task _ReloadFromAllPosts(string azureDBConnectionString)
		{
			string postSiteMapURL = "https://kawaii-mobile.com/post.xml";

			//string mongoConnectionString = "mongodb://localhost:27017/?readPreference=primary&appname=kawaiitwitter&ssl=false";
			//kawaii.twitter.db.Database database = new kawaii.twitter.db.Database(azureDBConnectionString, true);

			kawaii.twitter.db.SitePageCollection sitePageCollection = new db.SitePageCollection();
			sitePageCollection.Initialize(azureDBConnectionString, true, null, null);

			var sitePages = sitePageCollection.SitePages;

			kawaii.twitter.core.DatabaseFromSitemapUpdater databaseFromSitemapUpdater = new kawaii.twitter.core.DatabaseFromSitemapUpdater(sitePages);

			kawaii.twitter.core.SiteMap.XMLSiteMapLoader loader = new kawaii.twitter.core.SiteMap.XMLSiteMapLoader(_HttpClient);

			await databaseFromSitemapUpdater.UpdateFromSitemap(postSiteMapURL, loader);
		}

	}
}
