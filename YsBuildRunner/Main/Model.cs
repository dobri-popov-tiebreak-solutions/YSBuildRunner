using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Commands;
using YsBuildRunner.Data;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using YsBuildRunner.Build;
using YsBuildRunner.Properties;
using System.Windows;

namespace YsBuildRunner.Main
{
	/// <summary>
	/// View model for MainWindow
	/// </summary>
	internal class Model
	{
		#region Fields

		private readonly IYSCommand commandAdd_ = new Command();
		private readonly IYSCommand commandEdit_ = new Command();
		private readonly IYSCommand commandDelete_ = new Command();
		private readonly IYSCommand commandExit_ = new Command();
		private readonly IYSCommand commandConfig_ = new Command();

		private readonly MainWindow window_;
		private readonly EnablementManager enablementManager_ = new EnablementManager();

		private YsBuildRunner.Data.Build selectedBuild_;
		#endregion

		#region Properties

		/// <summary>
		/// Gets the CommandAdd
		/// </summary>
		public IYSCommand CommandAdd
		{
			get
			{
				return commandAdd_;
			}
		}

		/// <summary>
		/// Gets the CommandEdit
		/// </summary>
		public IYSCommand CommandEdit
		{
			get
			{
				return commandEdit_;
			}
		}

		/// <summary>
		/// Gets the CommandDelete
		/// </summary>
		public IYSCommand CommandDelete
		{
			get
			{
				return commandDelete_;
			}
		}

		/// <summary>
		/// Gets the CommandExit
		/// </summary>
		public IYSCommand CommandExit
		{
			get
			{
				return commandExit_;
			}
		}

		/// <summary>
		/// Gets the command config.
		/// </summary>
		public IYSCommand CommandConfig
		{
			get
			{
				return commandConfig_;
			}
		}

		/// <summary>
		/// Gets the value that inidicates whether the window may be closed.
		/// </summary>
		public bool CanClose
		{
			get
			{
				return enablementManager_.ExecutionState != ExecutionState.Running;
			}
		}

		/// <summary>
		/// Gets the Storage.
		/// </summary>
		public Storage Storage
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets value, which indicates if data was saved.
		/// </summary>
		private bool IsSaved
		{
			get;
			set;
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Model class.
		/// </summary>
		/// <param name="window">MainWindow's reference.</param>
		public Model(MainWindow window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}

			window_ = window;

			Load();
			IsSaved = false;

			window_.listBoxBuilds.SelectionChanged += ListBoxBuildsSelectionChanged;

			CommandAdd.OnExecute += OnCommandAdd;
			CommandEdit.OnExecute += OnCommandEdit;
			CommandDelete.OnExecute += OnCommandDelete;
			CommandExit.OnExecute += OnCommandExit;
			CommandConfig.OnExecute += OnCommandConfig;

			enablementManager_.AddCommand(CommandAdd, ExecutionState.NoRunning,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null);
	
			enablementManager_.AddCommand(CommandEdit, ExecutionState.NoRunning,
				new ListState[] { ListState.NoEmpty }, new ListState[] { ListState.Empty },
				new SelectionState[] { SelectionState.OneItemSelected }, new SelectionState[] { SelectionState.ManyItemsSelected, SelectionState.NoOneItemSelected });

			enablementManager_.AddCommand(CommandDelete, ExecutionState.NoRunning,
				new ListState[] { ListState.NoEmpty }, new ListState[] { ListState.Empty },
				new SelectionState[] { SelectionState.OneItemSelected }, new SelectionState[] { SelectionState.ManyItemsSelected, SelectionState.NoOneItemSelected });

			enablementManager_.AddCommand(CommandExit, ExecutionState.NoRunning,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null);

			enablementManager_.AddCommand(CommandConfig, ExecutionState.NoRunning,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null);

			enablementManager_.ListState = ListState.Empty;
			enablementManager_.SelectionState = SelectionState.NoOneItemSelected;
			enablementManager_.ExecutionState = ExecutionState.NoRunning;
		}
		#endregion

		#region Methods

		private void OnCommandAdd()
		{
			var newBuild = new YsBuildRunner.Data.Build();

			var dlg = new YsBuildRunner.Build.BuildWindow(newBuild)
			{
				Owner = window_
			};

			var result = dlg.ShowDialog();
			if (result.HasValue && result.Value)
			{
				Storage.Builds.Add(newBuild);
				Save();
			}
		}

		private void OnCommandEdit()
		{
			if (selectedBuild_ == null)
			{
				return;
			}

			(new YsBuildRunner.Build.BuildWindow(selectedBuild_)
			{
				Owner = window_
			}).ShowDialog();
		}

		private void OnCommandDelete()
		{
			if (selectedBuild_ == null)
			{
				return;
			}

			if (MessageBox.Show(string.Format("Are you sure delete {0}?", selectedBuild_.BuildName),
				window_.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
			{
				Storage.Builds.Remove(selectedBuild_);
			}
		}

		private void OnCommandExit()
		{
			Save();
			window_.Close();
		}

		private void OnCommandConfig()
		{
			var pathToMSBuild = Settings.Default.BuildCommand;
			var dlg = new ConfigWindow(pathToMSBuild);

			var result = dlg.ShowDialog();

			if (result.HasValue && result.Value)
			{
				Settings.Default.BuildCommand = dlg.PathToMSBuild;
				Settings.Default.Save();
				Settings.Default.Reload();
			}
		}

		private void ListBoxBuildsSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			selectedBuild_ = e.AddedItems.Count == 1 ? e.AddedItems[0] as YsBuildRunner.Data.Build : null;

			enablementManager_.ListState = Storage.Builds.Count == 0 ? ListState.Empty : ListState.NoEmpty;
			var selectedItemsCount = Storage.Builds.Where(p => p.IsSelected).Count();

			if (selectedItemsCount == 0)
			{
				enablementManager_.SelectionState = SelectionState.NoOneItemSelected;
			}
			else if (selectedItemsCount == 1)
			{
				enablementManager_.SelectionState = SelectionState.OneItemSelected;
			}
			else
			{
				enablementManager_.SelectionState = SelectionState.ManyItemsSelected;
			}
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		private void Load()
		{
			// Check, the storage file exists already.
			bool fileDoesnotExist;
			var fullPath = FileUtil.GetStorageFileName(out fileDoesnotExist);

			if (fileDoesnotExist)
			{
				Storage = new Storage
				{
					Version = "1.0"
				};
				Save();
			}

			// Create serializer.
			var serializer = new DataContractSerializer(typeof(Storage));

			// Create reader.
			using (var streamReader = new StreamReader(fullPath))
			{
				using (var xmlReader = XmlReader.Create(streamReader))
				{

					// Deserialize.
					Storage = (Storage)serializer.ReadObject(xmlReader);
				}
			}
		}

		/// <summary>
		/// Saves the data.
		/// </summary>
		public void Save()
		{
			if (!IsSaved)
			{
				// Create serializer.
				var serializer = new DataContractSerializer(typeof(Storage));

				// Create reader.
				bool fileDoesnotExist;
				using (var streamWriter = new StreamWriter(FileUtil.GetStorageFileName(out fileDoesnotExist)))
				{
					using (var xmlWriter = XmlWriter.Create(streamWriter))
					{
						if (xmlWriter != null)
						{
							// Serialize.
							serializer.WriteObject(xmlWriter, Storage);
						}
					}
				}

				IsSaved = true;
			}
		}
		#endregion
	}
}
