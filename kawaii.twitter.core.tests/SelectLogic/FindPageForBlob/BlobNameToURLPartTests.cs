using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kawaii.twitter.core.tests.SelectLogic.FindPageForBlob
{
	[TestClass]
	public class BlobNameToURLPartTests
	{

		[TestMethod]
		[Description("Тест проверки аргумента на null")]
		[TestCategory("BlobNameToURL")]
		public void GetURLPart_Argument_Null()
		{
			var bl = new BlobNameToURLPart();

			try
			{
				bl.GetURLPart(null);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "blobName");
			}
		}

		[TestMethod]
		[Description("Нормальный парсинг")]
		[TestCategory("BlobNameToURL")]
		public void GetURLPart_Normal()
		{
			var bl = new BlobNameToURLPart();

			//Наприклад для урл: https://kawaii-mobile.com/2017/01/shuumatsu-no-izetta/
			//його блоб має вигляд: shuumatsu-no-izetta:img1.gif

			string blobName = "shuumatsu-no-izetta:img1.gif";
			string expectedResult = "/shuumatsu-no-izetta/";

			string result = bl.GetURLPart(blobName);

			Assert.IsTrue(result == expectedResult);
		}

		[TestMethod]
		[Description("Неверный формат аргумента")]
		[TestCategory("BlobNameToURL")]
		public void GetURLPart_Argument_Invalid_Format()
		{
			var bl = new BlobNameToURLPart();

			//Формат имени блоба должен содержать двоеточие в обязат.порядке - "папка:имя-файла.gif"
			string blobName = "shuumatsu-no-izetta";

			try
			{
				bl.GetURLPart(blobName);
				Assert.Fail("Очікувалося ArgumentException");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "blobName");
			}
		}


	}
}
