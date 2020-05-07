using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Utils.Commands
{
	/// <summary>
	/// General command
	/// </summary>
	public class Command : IYSCommand
	{
		/// <summary>
		/// Uccurs when the command is executing.
		/// </summary>
        public event Action OnExecute;

		#region Fields

		private bool enabled_ = true;
		#endregion

		#region Methods

		/// <summary>
		/// Enables or disables the command.
		/// </summary>
		/// <param name="enabled">True, if to enable the command; false, if to disable the command.</param>
		public void Enable(bool enabled)
		{
			enabled_ = enabled;

			var canExecuteChanged = CanExecuteChanged;
			if (canExecuteChanged != null)
			{
				canExecuteChanged(this, EventArgs.Empty);
			}
		}
		#endregion

		#region ICommand Members

		/// <summary>
		/// Defines the method that determines whether the command can execute in its
		/// current state.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command. If the command does not require data to be passed,
		/// this object can be set to null.
		/// </param>
		/// <returns>The true if this command can be executed; otherwise, false.</returns>
		public bool CanExecute(object parameter)
		{
			return enabled_;
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command. If the command does not require data to be passed,
		/// this object can be set to null.
		/// </param>
		public void Execute(object parameter)
		{
			if (!CanExecute(parameter))
			{
				return;
			}

			var onExecute = OnExecute;

			if (onExecute != null)
			{
				onExecute();
			}
		}
		#endregion
	}
}
