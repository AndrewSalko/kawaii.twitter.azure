using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.BlobName;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kawaii.twitter.core.tests.SelectLogic.BlobName
{
	[TestClass]
	public class FormatterTests
	{

		[TestMethod]
		[Description("Тест проверки аргумента на null")]
		[TestCategory("Formatter")]
		public void GetBlobNamePrefix_Argument_Null()
		{
			var frm = new Formatter();

			try
			{
				frm.GetBlobNamePrefix(null);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "postFolderName");
			}
		}

		[TestMethod]
		[Description("Тест нормальной работы форматтера")]
		[TestCategory("Formatter")]
		public void GetBlobNamePrefix_Normal()
		{
			var frm = new Formatter();

			string folderName = "shuumatsu-no-izetta";
			string expectedResult = "shuumatsu-no-izetta:";

			string prefix=frm.GetBlobNamePrefix(folderName);

			Assert.IsTrue(prefix == expectedResult);
		}


	}
}
