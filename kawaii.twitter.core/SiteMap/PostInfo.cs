using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.SiteMap
{
	public class PostInfo
	{
		public PostInfo()
		{
		}

		public PostInfo(string htmlBody)
		{
			HtmlBody = htmlBody;
		}

		public string HtmlBody
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

	}
}
