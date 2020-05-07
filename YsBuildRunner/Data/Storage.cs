using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace YsBuildRunner.Data
{
	/// <summary>
	/// Contains the Build data.
	/// </summary>
	[DataContract]
	public class Storage : BaseDataObject
	{
		#region Fields

		/// <summary>
		/// Holds the builds list.
		/// </summary>
		private ObservableCollection<Build> builds_;

		/// <summary>
		/// Holds the builds list saved.
		/// </summary>
		private ObservableCollection<Build> buildsSaved_;

		/// <summary>
		/// Holds the version.
		/// </summary>
		private string version_;

		/// <summary>
		/// Holds the version saved.
		/// </summary>
		private string versionSaved_;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		[DataMember]
		public string Version
		{
			get
			{
				return version_;
			}
			set
			{
				version_ = value;
				SetPropertyChanged("Version");
			}
		}

		/// <summary>
		/// Gets the builds list.
		/// </summary>
		[DataMember]
		public ObservableCollection<Build> Builds
		{
			get
			{
				if (builds_ == null)
				{
					builds_ = new ObservableCollection<Build>();
				}
				return builds_;
			}
		}
		#endregion

		#region Overrides

		/// <summary>
		/// Gets the text representation of the instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var buffer = new StringBuilder("Storage:");

			buffer.AppendFormat(" Version: \"{0}\"", Version);

			buffer.Append(", Builds[");
			foreach (var build in Builds)
			{
				buffer.AppendFormat(", {0}", build.ToString());
			}
			buffer.Append("]");

			return buffer.ToString();
		}
		#endregion

		#region BaseDataObject Implementation

		/// <summary>
		/// Saves data in memory.
		/// </summary>
		protected override void Save()
		{
			versionSaved_ = Version;
			buildsSaved_ = new ObservableCollection<Build>(Builds.ToList());
		}

		/// <summary>
		/// Restores data from memory.
		/// </summary>
		protected override void Restore()
		{
			Version = versionSaved_;

			builds_ = new ObservableCollection<Build>(buildsSaved_.ToList());
			SetPropertyChanged("Builds");
		}
		#endregion

	}
}
