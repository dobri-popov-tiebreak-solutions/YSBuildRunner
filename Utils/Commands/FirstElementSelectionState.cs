using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Commands
{
	/// <summary>
	/// Defines the first element selection states.
	/// </summary>
	public enum FirstElementSelectionState
	{
		// <summary>
		/// All following states.
		/// </summary>
		All,

		/// <summary>
		/// First list element is selected
		/// </summary>
		Selected,

		/// <summary>
		/// First list element is unselected
		/// </summary>
		UnSelected
	}
}
