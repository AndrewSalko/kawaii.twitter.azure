using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.PageOrExternalImage;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace kawaii.twitter.core.tests.SelectLogic.PageOrExternalImage
{
	/// <summary>
	/// Тесты на заданную "вероятность" случайного выбора "использовать ли внешнее аним.изображение"
	/// </summary>
	[TestClass]
	public class PageOrExternalImageSelectorTests
	{

		[TestMethod]
		[Description("Создает тестовую базу в MongoDB, испытывает поиск страницы по блоб-имени и проверка найденного результата")]
		[TestCategory("PageOrExternalImageSelector")]
		public void PageOrExternalImageSelector_Normal_Test()
		{
			PageOrExternalImageSelector selector = new PageOrExternalImageSelector();

			int testCount = 100;
			//на испытании в 100 вызовов подсчитаем сколько он скажет true
			//0-1-2 == выдает true, 3-4-5-6-7-8-9
			//Примерно 29-30% (ну допустим, по рандому это будет и меньше, но где-то до 20 должно быть точно)

			int useExtCount = 0;

			for (int i = 0; i < testCount; i++)
			{
				bool useExt = selector.UseExternalAnimatedImage;
				if (useExt)
				{
					useExtCount++;
				}
			}

			Assert.IsTrue(useExtCount >= 20, $"UseExternalAnimatedImage недостаточно много случаев: {useExtCount}");
			Assert.IsTrue(useExtCount != testCount, $"UseExternalAnimatedImage не должен всегда возвращаться");
		}

	}
}
