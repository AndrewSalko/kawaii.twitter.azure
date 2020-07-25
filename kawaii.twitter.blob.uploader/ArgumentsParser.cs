using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.blob.uploader
{
	class ArgumentsParser
	{
		public const string ARG_NAME_FOLDER = "-folder";

		/// <summary>
		/// DefaultEndpointsProtocol=https;AccountName=youracc;AccountKey=yourkey;EndpointSuffix=core.windows.net
		/// </summary>
		public const string ARG_NAME_AZURE_BLOB_ANIMATED_CONNECTION_STRING = "-blobgifs";

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
				}
				

				if (currentArgType == ArgumentTypes.Unspecified)
					continue;   //не зрозуміло що це, порушено порядок аргументів

				_Arguments[currentArgType] = arg;
				currentArgType = ArgumentTypes.Unspecified;

			}//for
		}

		public string Folder
		{
			get
			{
				return _Arguments[ArgumentTypes.Folder];
			}
		}

		public string BlobGifsConnectionString
		{
			get
			{
				return _Arguments[ArgumentTypes.AnimatedBlobsConnectionString];
			}
		}

	}
}
