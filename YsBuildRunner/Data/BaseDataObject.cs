using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace YsBuildRunner.Data
{
	/// <summary>
	/// Base class for data objects.
	/// </summary>
	[DataContract]
	public abstract class BaseDataObject : INotifyPropertyChanged, IEditableObject
	{

		#region Fields

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Event to process property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets or sets the editing mode.
		/// </summary>
		protected bool EditingMode
		{
			get;
			set;
		}
		#endregion Properties

		#region Methods

		/// <summary>
		/// Initializes a new instance of the BaseDataObject class.
		/// </summary>
		protected BaseDataObject()
		{
			EditingMode = false;
		}

		/// <summary>
		/// Arise property changed event.
		/// </summary>
		/// <param name="propertyName">The property name</param>
		protected void SetPropertyChanged(String propertyName)
		{
			if (null != PropertyChanged)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Saves data members.
		/// </summary>
		protected abstract void Save();

		/// <summary>
		/// Restores data members.
		/// </summary>
		protected abstract void Restore();

		#region IEditableObject Members

		/// <summary>
		/// Marks editing start.
		/// </summary>
		public void BeginEdit()
		{
			if (!EditingMode)
			{
				Save();
				EditingMode = true;
			}

		}

		/// <summary>
		/// Cancels editing.
		/// </summary>
		public void CancelEdit()
		{
			if (EditingMode)
			{
				Restore();
				EditingMode = false;
			}
		}

		/// <summary>
		/// Marks editing end.
		/// </summary>
		public void EndEdit()
		{
			EditingMode = false;
		}
		#endregion IEditableObject Members


		#endregion Methods
	}
}
