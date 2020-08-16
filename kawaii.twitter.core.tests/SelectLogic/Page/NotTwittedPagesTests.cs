using System;
using System.Collections.Generic;
using System.Text;
using kawaii.twitter.core.SelectLogic.Page;
using kawaii.twitter.core.SelectLogic.Randomize;
using kawaii.twitter.core.tests.SelectLogic.Stubs;
using kawaii.twitter.db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;


namespace kawaii.twitter.core.tests.SelectLogic.Page
{
	[TestClass]
	public class NotTwittedPagesTests
	{
		const int _TOP_QUERY_COUNT = 3;

		[TestMethod]
		[Description("Тест конструктора, аргумент pages == null")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Ctor_Argument_Pages_Null()
		{
			try
			{
				var pg = new NotTwittedPages(null, new RandomSelector(), _TOP_QUERY_COUNT);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "pages");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент randomSelector == null")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Ctor_Argument_RandomSelector_Null()
		{
			try
			{
				var pg = new NotTwittedPages(new PagesCollectionStub(), null, _TOP_QUERY_COUNT);
				Assert.Fail("Очікувалося ArgumentNullException");
			}
			catch (ArgumentNullException ex)
			{
				Assert.IsTrue(ex.ParamName == "randomSelector");
			}
		}

		[TestMethod]
		[Description("Тест конструктора, аргумент topQueryCount == 0")]
		[TestCategory("NotTwittedPages")]
		public void NotTwittedPages_Ctor_Argument_TopQueryCount_Zero()
		{
			try
			{
				var pg = new NotTwittedPages(new PagesCollectionStub(), new RandomSelector(), 0);
				Assert.Fail("Очікувалося ArgumentException");
			}
			catch (ArgumentException ex)
			{
				Assert.IsTrue(ex.ParamName == "topQueryCount");
			}
		}



	}
}
