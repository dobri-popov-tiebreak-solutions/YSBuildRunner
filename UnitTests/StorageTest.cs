using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using YsBuildRunner.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    
    
    /// <summary>
    ///This is a test class for StorageTest and is intended
    ///to contain all StorageTest Unit Tests
    ///</summary>
	[TestClass]
	public class StorageTest
	{
		#region Constants

		private const string XmlFileName = @"c:\work\Storage.xml";
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
		///A test for Storage serialization
		///</summary>
		[TestMethod]
		public void StorageConstructorTest()
		{
			// Create storage.
			var storage = DataGenerator.CreateStorage(10, 25);
			Assert.IsNotNull(storage, "Failed creating Storage.");

			// Create writer.
			var streamWriter = new StreamWriter(XmlFileName);
			var xmlWriter = XmlWriter.Create(streamWriter);

			if (xmlWriter != null)
			{
				// Create serializer.
				var serializer = new DataContractSerializer(typeof(Storage));

				// Serialize.
				serializer.WriteObject(xmlWriter, storage);
				xmlWriter.Close();
				streamWriter.Close();

				// Create reader.
				var streamReader = new StreamReader(XmlFileName);
				var xmlReader = XmlReader.Create(streamReader);

				// Deserialize.
				var deserializedStorage = (Storage)serializer.ReadObject(xmlReader);
				xmlReader.Close();
				streamReader.Close();

				Assert.IsNotNull(deserializedStorage, "Failed deserializing.");

				Assert.AreEqual(storage.ToString(), deserializedStorage.ToString(), "Failed serialize/deserialize.");
			}
			else
			{
				Assert.Fail("Failed creation xmlWriter");
			}
		}

		/// <summary>
		/// Tests save/restore
		/// </summary>[TestMethod]
		public void StorageSaveRestoreTest()
		{
			// Create storage.
			var storage = DataGenerator.CreateStorage(10, 25);
			Assert.IsNotNull(storage, "Failed creating Storage.");

			// Keep sourcestorage ToString
			var src = storage.ToString();

			// Start edit.
			storage.BeginEdit();

			// Edit
			storage.Version = "edited";
			storage.Builds.RemoveAt(1);

			// Cancel edit.
			storage.CancelEdit();

			// Check.
			Assert.AreEqual(src, storage.ToString(), "Failed save/edit");
		}
	}
}
