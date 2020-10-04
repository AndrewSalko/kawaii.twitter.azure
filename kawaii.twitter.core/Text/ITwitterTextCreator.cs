using System;
using System.Collections.Generic;
using System.Text;

namespace kawaii.twitter.core.Text
{
	public interface ITwitterTextCreator
	{
		string CreateTwitterText(string url, string title);
	}
}
