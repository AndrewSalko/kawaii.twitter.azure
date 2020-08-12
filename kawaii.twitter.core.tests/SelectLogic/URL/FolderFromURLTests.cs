using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.URL;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace kawaii.twitter.core.tests.SelectLogic.URL
{

	[TestClass]
	public class FolderFromURLTests
	{

		[TestMethod]
		[Description("Аргумент null - выброс исключения")]
		[TestCategory("FolderFromURL")]
		public void FolderFromURL_ArgumentNull_Exception()
		{
			var folderFromURL = new FolderFromURL();

			try
			{
				folderFromURL.GetFolderFromURL(null);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "pageURL");
			}
		}

		[TestMethod]
		[Description("Парсинг url и получение папки поста")]
		[TestCategory("FolderFromURL")]
		public void FolderFromURL_NormalFlow_With_LastSlash()
		{
			var folderFromURL = new FolderFromURL();

			string url = "https://kawaii-mobile.com/2020/08/princess-connect-redive/";
			string expectedResult = "princess-connect-redive";

			string result = folderFromURL.GetFolderFromURL(url);

			Assert.IsTrue(expectedResult == result);
		}

		[TestMethod]
		[Description("Парсинг url и получение папки поста")]
		[TestCategory("FolderFromURL")]
		public void FolderFromURL_NormalFlow_Without_LastSlash()
		{
			var folderFromURL = new FolderFromURL();

			string url = "https://kawaii-mobile.com/2020/08/princess-connect-redive";
			string expectedResult = "princess-connect-redive";

			string result = folderFromURL.GetFolderFromURL(url);

			Assert.IsTrue(expectedResult == result);
		}

		[TestMethod]
		[Description("Парсинг url в котором пустой сегмент папки, выброс исключения")]
		[TestCategory("FolderFromURL")]
		public void FolderFromURL_InvalidURL_FolderEmpty_Exception()
		{
			var folderFromURL = new FolderFromURL();

			//это правильный урл, но папки у него нет (сегмент один и это слеш, толку от него нет)
			string url = "https://kawaii-mobile.com/";

			try
			{
				string result = folderFromURL.GetFolderFromURL(url);

				Assert.Fail("Очікувалося ApplicationException");
			}
			catch (ApplicationException ex)
			{
				Assert.IsTrue(ex.Message.Contains(url) && ex.Message.Contains("Invalid url"));
			}
		}

		[TestMethod]
		[Description("Парсинг url в котором пустой сегмент папки, выброс исключения")]
		[TestCategory("FolderFromURL")]
		public void FolderFromURL_InvalidURL_Exception()
		{
			var folderFromURL = new FolderFromURL();

			//это правильный урл, но папки у него нет (сегмент один и это слеш, толку от него нет)
			string url = "kawaii-mobile.com";

			try
			{
				string result = folderFromURL.GetFolderFromURL(url);

				Assert.Fail("Очікувалося UriFormatException");
			}
			catch (UriFormatException)
			{
			}
		}



	}
}
