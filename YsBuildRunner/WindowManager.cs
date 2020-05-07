using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using YsBuildRunner.Data;

namespace YsBuildRunner
{
	/// <summary>
	/// Window Configuration Manager.
	/// </summary>
	internal class WindowManager
	{
		#region Fields

		#endregion

		#region Properties

		/// <summary>
		/// Gets the WindowConfig.
		/// </summary>
		public WindowConfig WindowConfig
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the instance of the WindowManager class.
		/// </summary>
		public static WindowManager Instance
		{
			get;
			private set;
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the static data members.
		/// </summary>
		static WindowManager()
		{
			Instance = new WindowManager();
		}

		/// <summary>
		/// Initializes a new instance of the WindowManager class.
		/// </summary>
		private WindowManager()
		{
			Load();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Loads the window configuration from the file.
		/// </summary>
		private void Load()
		{
			// Check, the storage file exists already.
			bool fileDoesnotExist;
			var fullPath = FileUtil.GetWindowConfigFileName(out fileDoesnotExist);

			if (fileDoesnotExist)
			{
				WindowConfig = new WindowConfig
				{
					Version = "1.0"
				};
				Save();
			}

			// Create serializer.
			var serializer = new DataContractSerializer(typeof(WindowConfig));

			// Create reader.
			using (var streamReader = new StreamReader(fullPath))
			{
				using (var xmlReader = XmlReader.Create(streamReader))
				{
					// Deserialize.
					WindowConfig = (WindowConfig)serializer.ReadObject(xmlReader);
				}
			}
		}

		/// <summary>
		/// Saves the window configuration into file.
		/// </summary>
		public void Save()
		{
			// Create serializer.
			var serializer = new DataContractSerializer(typeof(WindowConfig));

			// Create reader.
			bool fileDoesnotExist;
			using (var streamWriter = new StreamWriter(FileUtil.GetWindowConfigFileName(out fileDoesnotExist)))
			{
				using (var xmlWriter = XmlWriter.Create(streamWriter))
				{
					if (xmlWriter != null)
					{
						// Serialize.
						serializer.WriteObject(xmlWriter, WindowConfig);
					}
				}
				streamWriter.Close();
			}
		}
		#endregion
	}
}
