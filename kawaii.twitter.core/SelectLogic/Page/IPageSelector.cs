using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.db;

namespace kawaii.twitter.core.SelectLogic.Page
{
	public interface IPageSelector
	{
		Task<SitePage> GetPageForTwitting();
	}
}
