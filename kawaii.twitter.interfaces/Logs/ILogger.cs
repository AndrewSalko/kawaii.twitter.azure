using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.Logs
{
	public interface ILogger
	{
		void Log(string format, params object[] args);
	}
}
