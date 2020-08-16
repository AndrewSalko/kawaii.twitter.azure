using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core.SelectLogic.Page
{
	/// <summary>
	/// Выбирает из всех страниц одну случайным образом, но среди тех кого не твитили достаточно давно
	/// </summary>
	public class PageSelector: IPageSelector
	{
		/// <summary>
		/// Сколько страниц "отбираем" для случайной выборки (страницы сортированы по дате твита. Мы берем первые N (например 5 самых старых)),
		/// и рандомно выберем из них одну для твита
		/// </summary>
		int _MaxPagesForRandomSelection;

		IMongoCollection<SitePage> _Pages;
		IRandomSelector _RandomSelector;

		public PageSelector(IMongoCollection<SitePage> pages, IRandomSelector randomSelector, int maxPagesForRandomSelection)
		{
			if (maxPagesForRandomSelection <= 0)
				throw new ArgumentException("maxPagesForRandomSelection must be greater than zero", nameof(maxPagesForRandomSelection));

			_Pages = pages ?? throw new ArgumentNullException(nameof(pages));
			_RandomSelector = randomSelector ?? throw new ArgumentNullException(nameof(randomSelector));
			_MaxPagesForRandomSelection = maxPagesForRandomSelection;
		}

		public async Task<SitePage> GetPageForTwitting()
		{
			string currentSpecialDay = null;

			//В этом случае начинает работать схема - "выбрать только пост, а потом уточнить если есть у него гифки, то случайно решить то ли изображение из поста, то ли гифка"
			long pagesCountLong = await _Pages.CountDocumentsAsync(x => !x.Blocked && x.SpecialDay == currentSpecialDay);
			int pagesCount = (int)pagesCountLong;   //это не супер красиво, но на самом деле маловероятно что у нас будет так много данных в базе

			if (pagesCount > _MaxPagesForRandomSelection)
			{
				pagesCount = _MaxPagesForRandomSelection;
			}

			int indForPage = _RandomSelector.GetRandomIndex(pagesCount);

			//выбрать одну случайную страницу
			var pagesRnd = await (from page in _Pages.AsQueryable() where (!page.Blocked && page.SpecialDay == currentSpecialDay) orderby page.TweetDate.Value select page).Skip(indForPage).Take(1).ToListAsync();
			if (pagesRnd.Count > 0)
			{
				var resultPage = pagesRnd[0];
				return resultPage;
			}

			return null;
		}


	}
}
