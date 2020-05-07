using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Commands
{
	/// <summary>
	/// Manager that allows enable or disable a commands.
	/// </summary>
	public class EnablementManager
	{
		#region Fields

		private ExecutionState executionState_ = ExecutionState.NoRunning;
		private ListState listState_ = ListState.Empty;
		private SelectionState selectionState_ = SelectionState.NoOneItemSelected;
		private FirstElementSelectionState firstElementSelectionState_;
		private LastElementSelectionState lastElementSelectionState_;
		protected List<CommandDescription> commandDescriptions_ = new List<CommandDescription>();
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the execution state.
		/// </summary>
		public ExecutionState ExecutionState
		{
			get
			{
				return executionState_;
			}
			set
			{
				executionState_ = value;
				OnStateChanged();
			}
		}

		/// <summary>
		/// Gets or sets the list state.
		/// </summary>
		public ListState ListState
		{
			get
			{
				return listState_;
			}
			set
			{
				listState_ = value;
				OnStateChanged();
			}
		}

		/// <summary>
		/// Sets the selection state.
		/// </summary>
		public SelectionState SelectionState
		{
			get
			{
				return selectionState_;
			}
			set
			{
				selectionState_ = value;
				OnStateChanged();
			}
		}

		/// <summary>
		/// Gets or sets the value that indicates the first element selection state.
		/// </summary>
		public FirstElementSelectionState FirstElementSelectionState
		{
			get
			{
				return firstElementSelectionState_;
			}
			set
			{
				firstElementSelectionState_ = value;
				OnStateChanged();
			}
		}

		/// <summary>
		/// Gets or sets the value that indicates the first element selection state.
		/// </summary>
		public LastElementSelectionState LastElementSelectionState
		{
			get
			{
				return lastElementSelectionState_;
			}
			set
			{
				lastElementSelectionState_ = value;
				OnStateChanged();
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Adds new command to enablement management.
		/// </summary>
		/// <param name="command">Specifies the command.</param>
		/// <param name="executionState">Specifies the execution state that enables the command.</param>
		/// <param name="enableListStates">Specifies the list states that enable the command. If at least one state coincide with a list state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableListStates">Specifies the list states that disable the command. If at least one state coincide with a list state, then the command is disabled. May be null or empty.</param>
		/// <param name="enableSelectionStates">Specifies the selection states that enable the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableSelectionStates">Specifies the selection states that disable the command. If at least one state coincide with a selection state, then the command is disabled. May be null or empty.</param>
		public void AddCommand(IYSCommand command, ExecutionState executionState,
				IEnumerable<ListState> enableListStates, IEnumerable<ListState> disableListStates,
				IEnumerable<SelectionState> enableSelectionStates, IEnumerable<SelectionState> disableSelectionStates)
		{
			commandDescriptions_.Add(new CommandDescription(command, executionState,
				enableListStates, disableListStates, enableSelectionStates, disableSelectionStates));
		}

		/// <summary>
		/// Adds new command to enablement management.
		/// </summary>
		/// <param name="command">Specifies the command.</param>
		/// <param name="executionState">Specifies the execution state that enables the command.</param>
		/// <param name="enableListStates">Specifies the list states that enable the command. If at least one state coincide with a list state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableListStates">Specifies the list states that disable the command. If at least one state coincide with a list state, then the command is disabled. May be null or empty.</param>
		/// <param name="enableSelectionStates">Specifies the selection states that enable the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableSelectionStates">Specifies the selection states that disable the command. If at least one state coincide with a selection state, then the command is disabled. May be null or empty.</param>
		/// <param name="enableFirstElementSelectionStates">Specifies the first element selection states that enables the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableFirstElementSelectionStates">Specifies the first element selection states that disables the command. If at least one state coincide with a selection state, then the command is disabled. May be null or empty.</param>
		/// <param name="enableLastElementSelectionStates">Specifies the last element selection states that enables the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableLastElementSelectionStates">Specifies the last element selection states that disables the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		public void AddCommand(IYSCommand command, ExecutionState executionState,
				IEnumerable<ListState> enableListStates, IEnumerable<ListState> disableListStates,
				IEnumerable<SelectionState> enableSelectionStates, IEnumerable<SelectionState> disableSelectionStates,
				IEnumerable<FirstElementSelectionState> enableFirstElementSelectionStates, IEnumerable<FirstElementSelectionState> disableFirstElementSelectionStates,
				IEnumerable<LastElementSelectionState> enableLastElementSelectionStates, IEnumerable<LastElementSelectionState> disableLastElementSelectionStates)
		{
			commandDescriptions_.Add(new CommandDescription(command, executionState,
				enableListStates, disableListStates, enableSelectionStates, disableSelectionStates,
				enableFirstElementSelectionStates, disableFirstElementSelectionStates,
				enableLastElementSelectionStates, disableLastElementSelectionStates));
		}

		private void OnStateChanged()
		{
			foreach (var commandDescription in commandDescriptions_)
			{
				bool enabled = commandDescription.ExecutionState == ExecutionState.All ? true : executionState_ == commandDescription.ExecutionState;

				#region ListState

				if (enabled)
				{
					// Enable, if at least one list state is matching.
					if (commandDescription.EnableListStates.Where(state => state == ListState.All).Any())
					{
						enabled = true;
					}
					else
					{
						enabled = commandDescription.EnableListStates.Where(state => state == listState_).Any();
					}
				}

				if (enabled)
				{
					// Disable, if at least one list state is matching.
					if (commandDescription.DisableListStates.Where(state => state == ListState.All).Any())
					{
						enabled = false;
					}
					else
					{
						enabled = !commandDescription.DisableListStates.Where(state => state == listState_).Any();
					}
				}
				#endregion 

				#region Selection State
	
				if (enabled)
				{
					// Enable, if at least one selection state is matching.
					if (commandDescription.EnableSelectionStates.Where(state => state == SelectionState.All).Any())
					{
						enabled = true;
					}
					else
					{
						enabled = commandDescription.EnableSelectionStates.Where(state => state == selectionState_).Any();
					}
				}

				if (enabled)
				{
					// Disable, if at least one selection state is matching.
					if (commandDescription.DisableSelectionStates.Where(state => state == SelectionState.All).Any())
					{
						enabled = false;
					}
					else
					{
						enabled = !commandDescription.DisableSelectionStates.Where(state => state == selectionState_).Any();
					}
				}
				#endregion

				#region First Element Selection State
				
				if (enabled)
				{
					// Enable, if at least one selection state is matching.
					if (commandDescription.EnableFirstElementSelectionStates.Where(state => state == FirstElementSelectionState.All).Any())
					{
						enabled = true;
					}
					else
					{
						enabled = commandDescription.EnableFirstElementSelectionStates.Where(state => state == firstElementSelectionState_).Any();
					}
				}

				if (enabled)
				{
					// Disable, if at least one selection state is matching.
					if (commandDescription.DisableFirstElementSelectionStates.Where(state => state == FirstElementSelectionState.All).Any())
					{
						enabled = false;   
					}
					else
					{
						enabled = !commandDescription.DisableFirstElementSelectionStates.Where(state => state == firstElementSelectionState_).Any();
					}
				}
				#endregion

				#region Last Element Selection State

				if (enabled)
				{
					// Enable, if at least one selection state is matching.
					if (commandDescription.EnableLastElementSelectionStates.Where(state => state == LastElementSelectionState.All).Any())
					{
						enabled = true;
					}
					else
					{
						enabled = commandDescription.EnableLastElementSelectionStates.Where(state => state == lastElementSelectionState_).Any();
					}
				}

				if (enabled)
				{
					// Disable, if at least one selection state is matching.
					if (commandDescription.DisableLastElementSelectionStates.Where(state => state == LastElementSelectionState.All).Any())
					{
						enabled = false;
					}
					else
					{
						enabled = !commandDescription.DisableLastElementSelectionStates.Where(state => state == lastElementSelectionState_).Any();
					}
				}
				#endregion

				commandDescription.Command.Enable(enabled);
			}
		}
		#endregion
	}
}
