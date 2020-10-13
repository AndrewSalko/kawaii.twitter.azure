using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace kawaii.twitter.db
{
	public interface IDatabase
	{
		IMongoDatabase DB
		{
			get;
		}

	}
}
