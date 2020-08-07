using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core
{
	/// <summary>
	/// Знаходить у базі нові пости, які ще жодного разу не твітіли
	/// </summary>
	public class NotTwittedPages: IPageSelector
	{
		IMongoCollection<SitePage> _Pages;
		int _TopQueryCount;
		IRandomSelector _RandomSelector;

		public NotTwittedPages(IMongoCollection<SitePage> pages, int topQueryCount, IRandomSelector randomSelector)
		{
			if (topQueryCount <= 0)
				throw new ArgumentException("topQueryCount повинно бути більше ніж 0", nameof(topQueryCount));

			_Pages = pages ?? throw new ArgumentNullException(nameof(pages));
			_TopQueryCount = topQueryCount;
			_RandomSelector=randomSelector ?? throw new ArgumentNullException(nameof(randomSelector));
		}

		public async Task<SitePage> GetPageForTwitting()
		{
			//TODO@: вычислить ивент-время, и использовать в запросе
			string currentSpecialDay = null;

			var pagesNotTwitted = (from page in _Pages.AsQueryable() where (!page.Blocked && page.TweetDate == null && page.SpecialDay == currentSpecialDay) select page).Take(_TopQueryCount);

			if (pagesNotTwitted != null)
			{
				var list = await pagesNotTwitted.ToListAsync();
				if (list.Count > 0)
				{
					int ind= _RandomSelector.GetRandomIndex(list.Count);
					SitePage resultPage = list[ind];
					return resultPage;
				}
			}

			return null;
		}

	}
}
