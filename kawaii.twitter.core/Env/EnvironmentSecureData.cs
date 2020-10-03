using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.Env
{
	/// <summary>
	/// Для отримання різних "секретних" даних з змінних середовища
	/// </summary>
	public class EnvironmentSecureData
	{
		public const string ENV_PREFIX = "env:";

		public static string GetValueFromEnvironment(string text)
		{
			if (string.IsNullOrEmpty(text))
				throw new ArgumentNullException(nameof(text));

			if (text.StartsWith(ENV_PREFIX))
			{
				//це зміння середовища, справжне значення у ньому
				string varName = text.Substring(ENV_PREFIX.Length);

				string result = Environment.GetEnvironmentVariable(varName);
				return result;
			}
			else
			{
				return text;
			}
		}
	}
}
