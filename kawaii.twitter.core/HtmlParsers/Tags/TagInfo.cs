using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.HtmlParsers.Tags
{
	public class TagInfo
	{
		public TagInfo(string name)
		{
			Name = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(name) : name;
		}

		public string Name
		{
			get;
			private set;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
