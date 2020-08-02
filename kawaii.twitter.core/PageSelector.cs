using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.db;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace kawaii.twitter.core
{
	class PageSelector: IPageSelector
	{
		IMongoCollection<SitePage> _Pages;
		int _TopQueryCount;
		IRandomSelector _RandomSelector;

		public PageSelector(IMongoCollection<SitePage> pages, int topQueryCount, IRandomSelector randomSelector)
		{
			if (topQueryCount <= 0)
				throw new ArgumentException("topQueryCount повинно бути більше ніж 0", nameof(topQueryCount));

			_Pages = pages ?? throw new ArgumentNullException(nameof(pages));
			_TopQueryCount = topQueryCount;
			_RandomSelector = randomSelector ?? throw new ArgumentNullException(nameof(randomSelector));
		}

		public async Task<SitePage> GetPageForTwitting()
		{
			string currentSpecialDay = null;

			//В этом случае начинает работать схема - "выбрать только пост, а потом уточнить если есть у него гифки, то случайно решить то ли изображение из поста, то ли гифка"
			long pagesCountLong = await _Pages.CountDocumentsAsync(x => !x.Blocked && x.SpecialDay == currentSpecialDay);
			int pagesCount = (int)pagesCountLong;   //это не супер красиво, но на самом деле маловероятно что у нас будет так много данных в базе

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
