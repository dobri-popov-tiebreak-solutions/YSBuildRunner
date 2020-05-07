using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using YsBuildRunner.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    /// <summary>
    ///This is a test class for BuildTest and is intended
    ///to contain all BuildTest Unit Tests
    ///</summary>
	[TestClass]
	public class BuildTest
	{
		#region Constants

		private const string XmlFileName = @"c:\work\Build.xml";
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
		///A test for Build serialization
		///</summary>
		[TestMethod]
		public void BuildSerializationTest()
		{
			var build = DataGenerator.CreateBuild(3);
			Assert.IsNotNull(build, "Failed creating Build.");

			// Create writer.
			var streamWriter = new StreamWriter(XmlFileName);
			var xmlWriter = XmlWriter.Create(streamWriter);
            
			if (xmlWriter != null)
			{
			// Create serializer.
			var serializer = new DataContractSerializer(typeof(Build));

			// Serialize.
			serializer.WriteObject(xmlWriter, build);
			xmlWriter.Close();
			streamWriter.Close();

			// Create reader.
			var streamReader = new StreamReader(XmlFileName);
			var xmlReader = XmlReader.Create(streamReader);

			// Deserialize.
			var deserializedBuild = (Build)serializer.ReadObject(xmlReader);
			xmlReader.Close();
			streamReader.Close();

			Assert.IsNotNull(deserializedBuild, "Failed deserializing.");

			Assert.AreEqual(build.ToString(), deserializedBuild.ToString(), "Failed serialize/deserialize.");
			}
			else
			{
				Assert.Fail("Failed creation xmlWriter");
			}
		}

		/// <summary>
		/// Tests save/restore
		/// </summary>
		[TestMethod]
		public void BuildSaveRestoreTest()
		{
			var build = DataGenerator.CreateBuild(3);
			Assert.IsNotNull(build, "Failed creating Build.");

			// Save ToString text
			var src = build.ToString();

			// Start edit.
			build.BeginEdit();
			
			// Edit
			build.BuildName = "edited";
			build.Solutions.RemoveAt(1);

			// Cancel edit.
			build.CancelEdit();

			// Check.
			Assert.AreEqual(src, build.ToString(), "Failed save/edit");
		}
	}
}
