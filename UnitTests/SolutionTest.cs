using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using YsBuildRunner.Data;
using YsBuildRunner.Data.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    /// <summary>
    ///This is a test class for SolutionTest and is intended
    ///to contain all SolutionTest Unit Tests
    ///</summary>
	[TestClass]
	public class SolutionTest
	{
		#region Constants
		
		private const string XmlFileName = @"c:\work\Solution.xml";
		#endregion

		#region Properties

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get;
			set;
		}
		
		#endregion

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
		///A test for Solution serialization.
		///</summary>
		[TestMethod]
		public void SolutionSerializationTest()
		{
			var solution = DataGenerator.CreateSolution();
			Assert.IsNotNull(solution, "Failed creating Solution.");

			//// Create writer
			//var buffer = new StringBuilder();
			//var xmlWriter = XmlWriter.Create(buffer);

			//// Create serializer.
			//var serializer = new DataContractSerializer(typeof(TradeValidationRequest));

			//// Serialize.
			//serializer.WriteObject(xmlWriter, tradeValidationRequest);
			//xmlWriter.Flush();

			//return buffer.ToString();

			// Create writer.
			var streamWriter = new StreamWriter(XmlFileName);
			var xmlWriter = XmlWriter.Create(streamWriter);

			if (xmlWriter != null)
			{

				// Create serializer.
				var serializer = new DataContractSerializer(typeof (Solution));

				// Serialize.
				serializer.WriteObject(xmlWriter, solution);
				xmlWriter.Close();
				streamWriter.Close();

				// Create reader.
				var streamReader = new StreamReader(XmlFileName);
				var xmlReader = XmlReader.Create(streamReader);

				// Deserialize.
				var deserializedSolution = (Solution) serializer.ReadObject(xmlReader);
				xmlReader.Close();
				streamReader.Close();

				Assert.IsNotNull(deserializedSolution, "Failed deserializing.");

				Assert.AreEqual(solution.ToString(), deserializedSolution.ToString(), "Failed serialize/deserialize.");
			}
			else
			{
				Assert.Fail("Failed creation xmlWriter");
			}
		}

		[TestMethod]
		public void SolutionSaveRestoreTest()
		{
			var solution = DataGenerator.CreateSolution();
			Assert.IsNotNull(solution, "Failed creating Solution.");

			// Keep source ToString
			var src = solution.ToString();

			// Start edit
			solution.BeginEdit();

			solution.SolutionName = "edited";
			solution.Path = "edited";
			solution.Task = (Lists.CreateTasks()as List<string>)[1];
			solution.Configuration = (Lists.CreateConfigurations() as List<string>)[1];
			solution.Platform = (Lists.CreatePlatforms() as List<string>)[1];
			solution.Condition = (Lists.CreateConditions() as List<string>)[1];

			// Cancel edit.
			solution.CancelEdit();

			// Check.
			Assert.AreEqual(src, solution.ToString(), "Failed save/restore");

		}

		[TestMethod]
		public void SolutionConstructorTest()
		{
			const string solutionName = "abc";
			const string fileName = @"c:\aaa\bbb\ddd\" + solutionName + @".sln";

			var solution = new Solution(fileName);
			Assert.IsNotNull(solution, "(1) Failed creating Solution");

			Assert.AreEqual(solutionName, solution.SolutionName, "(2) Failed creating Solution");
		}
	}
}
