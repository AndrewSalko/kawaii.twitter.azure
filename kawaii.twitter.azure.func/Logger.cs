using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace kawaii.twitter.azure.func
{
	class Logger : kawaii.twitter.Logs.ILogger
	{
		Microsoft.Extensions.Logging.ILogger _Log;

		public Logger(Microsoft.Extensions.Logging.ILogger log)
		{
			_Log = log;
		}

		public void Log(string format, params object[] args)
		{
			_Log.LogInformation(format, args);
		}
	}
}
