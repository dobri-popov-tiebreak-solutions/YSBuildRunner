using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Utils.Commands
{
	public interface IYSCommand : ICommand
	{
		/// <summary>
		/// Uccurs when the command is executing.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
		event Action OnExecute;

		/// <summary>
		/// Enables or disables the command.
		/// </summary>
		/// <param name="enabled">True, if to enable the command; false, if to disable the command.</param>
		void Enable(bool enabled);
	}
}
