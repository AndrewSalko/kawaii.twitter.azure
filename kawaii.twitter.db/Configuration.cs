using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace kawaii.twitter.db
{
	public class Configuration
	{
		public const string MAIN_CONFIG_UNIQUE_NAME = "configuration.kawaii-mobile.com";

		[BsonId]
		public MongoDB.Bson.ObjectId Id
		{
			get;
			set;
		}

		/// <summary>
		/// Уникальное имя (идентификация) документа-конфигурации
		/// </summary>
		[BsonRequired]
		public string UniqueName
		{
			get;
			set;
		}

		/// <summary>
		/// Дата модификации карты сайта post.xml
		/// </summary>
		[BsonIgnoreIfNull]
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime? PostXMLSiteMapLastModified
		{
			get;
			set;
		}

		public override string ToString()
		{
			return UniqueName;
		}


	}
}
