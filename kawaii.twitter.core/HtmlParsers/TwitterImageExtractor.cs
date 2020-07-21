using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kawaii.twitter.core.HtmlParsers
{
	/// <summary>
	/// Аналізує html-тіло та знаходить посилання на twitter-image якщо воно є
	/// </summary>
	public class TwitterImageExtractor
	{
		const string _TWITTER_IMAGE_HTML_TAG = "twitter:image";
		const string _TWITTER_IMAGE_HTML_CONTENT = "content=\"";

		public string ExtractImageURL(string htmlBody)
		{
			string resultURL = null;

			int twImgStart = htmlBody.IndexOf(_TWITTER_IMAGE_HTML_TAG);
			if (twImgStart >= 0)
			{
				int contentEqStart = htmlBody.IndexOf(_TWITTER_IMAGE_HTML_CONTENT, twImgStart + _TWITTER_IMAGE_HTML_TAG.Length);
				if (contentEqStart >= 0)
				{
					int lastQuoteInd = htmlBody.IndexOf("\"", contentEqStart + _TWITTER_IMAGE_HTML_CONTENT.Length);

					resultURL = htmlBody.Substring(contentEqStart + _TWITTER_IMAGE_HTML_CONTENT.Length, lastQuoteInd - (contentEqStart + _TWITTER_IMAGE_HTML_CONTENT.Length));
					return resultURL;
				}
			}

			return resultURL;
		}
	}
}
