using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YsBuildRunner.Data
{
	/// <summary>
	/// Inidicates enbale mode.
	/// </summary>
	public class Mode : BaseDataObject
	{
		#region Fields

		/// <summary>
		/// Holds the mode.
		/// </summary>
		private bool isEnabled_;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the mode.
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				return isEnabled_;
			}

			set
			{
				isEnabled_ = value;
				SetPropertyChanged("IsEnabled");
			}
		}
		#endregion

		#region BaseDataObject Implementation

		/// <summary>
		/// Saves the data in memory.
		/// </summary>
		protected override void Save()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Restores the data from the memory.
		/// </summary>
		protected override void Restore()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
