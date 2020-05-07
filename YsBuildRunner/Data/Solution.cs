using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using YsBuildRunner.Data.Enums;

namespace YsBuildRunner.Data
{
	/// <summary>
	/// Contains solution data.
	/// </summary>
	[DataContract]
	public class Solution : BaseDataObject
	{
		#region Fields

		/// <summary>
		/// Holds Solution Name.
		/// </summary>
		private string solutionName_;

		/// <summary>
		/// Holds Solution Name saved.
		/// </summary>
		private string solutionNameSaved_;

		/// <summary>
		/// Holds Solution path.
		/// </summary>
		private string path_;

		/// <summary>
		/// Holds Solution path saved.
		/// </summary>
		private string pathSaved_;

		/// <summary>
		/// Holds Task.
		/// </summary>
		private string task_;

		/// <summary>
		/// Holds Task saved.
		/// </summary>
		private string taskSaved_;

		/// <summary>
		/// Holds Configuration.
		/// </summary>
		private string configuration_;

		/// <summary>
		/// Holds Configuration saved.
		/// </summary>
		private string configurationSaved_;

		/// <summary>
		/// Holds Platform.
		/// </summary>
		private string platform_;

		/// <summary>
		/// Holds Platform saved.
		/// </summary>
		private string platformsaved_;

		/// <summary>
		/// Holds Condition.
		/// </summary>
		private string condition_;

		/// <summary>
		/// Holds Condition saved.
		/// </summary>
		private string conditionSaved_;

		/// <summary>
		/// Holds state.
		/// </summary>
		private State state_;

		/// <summary>
		/// True, if the solution is included into build.
		/// </summary>
		private bool include_;

		/// <summary>
		/// True, if the solution is included into build.
		/// </summary>
		private bool includeSaved_;

		/// <summary>
		/// Holds the state image.
		/// </summary>
		private string stateImage_;

		/// <summary>
		/// Holds selection state.
		/// </summary>
		private bool isSelected_;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the solution name.
		/// </summary>
		[DataMember]
		public string SolutionName
		{
			get
			{
				return solutionName_;
			}
			set
			{
				solutionName_ = value;
				SetPropertyChanged("SolutionName");
			}
		}

		/// <summary>
		/// Gets or sets the solution path.
		/// </summary>
		[DataMember]
		public string Path
		{
			get
			{
				return path_;
			}
			set
			{
				path_ = value;
				SetPropertyChanged("Path");
			}
		}

		/// <summary>
		/// Gets or sets the solution task.
		/// </summary>
		[DataMember]
		public string Task
		{
			get
			{
				return task_;
			}
			set
			{
				task_ = value;
				SetPropertyChanged("Task");
			}
		}

		/// <summary>
		/// Gets or sets the configuration.
		/// </summary>
		[DataMember]
		public string Configuration
		{
			get
			{
				return configuration_;
			}
			set
			{
				configuration_ = value;
				SetPropertyChanged("Configuration");
			}
		}

		/// <summary>
		/// Gets or sets the platform.
		/// </summary>
		[DataMember]
		public string Platform
		{
			get
			{
				return platform_;
			}
			set
			{
				platform_ = value;
				SetPropertyChanged("Platform");
			}
		}

		/// <summary>
		/// Gets or sets the condition.
		/// </summary>
		[DataMember]
		public string Condition
		{
			get
			{
				return condition_;
			}
			set
			{
				condition_ = value;
				SetPropertyChanged("Condition");
			}
		}

		/// <summary>
		/// Gets or sets the solution state.
		/// </summary>
		public State State
		{
			get
			{
				return state_;
			}
			set
			{
				state_ = value;
				SetPropertyChanged("State");

				switch (value)
				{
					case State.Failed:
						StateImage = "/YsBuildRunner;component/Images/Error.png";
						break;
					case State.Succeeded:
						StateImage = "/YsBuildRunner;component/Images/app.png";
						break;
					default:
						StateImage = "/YsBuildRunner;component/Images/unknown.png";
						break;
				}
			}
		}

