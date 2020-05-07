using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Commands
{
	/// <summary>
	/// Defines the first element selection states.
	/// </summary>
	public enum LastElementSelectionState
	{
		// <summary>
		/// All following states.
		/// </summary>
		All,

		/// <summary>
		/// Last list element is selected
		/// </summary>
		Selected,

		/// <summary>
		/// Last list element is unselected
		/// </summary>
		UnSelected
	}
}
