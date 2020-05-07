using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Commands
{
	/// <summary>
	/// Contains command description details.
	/// </summary>
	public class CommandDescription
	{
		#region Properties

		/// <summary>
		/// Gets the command.
		/// </summary>
		public IYSCommand Command
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the execution state.
		/// </summary>
		public ExecutionState ExecutionState
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the list states that enable the command.
		/// </summary>
		/// <remarks>
		/// If at least one state coincide with a list state, then the command is enabled.
		/// </remarks>
		public ICollection<ListState> EnableListStates
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the list states that disable the command.
		/// </summary>
		/// <remarks>
		/// If at least one state coincide with a list state, then the command is disabled.
		/// </remarks>
		public ICollection<ListState> DisableListStates
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the selection states that enable the command.
		/// </summary>
		/// <remarks>
		/// If at least one state coincide with a selection state, then the command is enabled.
		/// </remarks>
		public ICollection<SelectionState> EnableSelectionStates
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the selection states that disable the command.
		/// </summary>
		/// <remarks>
		/// If at least one state coincide with a selection state, then the command is disabled.
		/// </remarks>
		public ICollection<SelectionState> DisableSelectionStates
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the first element selection state.
		/// </summary>
		/// <remarks>
		/// If at least one state coincide with a first element selection state, then the command is enabled.
		/// </remarks>
		public ICollection<FirstElementSelectionState> EnableFirstElementSelectionStates
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the first element selection state.
		/// </summary>
		/// <remarks>
		/// If at least one state coincide with a first element selection state, then the command is disabled.
		/// </remarks>
		public ICollection<FirstElementSelectionState> DisableFirstElementSelectionStates
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the last element selection state.
		/// </summary>
		/// <remarks>
		/// If at least one state coincide with a last element selection state, then the command is enabled.
		/// </remarks>
		public ICollection<LastElementSelectionState> EnableLastElementSelectionStates
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the last element selection state.
		/// </summary>
		/// <remarks>
		/// If at least one state coincide with a last element selection state, then the command is disabled.
		/// </remarks>
		public ICollection<LastElementSelectionState> DisableLastElementSelectionStates
		{
			get;
			private set;
		}
		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the CommandDescription class.
		/// </summary>
		/// <param name="command">Specifies the command.</param>
		/// <param name="executionState">Specifies the execution state that enables the command.</param>
		/// <param name="enableListStates">Specifies the list states that enables the command. If at least one state coincide with a list state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableListStates">Specifies the list states that disables the command. If at least one state coincide with a list state, then the command is disabled. May be null or empty.</param>
		/// <param name="enableSelectionStates">Specifies the selection states that enables the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableSelectionStates">Specifies the selection states that disables the command. If at least one state coincide with a selection state, then the command is disabled. May be null or empty.</param>
		public CommandDescription(IYSCommand command, ExecutionState executionState,
			IEnumerable<ListState> enableListStates, IEnumerable<ListState> disableListStates,
			IEnumerable<SelectionState> enableSelectionStates, IEnumerable<SelectionState> disableSelectionStates)
			: this(command, executionState,
				enableListStates, disableListStates,
				enableSelectionStates, disableSelectionStates,
				new FirstElementSelectionState[] { FirstElementSelectionState.All }, null,
				new LastElementSelectionState[] { LastElementSelectionState.All }, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CommandDescription class.
		/// </summary>
		/// <param name="command">Specifies the command.</param>
		/// <param name="executionState">Specifies the execution state that enables the command.</param>
		/// <param name="enableListStates">Specifies the list states that enables the command. If at least one state coincide with a list state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableListStates">Specifies the list states that disables the command. If at least one state coincide with a list state, then the command is disabled. May be null or empty.</param>
		/// <param name="enableSelectionStates">Specifies the selection states that enables the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableSelectionStates">Specifies the selection states that disables the command. If at least one state coincide with a selection state, then the command is disabled. May be null or empty.</param>
		/// <param name="enableFirstElementSelectionStates">Specifies the first element selection states that enables the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableFirstElementSelectionStates">Specifies the first element selection states that disables the command. If at least one state coincide with a selection state, then the command is disabled. May be null or empty.</param>
		/// <param name="enableLastElementSelectionStates">Specifies the last element selection states that enables the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		/// <param name="disableLastElementSelectionStates">Specifies the last element selection states that disables the command. If at least one state coincide with a selection state, then the command is enabled. May be null or empty.</param>
		public CommandDescription(IYSCommand command, ExecutionState executionState,
			IEnumerable<ListState> enableListStates, IEnumerable<ListState> disableListStates,
			IEnumerable<SelectionState> enableSelectionStates, IEnumerable<SelectionState> disableSelectionStates,
			IEnumerable<FirstElementSelectionState> enableFirstElementSelectionStates, IEnumerable<FirstElementSelectionState> disableFirstElementSelectionStates,
			IEnumerable<LastElementSelectionState> enableLastElementSelectionStates, IEnumerable<LastElementSelectionState> disableLastElementSelectionStates)
		{
			Command = command;
			ExecutionState = executionState;
			EnableListStates = enableListStates == null ? new List<ListState>() : new List<ListState>(enableListStates);
			DisableListStates = disableListStates == null ? new List<ListState>() : new List<ListState>(disableListStates);
			EnableSelectionStates = enableSelectionStates == null ? new List<SelectionState>() : new List<SelectionState>(enableSelectionStates);
			DisableSelectionStates = disableSelectionStates == null ? new List<SelectionState>() : new List<SelectionState>(disableSelectionStates);
			EnableFirstElementSelectionStates = enableFirstElementSelectionStates == null ? new List<FirstElementSelectionState>() : new List<FirstElementSelectionState>(enableFirstElementSelectionStates);
			DisableFirstElementSelectionStates = disableFirstElementSelectionStates == null ? new List<FirstElementSelectionState>() : new List<FirstElementSelectionState>(disableFirstElementSelectionStates);
			EnableLastElementSelectionStates = enableLastElementSelectionStates == null ? new List<LastElementSelectionState>() : new List<LastElementSelectionState>(enableLastElementSelectionStates);
			DisableLastElementSelectionStates = disableLastElementSelectionStates == null ? new List<LastElementSelectionState>() : new List<LastElementSelectionState>(disableLastElementSelectionStates);
		}
		#endregion
	}
}
