using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.console
{
	class ArgumentsParser
	{
		public const string ARG_NAME_FOLDER = "-folder";

		/// <summary>
		/// DefaultEndpointsProtocol=https;AccountName=youracc;AccountKey=yourkey;EndpointSuffix=core.windows.net
		/// </summary>
		public const string ARG_NAME_AZURE_BLOB_ANIMATED_CONNECTION_STRING = "-blobgifs";

		/// <summary>
		/// Строка підключення до бази постів сайту
		/// </summary>
		public const string ARG_NAME_AZURE_DB_SITEPAGES_CONNECTION_STRING = "-dbsitepages";

		/// <summary>
		/// Очікується -dbupdate animated
		/// </summary>
		public const string ARG_NAME_UPDATE_DATABASE = "-dbupdate";

		/// <summary>
		/// Оновити базу анімованих зображень
		/// </summary>
		public const string ARG_VALUE_ANIMATED = "animated";

		/// <summary>
		/// Оновити базу постів (з xml карти сайту) - цей варіант працює довго, перевіряє усі пости
		/// </summary>
		public const string ARG_VALUE_POSTS_ALL = "allposts";

		/// <summary>
		/// Оновити базу постів (перші 5 постів) аналізуються з карти сайту
		/// </summary>
		public const string ARG_VALUE_POSTS_RECENT = "recentposts";

		Dictionary<ArgumentTypes, string> _Arguments = new Dictionary<ArgumentTypes, string>();

		public ArgumentsParser(string[] args)
		{
			if (args == null || args.Length == 0)
				return;

			ArgumentTypes currentArgType = ArgumentTypes.Unspecified;

			for (int i = 0; i < args.Length; i++)
			{
				string arg = args[i];

				if (string.Compare(arg, ARG_NAME_FOLDER, true) == 0)
				{
					//наступний аргумент - значення
					currentArgType = ArgumentTypes.Folder;
					continue;
				}
				else
				{
					if (string.Compare(arg, ARG_NAME_AZURE_BLOB_ANIMATED_CONNECTION_STRING, true) == 0)
					{
						currentArgType = ArgumentTypes.AnimatedBlobsConnectionString;
						continue;
					}
					else
					{
						if (string.Compare(arg, ARG_NAME_UPDATE_DATABASE, true) == 0)
						{
							currentArgType = ArgumentTypes.UpdateDatabase;
							continue;
						}
						else
						{
							if (string.Compare(arg, ARG_NAME_AZURE_DB_SITEPAGES_CONNECTION_STRING, true) == 0)
							{
								currentArgType = ArgumentTypes.SitePagesConnectionString;
								continue;
							}
						}
					}
				}


				if (currentArgType == ArgumentTypes.Unspecified)
					continue;   //не зрозуміло що це, порушено порядок аргументів

				_Arguments[currentArgType] = arg;
				currentArgType = ArgumentTypes.Unspecified;

			}//for
		}

		public string DBUpdate
		{
			get
			{
				_Arguments.TryGetValue(ArgumentTypes.UpdateDatabase, out string dbUpdateValue);
				return dbUpdateValue;
			}
		}

		/// <summary>
		/// Робоча папка, з якої треба до-завантажити gif-файли. Якщо не передано, не виконуєтсья
		/// </summary>
		public string Folder
		{
			get
			{
				_Arguments.TryGetValue(ArgumentTypes.Folder, out string folder);
				return folder;
			}
		}

		/// <summary>
		/// Строка підключення до блоб-сховища gif-файлів
		/// </summary>
		public string BlobGifsConnectionString
		{
			get
			{
				_Arguments.TryGetValue(ArgumentTypes.AnimatedBlobsConnectionString, out string blobConnectionString);
				return blobConnectionString;
			}
		}

		/// <summary>
		/// Строка підключення до бази сторінок сайту
		/// </summary>
		public string SitePagesConnectionString
		{
			get
			{
				_Arguments.TryGetValue(ArgumentTypes.SitePagesConnectionString, out string connectionString);
				return connectionString;
			}
		}
	}

}