		/// <summary>
		/// Gets or sets the "included into build" value.
		/// </summary>
		[DataMember]
		public bool Include
		{
			get
			{
				return include_;
			}

			set
			{
				include_ = value;
				SetPropertyChanged("Include");
			}
		}

		/// <summary>
		/// Gets the log file full path.
		/// </summary>
		public string LogFileName
		{
			get
			{
				return FileUtil.GetLogName(this);
			}
		}

		/// <summary>
		/// Gets or sets the state image.
		/// </summary>
		public string StateImage
		{
			get
			{
				if (stateImage_ == null)
				{
					stateImage_ = "/YsBuildRunner;component/Images/unknown.png";
				}
				return stateImage_;
			}
			set
			{
				stateImage_ = value;
				SetPropertyChanged("StateImage");
			}
		}

		/// <summary>
		/// Returns true, if it is solution; otherwise - false.
		/// </summary>
		public bool IsSolution
		{
			get
			{
				return String.Equals(System.IO.Path.GetExtension(Path), ".sln", StringComparison.CurrentCultureIgnoreCase);
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether a Project is selected.
		/// </summary>
		/// <value>
		/// true if the item is selected; otherwise, false. The default is false.
		/// </value>
		public bool IsSelected
		{
			get
			{
				return isSelected_;
			}
			set
			{
				isSelected_ = value;
				SetPropertyChanged("IsSelected");
			}
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Solution class.
		/// </summary>
		public Solution()
		{
		}

		/// <summary>
		/// Initializes a new instance of the Solution class.
		/// </summary>
		/// <remarks>
		/// The constructors extracts the solution name from the file name.
		/// The constructor initializes properties from the lists - takes first element.
		/// </remarks>
		/// <param name="fileName">Full path to a solution.</param>
		/// <exception cref="ArgumentNullException">
		/// The parameter fileName is null or empty.
		/// </exception>
		public Solution(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentNullException("fileName");
			}

			Path = fileName;

			if (IsSolution)
			{
				SolutionName = System.IO.Path.GetFileNameWithoutExtension(fileName);
				Task = Lists.CreateTasks().First();
				Configuration = Lists.CreateConfigurations().First();
				Platform = Lists.CreatePlatforms().First();
			}
			else
			{
				SolutionName = System.IO.Path.GetFileName(fileName);
				Task = Lists.CreateTasks().Last();
				Configuration = Lists.CreateConfigurations().Last();
				Platform = Lists.CreatePlatforms().Last();
			}

			
			Condition = Lists.CreateConditions().First();
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Gets the text representation of the instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var buffer = new StringBuilder("Solution:");

			buffer.AppendFormat(", SolutionName: \"{0}\"", SolutionName);
			buffer.AppendFormat(", Path: \"{0}\"", Path);
			buffer.AppendFormat(", Task: \"{0}\"", Task);
			buffer.AppendFormat(", Configuration: \"{0}\"", Configuration);
			buffer.AppendFormat(", Platform: \"{0}\"", Platform);
			buffer.AppendFormat(", Condition: \"{0}\"", Condition);
			buffer.AppendFormat(", State: \"{0}\"", State);
			buffer.AppendFormat(", Include: \"{0}\"", Include);
			buffer.AppendFormat(", LogFileName: \"{0}\"", LogFileName);
			buffer.AppendFormat(", StateImage: \"{0}\"", StateImage);

			return buffer.ToString();
		}
		#endregion

		#region BaseDataObject Implementation

		/// <summary>
		/// Saves data in memory.
		/// </summary>
		protected override void Save()
		{
			solutionNameSaved_ = SolutionName;
			pathSaved_ = Path;
			taskSaved_ = Task;
			configurationSaved_ = Configuration;
			platformsaved_ = Platform;
			conditionSaved_ = Condition;
			includeSaved_ = Include;
		}

		/// <summary>
		/// Restores data from memory.
		/// </summary>
		protected override void Restore()
		{
			SolutionName = solutionNameSaved_;
			Path = pathSaved_;
			Task = taskSaved_;
			Configuration = configurationSaved_;
			Platform = platformsaved_;
			Condition = conditionSaved_;
			Include = includeSaved_;
		}

		#endregion
	}
}
