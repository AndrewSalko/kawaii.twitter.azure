using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using kawaii.twitter.core.SelectLogic.Images;
using kawaii.twitter.core.SelectLogic.Images.ExcludeUsed;
using kawaii.twitter.core.SelectLogic.Images.Find;
using kawaii.twitter.core.SelectLogic.Page;
using kawaii.twitter.core.SelectLogic.PageOrExternalImage;
using kawaii.twitter.core.SelectLogic.SpecialDay;
using kawaii.twitter.db;

namespace kawaii.twitter.core.SelectLogic
{
	/// <summary>
	/// Содержит логику выбора "какую страницу будем постить в твиттер", с учетом анимированных изображений,
	/// и особенностей кого твитили и как давно
	/// </summary>
	public class PageForTwittingSelector: IPageForTwittingSelector
	{
		/// <summary>
		/// Знайде ті пости, які жодного разу не твітіли
		/// </summary>
		IPageSelector _PageSelectorForNewPages;

		/// <summary>
		/// Загальний механізм обрання поста для твітінгу
		/// </summary>
		IPageSelector _PageSelectorForAnyPages;

		/// <summary>
		/// Знайде ті gif-зображення, які ще не твітіли
		/// </summary>
		IFindAnimatedByPage _FindNewAnimatedByPage;

		/// <summary>
		/// Знаходить усі анім.зображення для вказаного посту (за url)
		/// </summary>
		IFindAnimatedByPage _FindAnimatedByPage;

		/// <summary>
		/// Определяет использовать ли изображение из поста или внешнее изображение
		/// </summary>
		IPageOrExternalImageSelector _PageOrExternalImageSelector;

		/// <summary>
		/// Для выбора из набора аним.изображений случайного, но с учетом "не выбирать то, что твитили недавно"
		/// </summary>
		IAnimatedSelectorWithExcludeLast _AnimatedSelectorWithExcludeLast;

		kawaii.twitter.Logs.ILogger _Log;

		public PageForTwittingSelector(IPageSelector pageSelectorForNewPages, IFindAnimatedByPage findNewAnimatedByPage, IPageSelector pageSelectorForAnyPages, IFindAnimatedByPage findAnimatedByPage, IPageOrExternalImageSelector pageOrExternalImageSelector, IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast, kawaii.twitter.Logs.ILogger log)
		{
			_PageSelectorForNewPages = pageSelectorForNewPages ?? throw new ArgumentNullException(nameof(pageSelectorForNewPages));
			_FindNewAnimatedByPage = findNewAnimatedByPage ?? throw new ArgumentNullException(nameof(findNewAnimatedByPage));
			_PageSelectorForAnyPages = pageSelectorForAnyPages ?? throw new ArgumentNullException(nameof(pageSelectorForAnyPages));
			_FindAnimatedByPage = findAnimatedByPage ?? throw new ArgumentNullException(nameof(findAnimatedByPage));
			_PageOrExternalImageSelector = pageOrExternalImageSelector ?? throw new ArgumentNullException(nameof(pageOrExternalImageSelector));
			_AnimatedSelectorWithExcludeLast = animatedSelectorWithExcludeLast ?? throw new ArgumentNullException(nameof(animatedSelectorWithExcludeLast));
			_Log = log ?? throw new ArgumentNullException(nameof(log));
		}

		public async Task<TwittData> GetPageForTwitting()
		{
			//В базе есть страницы для всех постов сайта. В некоторых случаях (но не всегда) к посту может быть дополнительно
			//доступны N gif-анимированных изображений (они в отдельной коллекции)

			//Мы будем твитить тех, кого "ни разу", в первую очередь, но только если они не блокированы и не принадлежат к спец-ивенту
			//Спец.ивент - это Рождество (новогодние посты), и Хеллоуин - их твитить надо строго в опред.диапазоне времени

			//Первая часть логики - "Если пост новый, то без вариантов твитим его, изображение берем из него же" (никаких внешних аним.гифок)
			SitePage page = await _PageSelectorForNewPages.GetPageForTwitting();
			if (page != null)
			{
				_Log.Log("_PageSelectorForNewPages.GetPageForTwitting done {0}", DateTime.Now);

				TwittData result = new TwittData
				{
					Page = page
				};
				return result;
			}

			await Task.Delay(1100);

			//на этом этапе у нас все новые страницы и уже твитились
			//В этом случае начинает работать схема - "выбрать только пост, а потом уточнить если есть у него гифки, то случайно решить то ли изображение из поста, то ли гифка"
			//Здесь селектор должен быть умен в плане предлагать вначале более "старо-твиченные посты"
			SitePage pageSelected = await _PageSelectorForAnyPages.GetPageForTwitting();
			if (pageSelected == null)
			{
				throw new ApplicationException("No page found for twitting");	//это правда необычно..скорее ошибка т.к. база ведь не пустая
			}

			await Task.Delay(1100);

			//теперь решаем: то ли просто страница , то ли используем аним.изображение (для этой страницы, если конечно они есть)
			if (_PageOrExternalImageSelector.UseExternalAnimatedImage)
			{
				//Пробуем использовать аним.изображение, но это ЕСЛИ оно найдется для этой страницы (бывают страницы без них вообще)

				string url = pageSelected.URL;
				AnimatedImage animImg = null;

				AnimatedImage[] imgsNew = await _FindNewAnimatedByPage.GetAnimatedImagesForPage(url);
				if (imgsNew != null)
				{
					//берем первую что нашли - эти гифки не твитили ни разу (скорее всего она будет одна в результате-ответе)
					animImg = imgsNew[0];
				}
				else
				{
					await Task.Delay(500);

					//новых нет, но может есть "не новые" гиф-файлы?
					AnimatedImage[] imgsForPage = await _FindAnimatedByPage.GetAnimatedImagesForPage(url);
					if (imgsForPage != null && imgsForPage.Length > 0)
					{
						//выбираем случайно аним.гифку, кроме той которую твитили последний раз (если их более чем одна)
						animImg = _AnimatedSelectorWithExcludeLast.SelectImage(imgsForPage);
					}
				}

				if (animImg != null)
				{
					TwittData result = new TwittData
					{
						Image = animImg,
						Page = pageSelected
					};
					return result;
				}
			}

			//просто страницу
			TwittData resultData = new TwittData
			{
				Page = pageSelected
			};
			return resultData;

		}//GetPageForTwitting

	}
}
