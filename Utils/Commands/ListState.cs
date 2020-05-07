using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Commands
{
	/// <summary>
	/// Defines a list states.
	/// </summary>
	public enum ListState
	{
		/// <summary>
		/// Both states: empty and no empty.
		/// </summary>
		All,

		/// <summary>
		/// Empty list.
		/// </summary>
		Empty,

		/// <summary>
		/// No empty list.
		/// </summary>
		NoEmpty
	}
}
