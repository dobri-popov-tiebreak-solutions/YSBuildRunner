using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Commands
{
	/// <summary>
	/// Defines an execution states.
	/// </summary>
	public enum ExecutionState
	{
		/// <summary>
		/// Both states: NoRunning and Running.
		/// </summary>
		All,

		/// <summary>
		/// No running state.
		/// </summary>
		NoRunning,

		/// <summary>
		/// Running state.
		/// </summary>
		Running,
	}
}
