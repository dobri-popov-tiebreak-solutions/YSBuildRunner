using System;
using System.Threading;
using Data = YsBuildRunner.Data;
using YsBuildRunner.Data.Enums;

namespace YsBuildRunner.Executer
{
	/// <summary>
	/// Executes build for all solution in a build.
	/// </summary>
	internal class BuildExecuter : IDisposable
	{
		/// <summary>
		/// The event OnExecuted occures when build completes execution.
		/// </summary>
		public Action<string> OnExecuted;

		/// <summary>
		/// The event OnExecute occures when build of a solution is executed.
		/// </summary>
		public Action<Data.Solution> OnExecute;

		/// <summary>
		/// The event OnSolutionCompiled occures when the solution compilation was completed.
		/// </summary>
		public Action<ExecutionResult> OnSolutionCompiled;

		private bool disposed_ = false;
		#region Fields

		/// <summary>
		/// Holds the build.
		/// </summary>
		private readonly Data.Build build_;

		private readonly ManualResetEvent solutionCompletedEvent_ = new ManualResetEvent(false);

		#endregion

		#region Properties

		/// <summary>
		/// Gets running state.
		/// </summary>
		public bool Running
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the value, that specifies to continue build execution (true) or don't continue build execution (false). 
		/// </summary>
		private bool Execute
		{
			get; set;
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the BuildExecuter class.
		/// </summary>
		/// <param name="build"></param>
		public BuildExecuter(Data.Build build)
		{
			build_ = build;
			Running = false;
		}

		~BuildExecuter()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Starts build for all soltions.
		/// </summary>
		public void Start()
		{
			var thread = new Thread(DoBuild);
			Running = true;
			Execute = true;
			thread.Start();
		}

		/// <summary>
		/// Executes build for all solutions.
		/// </summary>
		private void DoBuild(object data)
		{
			build_.State = State.Running;

			var result = true;
			foreach (var solution in build_.Solutions)
			{
				solutionCompletedEvent_.Reset();

				if (!Execute)
				{
					result = false;
					break;
				}

				if (solution.Include)
				{
					var onExecute = OnExecute;
					if (onExecute != null)
					{
						onExecute(solution);
					}

					var solutionExecuter = new SolutionExecuter(solution);
					var solutionExecutionResult = solutionExecuter.Build();
					result &= solutionExecutionResult;

					var onSolutionCompiled = OnSolutionCompiled;
					if (onSolutionCompiled != null)
					{
						onSolutionCompiled(solutionExecutionResult ? ExecutionResult.Succeeded : ExecutionResult.Failed);
					}

					if (!solutionExecutionResult && solution.Condition == "Stop")
					{
						break;
					}
				}
				else
				{
					if (OnSolutionCompiled != null)
					{
						OnSolutionCompiled(ExecutionResult.Skipped);
					}
				}
			}

			build_.State = result ? State.Succeeded : State.Failed;

			Running = false;
			Execute = false;

			solutionCompletedEvent_.Set();

			if (OnExecuted != null)
			{
				OnExecuted("done");
			}
		}

		/// <summary>
		/// Stops the build.
		/// </summary>
		public void Stop()
		{
			if (!Execute)
			{
				return;
			}

			Execute = false;
			try
			{
				solutionCompletedEvent_.WaitOne();
			}
			catch
			{
			}
		}

		#endregion

		#region IDisposable Members

		private void Dispose(bool disposing)
		{
			if (!disposed_)
			{
				solutionCompletedEvent_.Set();
				solutionCompletedEvent_.Dispose();
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
