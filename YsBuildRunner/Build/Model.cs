using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Utils.Commands;
using YsBuildRunner.Data.Enums;
using YsBuildRunner.Executer;
using SWF = System.Windows.Forms;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace YsBuildRunner.Build
{
	/// <summary>
	/// Model view for Build Window
	/// </summary>
	public class Model : IDisposable
	{
		#region Fields

		private readonly BuildWindow window_;
		private readonly EnablementManager enablementManager_ = new EnablementManager();

		private YsBuildRunner.Data.Solution selectedSolution_;

		private readonly IYSCommand commandAdd_ = new Command();
		private readonly IYSCommand commandEdit_ = new Command();
		private readonly IYSCommand commandDelete_ = new Command();
		private readonly IYSCommand commandExecuteBuild_ = new Command();
		private readonly IYSCommand commandStopBuild_ = new Command();
		private readonly IYSCommand commandDown_ = new Command();
		private readonly IYSCommand commandUp_ = new Command();
		private readonly IYSCommand commandShowLog_ = new Command();
		private readonly IYSCommand commandOk_ = new Command();
		private readonly IYSCommand commandCancel_ = new Command();
		private readonly IYSCommand commandAll_ = new Command();

		/// <summary>
		/// Holds the BuildExecuter instance.
		/// </summary>
		private BuildExecuter buildExecuter_;
		private bool disposed_ = false;
		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandAdd
		{
			get
			{
				return commandAdd_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandEdit
		{
			get
			{
				return commandEdit_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandDelete
		{
			get
			{
				return commandDelete_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandExecuteBuild
		{
			get
			{
				return commandExecuteBuild_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandStopBuild
		{
			get
			{
				return commandStopBuild_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandDown
		{
			get
			{
				return commandDown_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandUp
		{
			get
			{
				return commandUp_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandShowLog
		{
			get
			{
				return commandShowLog_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandOk
		{
			get
			{
				return commandOk_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandCancel
		{
			get
			{
				return commandCancel_;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IYSCommand CommandAll
		{
			get
			{
				return commandAll_;
			}
		}

		/// <summary>
		/// Gets the Build object.
		/// </summary>
		public YsBuildRunner.Data.Build Build
		{
			get;
			private set;
		}


		/// <summary>
		/// Gets the value that inidicates whether the window may be closed.
		/// </summary>
		public bool CanClose
		{
			get
			{
				return enablementManager_.ExecutionState != ExecutionState.Running;
			}
		}
		#endregion

		#region Constructors

		public Model(BuildWindow window, YsBuildRunner.Data.Build build)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			window_ = window;

			Build = build;

			window_.listBoxSolutions.SelectionChanged += ListBoxSolutionsSelectionChanged;

			CommandAdd.OnExecute += OnCommandAdd;
			CommandEdit.OnExecute += OnCommandEdit;
			CommandDelete.OnExecute += OnCommandDelete;
			CommandExecuteBuild.OnExecute += OnCommandExecuteBuild;
			CommandStopBuild.OnExecute += OnCommandStopBuild;
			CommandDown.OnExecute += OnCommandDown;
			CommandUp.OnExecute += OnCommandUp;
			CommandShowLog.OnExecute += OnCommandShowLog;
			CommandOk.OnExecute += OnCommandOk;
			CommandCancel.OnExecute += OnCommandCancel;
			CommandAll.OnExecute += OnCommandAll;

			enablementManager_.AddCommand(CommandAdd, ExecutionState.NoRunning,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null);
			enablementManager_.AddCommand(CommandEdit, ExecutionState.All,
				new ListState[] { ListState.NoEmpty }, new ListState[] { ListState.Empty },
				new SelectionState[] { SelectionState.OneItemSelected }, new SelectionState[] { SelectionState.NoOneItemSelected, SelectionState.ManyItemsSelected });
			enablementManager_.AddCommand(CommandDelete, ExecutionState.NoRunning,
				new ListState[] { ListState.NoEmpty }, new ListState[] { ListState.Empty },
				new SelectionState[] { SelectionState.OneItemSelected }, new SelectionState[] { SelectionState.NoOneItemSelected, SelectionState.ManyItemsSelected });
			enablementManager_.AddCommand(CommandExecuteBuild, ExecutionState.NoRunning,
				new ListState[] { ListState.NoEmpty }, null,
				new SelectionState[] { SelectionState.All }, null);
			enablementManager_.AddCommand(CommandStopBuild, ExecutionState.Running,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null);
			enablementManager_.AddCommand(CommandDown, ExecutionState.NoRunning,
				new ListState[] { ListState.NoEmpty }, new ListState[] { ListState.Empty },
				new SelectionState[] { SelectionState.OneItemSelected }, new SelectionState[] { SelectionState.NoOneItemSelected, SelectionState.ManyItemsSelected },
				new FirstElementSelectionState[] { FirstElementSelectionState.All }, null,
				new LastElementSelectionState[] { LastElementSelectionState.All }, new LastElementSelectionState[] { LastElementSelectionState.Selected });
			enablementManager_.AddCommand(CommandUp, ExecutionState.NoRunning,
				new ListState[] { ListState.NoEmpty }, new ListState[] { ListState.Empty },
				new SelectionState[] { SelectionState.OneItemSelected }, new SelectionState[] { SelectionState.NoOneItemSelected, SelectionState.ManyItemsSelected },
				new FirstElementSelectionState[] { FirstElementSelectionState.All }, new FirstElementSelectionState[] { FirstElementSelectionState.Selected },
				new LastElementSelectionState[] { LastElementSelectionState.All }, null);
			enablementManager_.AddCommand(CommandShowLog, ExecutionState.All,
				new ListState[] { ListState.NoEmpty }, new ListState[] { ListState.Empty },
				new SelectionState[] { SelectionState.OneItemSelected }, new SelectionState[] { SelectionState.NoOneItemSelected, SelectionState.ManyItemsSelected });
			enablementManager_.AddCommand(CommandOk, ExecutionState.NoRunning,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null);
			enablementManager_.AddCommand(CommandCancel, ExecutionState.NoRunning,
				new ListState[] { ListState.All }, null,
				new SelectionState[] { SelectionState.All }, null);
			enablementManager_.AddCommand(CommandAll, ExecutionState.NoRunning,
				new ListState[] { ListState.NoEmpty }, new ListState[] { ListState.Empty },
				new SelectionState[] { SelectionState.All }, null);

			enablementManager_.ListState = Build.Solutions.Any() ? ListState.NoEmpty : ListState.Empty;
			enablementManager_.SelectionState = SelectionState.NoOneItemSelected;
			enablementManager_.ExecutionState = ExecutionState.NoRunning;
		}

		~Model()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region Methods

		private void ListBoxSolutionsSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			selectedSolution_ = e.AddedItems.Count == 1 ? e.AddedItems[0] as YsBuildRunner.Data.Solution : null;

			enablementManager_.ListState = Build.Solutions.Count == 0 ? ListState.Empty : ListState.NoEmpty;
			var selectedItemsCount = Build.Solutions.Where(p => p.IsSelected).Count();

			if (selectedItemsCount == 0)
			{
				enablementManager_.SelectionState = SelectionState.NoOneItemSelected;
			}
			else if (selectedItemsCount == 1)
			{
				enablementManager_.SelectionState = SelectionState.OneItemSelected;
			}
			else
			{
				enablementManager_.SelectionState = SelectionState.ManyItemsSelected;
			}

			if (selectedItemsCount > 0)
			{
				enablementManager_.FirstElementSelectionState = Build.Solutions[0].IsSelected ?
					FirstElementSelectionState.Selected
					: FirstElementSelectionState.UnSelected;
				enablementManager_.LastElementSelectionState = Build.Solutions[Build.Solutions.Count - 1].IsSelected ?
					LastElementSelectionState.Selected
					: LastElementSelectionState.UnSelected;
			}
		}

		private void OnCommandAdd()
		{
			using (var openFileDialog = new SWF.OpenFileDialog
			{
				DefaultExt = ".sln",
				Filter = "Solution files (*.sln)|*.sln|Script files (*.bat;*.cmd;*.exe)|*.bat;*.cmd;*.exe",
				Title = "Select solution or script"
			})
			{

				if (openFileDialog.ShowDialog() == SWF.DialogResult.OK)
				{
					Build.Solutions.Add(new Data.Solution(openFileDialog.FileName));
				}
			}
		}

		private void OnCommandEdit()
		{
			if (selectedSolution_ == null)
			{
				return;
			}

			var dlg = new Solution.SolutionWindow(selectedSolution_);

            selectedSolution_.BeginEdit();

            dlg.Owner = Application.Current.MainWindow;
            var result = dlg.ShowDialog();
			if (result.HasValue && result.Value)
			{
				selectedSolution_.EndEdit();
			}
			else
			{
				selectedSolution_.CancelEdit();
			}
		}

		private void OnCommandDelete()
		{
			if (selectedSolution_ == null)
			{
				return;
			}

			if (MessageBox.Show(String.Format("Do you want to delete {0} solution?", selectedSolution_.SolutionName),
				window_.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				Build.Solutions.Remove(selectedSolution_);
			}
		}

		private void OnCommandExecuteBuild()
		{
			if (buildExecuter_ == null)
			{
				buildExecuter_ = new BuildExecuter(Build);
				buildExecuter_.OnExecuted += OnBuildExecuted;
				buildExecuter_.OnExecute += OnBuildExecute;
				buildExecuter_.OnSolutionCompiled += OnSolutionCompiled;
			}

			Build.Processed = 0;
			Build.Succeeded = 0;
			Build.Failed = 0;
			Build.Skipped = 0;

			window_.Dispatcher.BeginInvoke(DispatcherPriority.Send, (Action)(() =>
			{
				enablementManager_.ExecutionState = ExecutionState.Running;
			}));

			buildExecuter_.Start();
		}

		private void OnBuildExecuted(string name)
		{
			// Select first solution.
			window_.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
			{
				Build.Solutions[0].IsSelected = false;
				selectedSolution_ = Build.Solutions[0];

				window_.listBoxSolutions.ScrollIntoView(
					selectedSolution_);

				enablementManager_.ExecutionState = ExecutionState.NoRunning;
			}));
		}

		private void OnBuildExecute(Data.Solution solution)
		{
			Build.Processed = Build.Processed + 1;

			window_.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
			{
				selectedSolution_ = solution;
				selectedSolution_.IsSelected = true;
				window_.listBoxSolutions.ScrollIntoView(selectedSolution_);
			}));
		}

		private void OnSolutionCompiled(ExecutionResult executionResult)
		{
			window_.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
			{
				switch (executionResult)
				{
					case ExecutionResult.Succeeded:
						Build.Succeeded = Build.Succeeded + 1;
						break;
					case ExecutionResult.Failed:
						Build.Failed = Build.Failed + 1;
						break;
					case ExecutionResult.Skipped:
						Build.Skipped = Build.Skipped + 1;
						break;
					default:
						throw new ArgumentOutOfRangeException(String.Format("Unsupported result: {0}", executionResult));
				}

				foreach (var solution in Build.Solutions)
				{
					solution.IsSelected = false;
				}
			}));
		}

		private void OnCommandStopBuild()
		{
			if (buildExecuter_ != null)
			{
				window_.Dispatcher.BeginInvoke(DispatcherPriority.Send, (Action)(() =>
				{
					CommandStopBuild.Enable(false);
				}));

				ThreadPool.QueueUserWorkItem((o) =>
				{
					buildExecuter_.Stop();
				});
			}
		}

		private void OnCommandDown()
		{
			if (selectedSolution_ != null)
			{
				window_.Dispatcher.BeginInvoke(DispatcherPriority.Send, (Action)(() =>
				{
					var solutionToMove = selectedSolution_;
					var index = Build.Solutions.IndexOf(solutionToMove);

					if (index >= (Build.Solutions.Count - 1))
					{
						return;
					}

					Build.Solutions.Remove(solutionToMove);
					Build.Solutions.Insert(index + 1, solutionToMove);
					window_.listBoxSolutions.SelectedItem = solutionToMove;
				}));
			}
		}

		private void OnCommandUp()
		{
			if (selectedSolution_ != null)
			{
				window_.Dispatcher.BeginInvoke(DispatcherPriority.Send, (Action)(() =>
				{
					var solutionToMove = selectedSolution_;

					var index = Build.Solutions.IndexOf(solutionToMove);

					if (index < 1)
					{
						return;
					}
					Build.Solutions.Remove(solutionToMove);
					Build.Solutions.Insert(index - 1, solutionToMove);
					window_.listBoxSolutions.SelectedItem = solutionToMove;
				}));
			}
		}

		private void OnCommandShowLog()
		{
			if (selectedSolution_ == null)
			{
				return;
			}

			using (var p = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = selectedSolution_.LogFileName,
					WindowStyle = ProcessWindowStyle.Normal
				}
			})
			{
				p.Start();
			}
		}

		private void OnCommandOk()
		{
			window_.DialogResult = true;
			window_.Close();
		}

		private void OnCommandCancel()
		{
			window_.Close();
		}

		private void OnCommandAll()
		{
			ThreadPool.QueueUserWorkItem(o =>
			{
				if (!Build.Solutions.Any())
				{
					return;
				}

				var firstState = Build.Solutions.First().IsSelected;
				foreach (var solution in Build.Solutions)
				{
					solution.IsSelected = !firstState;
				}
			});
		}

		public void IncludeWasChanged(object sender)
		{
			var isChecked = (sender as CheckBox).IsChecked;

			foreach (var solution in Build.Solutions.Where(s => s.IsSelected))
			{
				solution.Include = isChecked.Value;
			}
		}
		#endregion

		#region IDisposable Members

		private void Dispose(bool disposing)
		{
			if (!disposed_)
			{
				if (buildExecuter_ != null)
				{
					buildExecuter_.Dispose();
				}

				disposed_ = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
