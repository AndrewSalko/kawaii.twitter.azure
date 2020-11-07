using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace kawaii.twitter.azure.func
{
	class Logger : kawaii.twitter.Logs.ILogger
	{
		/// <summary>
		/// Возможные значения: all, error (любое другое значение - не вести лог вообще)
		/// </summary>
		public const string ENV_LOGING_LEVEL = "kawaii_twitter_azure_log_level";

		public const string LOG_LEVEL_ALL = "all";
		public const string LOG_LEVEL_ERROR = "error";

		Microsoft.Extensions.Logging.ILogger _Log;

		string _LogLevel;

		public Logger(Microsoft.Extensions.Logging.ILogger log)
		{
			string envLogLevel = Environment.GetEnvironmentVariable(ENV_LOGING_LEVEL);
			if (string.IsNullOrEmpty(envLogLevel))
			{
				return;	//не вести лог вообще
			}

			_LogLevel = envLogLevel;
			_Log = log;
		}

		public void Log(string format, params object[] args)
		{
			if (_Log == null)
				return;

			if (_LogLevel != LOG_LEVEL_ALL)
				return;

			_Log.LogInformation(format, args);
		}

		public void LogError(string format, params object[] args)
		{
			if (_Log == null)
				return;

			if (_LogLevel != LOG_LEVEL_ALL && _LogLevel != LOG_LEVEL_ERROR)
				return;

			_Log.LogError(format, args);
		}

	}
}
