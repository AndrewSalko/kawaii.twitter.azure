using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.db;

namespace kawaii.twitter.core
{
	public interface IAnimatedSelector
	{
		Task<AnimatedImage> GetAnimatedImageForTwitting();
	}
}
