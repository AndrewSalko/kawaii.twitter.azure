using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.HtmlParsers
{
	public interface IImageOnWeb
	{
		Task<ImageInfo> Download(string url);
	}
}
