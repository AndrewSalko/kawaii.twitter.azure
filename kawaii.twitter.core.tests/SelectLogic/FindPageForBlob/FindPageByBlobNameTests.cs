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

		[TestMethod]
		[Description("Тест поиска страницы по блоб-имени и проверка найденного результата")]
		[TestCategory("FindPageByBlobName")]
		public void FindPageByBlobName_Normal_Find()
		{
			try
			{
				var page2 = new SitePage()
				{
					Blocked = false,
					SpecialDay = null,
					Title = "Shuumatsu no Izetta",
					LastModified = new DateTime(2020, 04, 25),
					TweetDate = null,
					URL = "https://kawaii-mobile.com/2017/01/shuumatsu-no-izetta/"
				};

				List<SitePage> pagesForTest = new List<SitePage>()
				{
					new SitePage(){ Blocked=false, SpecialDay=null, Title="Princess connect! Re:Dive", LastModified=new DateTime(2020,04,26), TweetDate=null, URL="https://kawaii-mobile.com/2020/08/princess-connect-redive/" },
					page2,
					new SitePage(){ Blocked=false, SpecialDay=null, Title="Gleipnir", LastModified=new DateTime(2020,04,23), TweetDate=null, URL="https://kawaii-mobile.com/2020/07/gleipnir/" },
				};

				QueryableStub<SitePage> queryableStub = new QueryableStub<SitePage>
				{
					ResultData = pagesForTest
				};

				string blobName = "shuumatsu-no-izetta:image1.gif";

				var fnd = new FindPageByBlobName(queryableStub, new BlobNameToURLPart());
				var result = fnd.Find(blobName).Result;

				Assert.AreSame(result, page2);
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "blobNameToURLPart");
			}
		}


	}
}
