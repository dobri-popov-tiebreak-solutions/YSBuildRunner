using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using YsBuildRunner.Data.Enums;

namespace YsBuildRunner.Data
{
	/// <summary>
	/// Contains the Build data.
	/// </summary>
	[DataContract]
	public class Build : BaseDataObject
	{
		#region Fields

		/// <summary>
		/// Holds the solutions list.
		/// </summary>
		private ObservableCollection<Solution> solutions_;

		/// <summary>
		/// Holds the solutions list saved.
		/// </summary>
		private ObservableCollection<Solution> solutionsSaved_;

		/// <summary>
		/// Holds the buildName.
		/// </summary>
		private string buildName_;

		/// <summary>
		/// Holds the buildName saved.
		/// </summary>
		private string buildNameSaved_;

		/// <summary>
		/// Holds state.
		/// </summary>
		private State state_;

		/// <summary>
		/// Total amount of the processed solutions.
		/// </summary>
		private int processed_;

		/// <summary>
		/// Total amount of the succeeded solutions.
		/// </summary>
		private int succeeded_;

		/// <summary>
		/// Total amount of the failed solutions.
		/// </summary>
		private int failed_;

		/// <summary>
		/// Total amount of the skipped solutions.
		/// </summary>
		private int skipped_;

		#endregion

		#region Properties

		/// <summary>
		/// Gts or sets the buildName.
		/// </summary>
		[DataMember]
		public string BuildName
		{
			get
			{
				return buildName_;
			}
			set
			{
				buildName_ = value;
				SetPropertyChanged("BuildName");
			}
		}

		/// <summary>
		/// Gets the solutions list.
		/// </summary>
		[DataMember]
		public ObservableCollection<Solution> Solutions
		{
			get
			{
				if (solutions_ == null)
				{
					solutions_ = new ObservableCollection<Solution>();
				}
				return solutions_;
			}
		}

		/// <summary>
		/// Gets or sets the build state.
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
			}
		}

		/// <summary>
		/// Gets or sets the processed amount.
		/// </summary>
		public int Processed
		{
			get
			{
				return processed_;
			}
			set
			{
				processed_ = value;
				SetPropertyChanged("Processed");
			}
		}

		/// <summary>
		/// Gets or sets the failed amount.
		/// </summary>
		public int Failed
		{
			get
			{
				return failed_;
			}
			set
			{
				failed_ = value;
				SetPropertyChanged("Failed");
			}
		}

		/// <summary>
		/// Gets or sets the succeeded amount.
		/// </summary>
		public int Succeeded
		{
			get
			{
				return succeeded_;
			}
			set
			{
				succeeded_ = value;
				SetPropertyChanged("Succeeded");
			}
		}

		/// <summary>
		/// Gets or sets the skipped amount.
		/// </summary>
		public int Skipped
		{
			get
			{
				return skipped_;
			}
			set
			{
				skipped_ = value;
				SetPropertyChanged("Skipped");
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
			get;
			set;
		}
		#endregion

		#region Overrides

		/// <summary>
		/// Gets the text representation of the instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var buffer = new StringBuilder("Build:");

			buffer.AppendFormat(" BuildName: \"{0}\"", BuildName);

			buffer.Append(", Solutions[");
			foreach (var solution in Solutions)
			{
				buffer.AppendFormat(", {0}", solution.ToString());
			}
			buffer.Append("]");

			buffer.AppendFormat(", State: \"{0}\"", State);

			return buffer.ToString();
		}
		#endregion

		#region BaseDataObject Implementation

		/// <summary>
		/// Saves data in memory.
		/// </summary>
		protected override void Save()
		{
			buildNameSaved_ = BuildName;
			solutionsSaved_ = new ObservableCollection<Solution>(Solutions.ToList());

			// Save old values for each solution property.
			foreach (var solution in solutionsSaved_)
			{
				solution.BeginEdit();
			}
		}

		/// <summary>
		/// Restores data from memory.
		/// </summary>
		protected override void Restore()
		{
			BuildName = buildNameSaved_;

			// restor old values for each solution property.
			foreach (var solution in solutionsSaved_)
			{
				solution.CancelEdit();
			}

			solutions_ = new ObservableCollection<Solution>(solutionsSaved_.ToList());

			SetPropertyChanged("Solutions");
		}
		#endregion

	}
}
