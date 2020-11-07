using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.Logs;

namespace kawaii.twitter.console
{
	class ConsoleLogger : ILogger
	{
		public static string GetFormatted(string format, params object[] args)
		{
			if (args == null || args.Length == 0)
				return format;

			try
			{
				string result = string.Format(format, args);
				return result;
			}
			catch (Exception)
			{
				return format;
			}
		}

		public void Log(string format, params object[] args)
		{
			Console.WriteLine(GetFormatted(format, args));
		}

		public void LogError(string format, params object[] args)
		{
			Console.WriteLine(GetFormatted(format, args));
		}
	}
}
