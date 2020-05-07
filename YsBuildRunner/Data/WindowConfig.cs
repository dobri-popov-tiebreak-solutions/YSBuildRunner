using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;

namespace YsBuildRunner.Data
{
	/// <summary>
	/// Holds windows configurations.
	/// </summary>
	[DataContract]
	internal class WindowConfig
	{
		#region Fields

		[DataMember]
		private Dictionary<string, WindowPosition> windows_;
		#endregion

		#region Properties
		
		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		[DataMember]
		public string Version
		{
			get;
			set;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Gets the WindowPosition instance for specified window name.
		/// </summary>
		/// <param name="windowName">Indicates the window name.</param>
		/// <returns>The WindowPosition instance for specified window name.</returns>
		private WindowPosition GetWindowPosition(string windowName)
		{
			WindowPosition windowPosition = null;

			lock (this)
			{
				if (windows_ != null)
				{
					windows_.TryGetValue(windowName, out windowPosition);
				}
			}

			return windowPosition;
		}

		/// <summary>
		/// Adds new WindowPosition instance.
		/// </summary>
		/// <remarks>
		/// If the WindowPosition with same window name exists already, then the method
		/// replaces old value with new one.
		/// </remarks>
		/// <param name="windowPosition">the WindowPosition instance to add.</param>
		private void AddWindowPosition(WindowPosition windowPosition)
		{
			lock (this)
			{
				if (windows_ == null)
				{
					windows_ = new Dictionary<string, WindowPosition>
				           	{
								{windowPosition.WindowName, windowPosition}
				           	};
				}
				else
				{
					var windowPositionExisting = GetWindowPosition(windowPosition.WindowName);

					if (windowPositionExisting != null)
					{
						windows_.Remove(windowPosition.WindowName);
					}
					windows_.Add(windowPosition.WindowName, windowPosition);
				}
			}
		}

		/// <summary>
		/// Saves the window position parameters.
		/// </summary>
		/// <param name="window">The window, which position to save</param>
		/// <exception cref="ArgumentNullException">
		/// The parameter window is null.
		/// </exception>
		public void SaveWindowPosition(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}

			lock (this)
			{
				var windowPosition = GetWindowPosition(window.Title);

				if (windowPosition == null)
				{
					windowPosition = new WindowPosition(window.Title);

					AddWindowPosition(windowPosition);
				}

				windowPosition.WindowName = window.Title;
				windowPosition.Left = window.Left;
				windowPosition.Top = window.Top;
				windowPosition.Width = window.Width;
				windowPosition.Height = window.Height;
			}
		}

		/// <summary>
		/// Sets the window position.
		/// </summary>
		/// <param name="window">The window for wich to set window position.</param>
		/// <exception cref="ArgumentNullException">
		/// The parameter window is null.
		/// </exception>
		public void SetWindowPosition(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}

			var windowPosition = GetWindowPosition(window.Title);

			if (windowPosition != null)
			{
				window.Left = windowPosition.Left;
				window.Top = windowPosition.Top;
				window.Width = windowPosition.Width;
				window.Height = windowPosition.Height;
			}
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Gets the text representation of the instance.
		/// </summary>
		/// <returns>The text representation of the instance.</returns>
		public override string ToString()
		{
			var buffer = new StringBuilder("WindowConfig:");

			buffer.Append(" windows[");
			if (windows_ != null)
			{
				foreach (var window in windows_)
				{
					buffer.AppendFormat(", {0}", window.ToString());
				}
			}
			buffer.Append("]");

			return buffer.ToString();
		}
		#endregion
	}
}
