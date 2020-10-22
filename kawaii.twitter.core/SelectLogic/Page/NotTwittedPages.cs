using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core.SelectLogic.Page
{
	/// <summary>
	/// Знаходить у базі нові пости, які ще жодного разу не твітіли
	/// </summary>
	public class NotTwittedPages: IPageSelector
	{
		IMongoCollection<SitePage> _Pages;
		int _TopQueryCount;
		IRandomSelector _RandomSelector;

		string _SpecialDayName;

		public NotTwittedPages(IMongoCollection<SitePage> pages, IRandomSelector randomSelector, string specialDayName, int topQueryCount)
		{
			_Pages = pages ?? throw new ArgumentNullException(nameof(pages));
			_RandomSelector=randomSelector ?? throw new ArgumentNullException(nameof(randomSelector));

			_SpecialDayName = specialDayName;	//этот может быть null если сейчас не "особый день"

			if (topQueryCount <= 0)
				throw new ArgumentException("topQueryCount повинно бути більше ніж 0", nameof(topQueryCount));

			_TopQueryCount = topQueryCount;
		}

		public async Task<SitePage> GetPageForTwitting()
		{
			string currentSpecialDay = _SpecialDayName;

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
