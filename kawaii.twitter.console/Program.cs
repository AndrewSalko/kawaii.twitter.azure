using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using kawaii.twitter.blob;

namespace kawaii.twitter.console
{
	class Program
	{
		const string _GIF_FILES_MASK = "*.gif";

		/// <summary>
		/// Оновлювати останні пости (стільки шт)
		/// </summary>
		const int _RECENT_POSTS_COUNT = 10;

		/// <summary>
		/// Карта сайту (постів) WordPress
		/// </summary>
		const string _SITEMAP_POSTS_URL = "https://kawaii-mobile.com/post.xml";

		static HttpClient _HttpClient = new HttpClient();


		static async Task Main(string[] args)
		{
			Console.WriteLine("kawaii-mobile.com - console app to manual update");

			if (args == null || args.Length == 0)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine();
				Console.WriteLine("Scenario 1: update posts database from sitemap");
				Console.WriteLine("Use '-dbupdate allposts' or '-dbupdate recentposts', with -dbsitepages <azure connection to pages database>");

				Console.WriteLine("Scenario 2: upload new .gif files for posts to database (from local folder)");
				Console.WriteLine("Use '-dbupdate animated' with '-folder <path to folder>' with subfolders for each post and .gif files inside.");
				Console.WriteLine("-blobgifs <azure blob connection string> is required.");
				
				return;
			}

			try
			{
				int tick = Environment.TickCount;

				ArgumentsParser argumentsParser = new ArgumentsParser(args);

				//-dbupdate необхідний аргумент, якщо його немає - це помилка
				string dbUpdateMode = argumentsParser.DBUpdate;
				if (string.IsNullOrEmpty(dbUpdateMode))
				{
					Console.WriteLine("-dbupdate is required argument!");
					return;
				}

				string azureBlobConnectionString = argumentsParser.BlobGifsConnectionString;
				string azureDBSitePagesConnectionString = argumentsParser.SitePagesConnectionString;

				if (dbUpdateMode == ArgumentsParser.ARG_VALUE_ANIMATED)
				{
					Console.WriteLine("Updating animated images for posts (search and load .gif files)...");

					string sourceFolder = argumentsParser.Folder;
					if (string.IsNullOrEmpty(sourceFolder))
					{
						Console.WriteLine(ArgumentsParser.ARG_NAME_FOLDER + " <path to folder> is required argument!");
						return;
					}

					if (string.IsNullOrEmpty(azureBlobConnectionString))
					{
						Console.WriteLine(ArgumentsParser.ARG_NAME_AZURE_BLOB_ANIMATED_CONNECTION_STRING + " is required argument!");
						return;
					}

					if (string.IsNullOrEmpty(azureDBSitePagesConnectionString))
					{
						Console.WriteLine(ArgumentsParser.ARG_NAME_AZURE_DB_SITEPAGES_CONNECTION_STRING + " is required argument!");
						return;
					}

					await _UpdateGifFromDisk(azureBlobConnectionString, azureDBSitePagesConnectionString, sourceFolder);
				}
				else
				{
					if (string.IsNullOrEmpty(azureDBSitePagesConnectionString))
					{
						Console.WriteLine(ArgumentsParser.ARG_NAME_AZURE_DB_SITEPAGES_CONNECTION_STRING + " is required argument!");
						return;
					}

					if (dbUpdateMode == ArgumentsParser.ARG_VALUE_POSTS_ALL)
					{
						//оновити за усіма постами (0 - без обмежень)
						await _UpdateDBFromSitemap(azureDBSitePagesConnectionString, 0);
					}
					else
					{
						if (dbUpdateMode == ArgumentsParser.ARG_VALUE_POSTS_RECENT)
						{
							//оновити недавні пости
							await _UpdateDBFromSitemap(azureDBSitePagesConnectionString, _RECENT_POSTS_COUNT);
						}
						else
						{
							Console.WriteLine("Unsupported option:" + dbUpdateMode);
							return;
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


		/// <summary>
		/// Оновити базу з карти сайту
		/// </summary>
		/// <param name="azureDBConnectionString">Строка підключення до бази постів</param>
		/// <param name="limitUpdateCount">якщо потрібно оновити лише "найновіші пости", наприклад 10 останніх - передати більше 0. Якщо 0 - то оновити усі</param>
		/// <returns></returns>
		static async Task _UpdateDBFromSitemap(string azureDBConnectionString, int limitUpdateCount)
		{
			kawaii.twitter.db.SitePageCollection sitePageCollection = new db.SitePageCollection();
			sitePageCollection.Initialize(azureDBConnectionString, true, null, null);

			var sitePages = sitePageCollection.SitePages;

			kawaii.twitter.core.DatabaseFromSitemapUpdater databaseFromSitemapUpdater = new kawaii.twitter.core.DatabaseFromSitemapUpdater(sitePages);

			kawaii.twitter.core.SiteMap.XMLSiteMapLoader loader = new kawaii.twitter.core.SiteMap.XMLSiteMapLoader(_HttpClient)
			{
				//якщо потрібно оновити лише "найновіші пости", наприклад 10 останніх - передати більше 0. Якщо 0 - то оновити усі
				LimitCount = limitUpdateCount
			};

			ConsoleLogger logger = new ConsoleLogger();

			await databaseFromSitemapUpdater.UpdateFromSitemap(_SITEMAP_POSTS_URL, loader, logger);
		}

		/// <summary>
		/// Оновлює базу блобів та таблицу (mongodb). Файли .gif шукаються у локальній папці, якщо потрібно - завантажуються до хмарного сховища
		/// </summary>
		/// <param name="azureBlobConnectionString"></param>
		/// <param name="azureDBSitePagesConnectionString"></param>
		/// <param name="sourceFolder"></param>
		/// <returns></returns>
		static async Task _UpdateGifFromDisk(string azureBlobConnectionString, string azureDBSitePagesConnectionString, string sourceFolder)
		{
			ConsoleLogger logger = new ConsoleLogger();

			//конейнер блобів - відповідає лише за збереження бінар.файлів за спец.іменуванням
			var animatedImagesBlobContainer = new AnimatedImagesBlobContainer(azureBlobConnectionString);

			//пошук усіх варіантів gif-файлів на диску...
			var files = Directory.EnumerateFiles(sourceFolder, _GIF_FILES_MASK, SearchOption.AllDirectories);

			int count = 0;
			int counterForLog = 0;
			int added = 0;
			foreach (var fileName in files)
			{
				int t2 = Environment.TickCount;
				//UploadImage перевіряє, якщо такий файл є, нічого не робить
				string blobName = animatedImagesBlobContainer.UploadImage(fileName, out bool upladedNewImage);
				t2 = Environment.TickCount - t2;

				if (upladedNewImage)
				{
					logger.Log("{0} - uploaded to blob: {1} ({2} ms)", fileName, blobName, t2);
					added++;
				}

				count++;
				counterForLog++;
				if (counterForLog >= 25)
				{
					logger.Log("Processed {0} files...", count);
					counterForLog = 0;
				}

			}//foreach

			logger.Log("Processed {0} files, new uploaded to azure: {1}", count, added);

			//Тепер необхідно "актуалізувати" базу у mongodb - виконати до-вантаження (або створити) базу анімованих зображень з наявних блоб-об'єктів
			//TODO@: тут немає варіанту якщо блоб було "видалено", подумати про це

			DatabaseUpdater upd = new DatabaseUpdater(azureBlobConnectionString, azureDBSitePagesConnectionString, logger);
			await upd.UpdateAnimatedBlobDataBase();
		}

	}
}
