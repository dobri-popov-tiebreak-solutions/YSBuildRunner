using System.Runtime.Serialization;
using System.Text;

namespace YsBuildRunner.Data
{
	/// <summary>
	/// Holds a window position. 
	/// </summary>
	[DataContract]
	internal class WindowPosition
	{
		#region Properties

		/// <summary>
		/// Gets or sets the window name.
		/// </summary>
		[DataMember]
		public string WindowName
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the window left corner position.
		/// </summary>
		[DataMember]
		public double Left
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the window top corner position.
		/// </summary>
		[DataMember]
		public double Top
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the window width.
		/// </summary>
		[DataMember]
		public double Width
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the window height.
		/// </summary>
		[DataMember]
		public double Height
		{
			get;
			set;
		}
		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the WindowPosition class.
		/// </summary>
		/// <param name="windowName"></param>
		public WindowPosition(string windowName)
		{
			WindowName = windowName;
		}

		#endregion
		
		#region Overrides

		/// <summary>
		/// Gets the text representation of the instance.
		/// </summary>
		/// <returns>The text representation of the instance.</returns>
		public override string ToString()
		{
			var buffer = new StringBuilder("WindowPosition:");

			buffer.AppendFormat(" Left={0}", Left);
			buffer.AppendFormat(", Top={0}", Top);
			buffer.AppendFormat(", Width={0}", Width);
			buffer.AppendFormat(", Height={0}", Height);

			return buffer.ToString();
		}
		#endregion
	}
}
