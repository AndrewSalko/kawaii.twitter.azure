using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.tests.TweetCreator.Stubs
{
	class Logger : kawaii.twitter.Logs.ILogger
	{
		public Logger()
		{
		}

		public void Log(string format, params object[] args)
		{
		}

		public void LogError(string format, params object[] args)
		{
		}
	}
}
