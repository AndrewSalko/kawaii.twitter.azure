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
		IAnimatedSelector _AnimatedSelectorForNewImages;

		/// <summary>
		/// Знаходить усі анім.зображення для вказаного посту (за url)
		/// </summary>
		IFindAnimatedByPage _FindAnimatedByPage;

		/// <summary>
		/// Знайде сторінку за іменем анімованого блоб-зображення (якщо у нас є code-geass:geass.gif, то знайде сам пост)
		/// </summary>
		IFindPageByBlobName _FindPageByBlobName;

		/// <summary>
		/// Определяет использовать ли изображение из поста или внешнее изображение
		/// </summary>
		IPageOrExternalImageSelector _PageOrExternalImageSelector;

		/// <summary>
		/// Для выбора из набора аним.изображений случайного, но с учетом "не выбирать то, что твитили недавно"
		/// </summary>
		IAnimatedSelectorWithExcludeLast _AnimatedSelectorWithExcludeLast;

		kawaii.twitter.Logs.ILogger _Log;

		public PageForTwittingSelector(IPageSelector pageSelectorForNewPages, IAnimatedSelector animatedSelectorForNewImages, IFindPageByBlobName findPageByBlobName, IPageSelector pageSelectorForAnyPages, IFindAnimatedByPage findAnimatedByPage, IPageOrExternalImageSelector pageOrExternalImageSelector, IAnimatedSelectorWithExcludeLast animatedSelectorWithExcludeLast, kawaii.twitter.Logs.ILogger log)
		{
			_PageSelectorForNewPages = pageSelectorForNewPages ?? throw new ArgumentNullException(nameof(pageSelectorForNewPages));
			_AnimatedSelectorForNewImages = animatedSelectorForNewImages ?? throw new ArgumentNullException(nameof(animatedSelectorForNewImages));
			_FindPageByBlobName = findPageByBlobName ?? throw new ArgumentNullException(nameof(findPageByBlobName));
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

			//Если попали сюда значит нет новых страниц, но может есть новые gif-файлы? (которые НИ разу не твитили)
			//Важно отметить, что новые гиф-изображения по сути приоритетно решают кого твитнем (пусть даже эта страница недавно и проходила)
			AnimatedImage img = await _AnimatedSelectorForNewImages.GetAnimatedImageForTwitting();
			if (img != null)
			{
				//здесь нам все равно нужно найти страницу, просто изображение будет взято из аним.гифки что нашли
				SitePage blobPage = await _FindPageByBlobName.Find(img.BlobName);
				if (blobPage == null)
				{
					throw new ApplicationException("Find page by blob name failed for:" + img.BlobName);
				}

				TwittData result = new TwittData
				{
					Page = blobPage,
					Image = img
				};
				return result;
			}

			//на этом этапе у нас все страницы и все гифки уже твитились
			//В этом случае начинает работать схема - "выбрать только пост, а потом уточнить если есть у него гифки, то случайно решить то ли изображение из поста, то ли гифка"
			//Здесь селектор должен быть умен в плане предлагать вначале более "старо-твиченные посты"
			SitePage pageSelected = await _PageSelectorForAnyPages.GetPageForTwitting();

			if (pageSelected == null)
			{
				throw new ApplicationException("No page found for twitting");	//это правда необычно..скорее ошибка т.к. база ведь не пустая
			}

			//получить связанные с ней аним.изображения, и если они есть, решить - будем показывать аним.изображение или изображение из поста (случайное)
			//(здесь плохо то, что весь массив imgsForPage в памяти..но пока их не слишком много это не проблема)
			AnimatedImage[] imgsForPage = await _FindAnimatedByPage.GetAnimatedImagesForPage(pageSelected.URL);
			if (imgsForPage != null && imgsForPage.Length > 0)
			{
				//теперь решаем: то ли просто страница , то ли используем аним.изображение что нашли (для этой страницы)
				if (_PageOrExternalImageSelector.UseExternalAnimatedImage)
				{
					//выбираем случайно аним.гифку, кроме той которую твитили последний раз (если их более чем одна)
					var animImg = _AnimatedSelectorWithExcludeLast.SelectImage(imgsForPage);

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
