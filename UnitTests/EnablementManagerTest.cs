using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Commands;

namespace UnitTests
{
	/// <summary>
	/// This is a test class for EnablementManager and is intended
	///to contain all EnablementManager Unit Tests
	/// </summary>
	[TestClass]
	public class EnablementManagerTest
	{
		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get;
			set;
		}

		/// <summary>
		/// A test for FirstElementSelectionState handling - Enabling
		/// </summary>
		[TestMethod]
		public void FirstElementSelectionEnablingTest()
		{
			var manager = new EnablementManager();
			var command = new Command();

			manager.AddCommand(command,
				ExecutionState.All,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null,
				new FirstElementSelectionState[] { FirstElementSelectionState.Selected }, null,
				new LastElementSelectionState[] { LastElementSelectionState.All }, null);

			manager.FirstElementSelectionState = FirstElementSelectionState.Selected;
			Assert.IsTrue(command.CanExecute(null));

			manager.FirstElementSelectionState = FirstElementSelectionState.UnSelected;
			Assert.IsFalse(command.CanExecute(null));
		}

		/// <summary>
		/// A test for FirstElementSelectionState handling - Disabling
		/// </summary>
		[TestMethod]
		public void FirstElementSelectionDisablingTest()
		{
			var manager = new EnablementManager();
			var command = new Command();

			manager.AddCommand(command,
				ExecutionState.All,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null,
				new FirstElementSelectionState[] { FirstElementSelectionState.UnSelected }, null,
				new LastElementSelectionState[] { LastElementSelectionState.All }, null);

			manager.FirstElementSelectionState = FirstElementSelectionState.Selected;
			Assert.IsFalse(command.CanExecute(null));

			manager.FirstElementSelectionState = FirstElementSelectionState.UnSelected;
			Assert.IsTrue(command.CanExecute(null));
		}

		/// <summary>
		/// A test for FirstElementSelectionState handling - Enabling and Disabling
		/// </summary>
		[TestMethod]
		public void FirstElementSelectionEnablingDisablingTest()
		{
			var manager = new EnablementManager();
			var command = new Command();

			manager.AddCommand(command,
				ExecutionState.All,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null,
				new FirstElementSelectionState[] { FirstElementSelectionState.All }, new FirstElementSelectionState[]{FirstElementSelectionState.Selected},
				new LastElementSelectionState[] { LastElementSelectionState.All }, null);

			manager.FirstElementSelectionState = FirstElementSelectionState.UnSelected;
			Assert.IsTrue(command.CanExecute(null));

			manager.FirstElementSelectionState = FirstElementSelectionState.Selected;
			Assert.IsFalse(command.CanExecute(null));
		}
	}
}
