using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.db;

namespace kawaii.twitter.core
{
	public interface IFindAnimatedByPage
	{
		Task<AnimatedImage[]> GetAnimatedImagesForPage(string pageURL);
	}
}
