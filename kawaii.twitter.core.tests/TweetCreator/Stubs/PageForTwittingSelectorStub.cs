using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic;
using kawaii.twitter.db;

namespace kawaii.twitter.core.tests.TweetCreator.Stubs
{
	class PageForTwittingSelectorStub: IPageForTwittingSelector
	{
		public PageForTwittingSelectorStub()
		{
		}

		/// <summary>
		/// Установить извне - эта страница будет результатом "выбора"
		/// </summary>
		public SitePage ResultPage
		{
			get;
			set;
		}

		public async Task<TwittData> GetPageForTwitting()
		{
			TwittData twittData = new TwittData
			{
				Page = ResultPage
			};

			Task<TwittData> task = new Task<TwittData>(()=>
			{
				return twittData;
			});

			task.Start();
			return await task;
		}
	}
}
