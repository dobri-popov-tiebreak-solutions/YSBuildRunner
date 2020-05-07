using System;
using YsBuildRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YsBuildRunner.Data;

namespace UnitTests
{
	/// <summary>
	///This is a test class for FileUtilTest and is intended
	///to contain all FileUtilTest Unit Tests
	///</summary>
	[TestClass]
	public class FileUtilTest
	{
		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get;
			set;
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for GetLogName
		///</summary>
		[TestMethod]
		public void GetLogNameTest()
		{
			var solution = new Solution(@"c:\Users\Yuriy\Documents\Stulsoft\YsBuildRunner\YsBuildRunner.sln");
			var fileName = FileUtil.GetLogName(solution);
			Assert.IsTrue(!String.IsNullOrEmpty(fileName));
		}
	}
}
