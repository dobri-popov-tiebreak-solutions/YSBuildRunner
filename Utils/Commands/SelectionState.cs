using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Commands
{
	/// <summary>
	/// Defines a selection states.
	/// </summary>
	public enum SelectionState
	{
		/// <summary>
		/// All following states.
		/// </summary>
		All,

		/// <summary>
		/// No one item in the list is selected.
		/// </summary>
		NoOneItemSelected,

		/// <summary>
		/// Only one list item is selected.
		/// </summary>
		OneItemSelected,

		/// <summary>
		/// More than one list item are selected.
		/// </summary>
		ManyItemsSelected
	}
}
