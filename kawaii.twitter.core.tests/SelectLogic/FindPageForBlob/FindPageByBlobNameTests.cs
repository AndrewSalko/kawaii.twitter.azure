using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.FindPageForBlob;
using kawaii.twitter.core.tests.SelectLogic.Stubs;
using kawaii.twitter.db;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace kawaii.twitter.core.tests.SelectLogic.FindPageForBlob
{
	[TestClass]
	public class FindPageByBlobNameTests
	{

		[TestMethod]
		[Description("Тест конструктора - аргумент pages null")]
		[TestCategory("FindPageByBlobName")]
		public void FindPageByBlobName_Ctor_Argument_Pages_Null()
		{
			try
			{
				var fnd = new FindPageByBlobName(null, new BlobNameToURLPart());

				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "pages");
			}
		}

		[TestMethod]
		[Description("Тест конструктора - аргумент blobNameToURLPart null")]
		[TestCategory("FindPageByBlobName")]
		public void FindPageByBlobName_Ctor_Argument_BlobNameToURLPart_Null()
		{
			try
			{
				QueryableStub<SitePage> queryableStub = new QueryableStub<SitePage>();

				var fnd = new FindPageByBlobName(queryableStub, null);

				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "blobNameToURLPart");
			}
		}

	}
}
