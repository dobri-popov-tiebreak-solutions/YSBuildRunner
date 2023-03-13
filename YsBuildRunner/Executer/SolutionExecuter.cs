using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using Data = YsBuildRunner.Data;
using YsBuildRunner.Data.Enums;
using YsBuildRunner.Properties;

namespace YsBuildRunner.Executer
{
	/// <summary>
	/// Executes solution build.
	/// </summary>
	internal class SolutionExecuter
	{
		#region Fields

		/// <summary>
		/// Holds the solution.
		/// </summary>
		private readonly Data.Solution solution_;
		private readonly StringBuilder outputBuffer_ = new StringBuilder();
		#endregion

		#region Properties

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the SolutionExecuter class.
		/// </summary>
		/// <param name="solution">Solution.</param>
		public SolutionExecuter(Data.Solution solution)
		{
			solution_ = solution;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Builds the solution.
		/// </summary>
		/// <returns>Build result: true, if succeeded; otherwise - false.</returns>
		public bool Build()
		{
			solution_.State = State.Running;
			
			var result = solution_.IsSolution? ExecuteSolution() : ExecuteScript();

			solution_.State = result ? State.Succeeded : State.Failed;

			return result;
		}

		/// <summary>
		/// Executes solution.
		/// </summary>
		/// <remarks>
		/// Creates the command to execute solution and runs it.
		/// </remarks>
		/// <returns>Solution build result.</returns>
		private bool ExecuteSolution()
		{
			bool result;

			using (var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = Settings.Default.BuildCommand,
					Arguments = GetCommand(),
					WindowStyle = ProcessWindowStyle.Hidden
				}
			})
			{
				try
				{
					if (process.Start())
					{
						process.WaitForExit();
						result = (process.ExitCode == 0);
					}
					else
					{
						result = false;
					}
				}
				catch (Win32Exception ex)
				{
					HandleExecutionError(ex, process.StartInfo.FileName);
					result = false;
				}
			}
			return result;
		}

		/// <summary>
		/// Executes a script.
		/// </summary>
		/// <returns></returns>
		private bool ExecuteScript()
		{
			bool result;

			using (var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = solution_.Path,
					Arguments = null,
					WindowStyle = ProcessWindowStyle.Hidden,
					WorkingDirectory = Path.GetDirectoryName(solution_.Path),
					UseShellExecute = false,
					ErrorDialog = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				},
			})
			{
				try
				{
					process.OutputDataReceived += OnOutputDataReceived;
					process.ErrorDataReceived += OnErrorDataReceived;

					if (process.Start())
					{
						// Start the asynchronous read of the output and error streams.
						process.BeginOutputReadLine();
						process.BeginErrorReadLine();

						process.WaitForExit();
						result = (process.ExitCode == 0);

						using (var output = new StreamWriter(solution_.LogFileName))
						{
							output.WriteLine(outputBuffer_.ToString());
						}
					}
					else
					{
						result = false;
					}
				}
				catch (Win32Exception ex)
				{
					HandleExecutionError(ex, process.StartInfo.FileName);
					result = false;
				}
			}
			return result;
		}

		void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			lock (outputBuffer_)
			{
				outputBuffer_.AppendLine(e.Data);
			}
		}

		void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			lock (outputBuffer_)
			{
				outputBuffer_.AppendLine(e.Data);
			}
		}

		/// <summary>
		/// Outputs the error message box.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <param name="fileName">The file name.</param>
		private static void HandleExecutionError(Win32Exception ex, string fileName)
		{
			var errMsg = String.Format("Can't execute {0}.{1}Error is \"{2}\".{3}Error code is {4}",
									   fileName,
									   Environment.NewLine, ex.Message,
									   Environment.NewLine, ex.NativeErrorCode);
			MessageBox.Show(errMsg, "Solution execution", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		/// <summary>
		/// Gets the command to build a solution.
		/// </summary>
		/// <remarks>
		/// The command has format:
		/// <para>
		/// C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild {path to solution} /t:{task} /p:Configuration={configuration};Platform="{platform}"
		/// </para>
		/// <para>
		/// Example:
		/// </para>
		/// <para>
		/// C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild C:\Development\Djenne\Dev\Common\Common.sln /t:Clean /p:Configuration=Debug;Platform="Any CPU"
		/// </para>
		/// </remarks>
		/// <returns>The command to build a solution.</returns>
		public string GetCommand()
		{
			// Full path to solution
			var buffer = new StringBuilder(solution_.Path);

            buffer.Append(" -t:restore -p:RestorePackagesConfig=true");

            // Task
			buffer.AppendFormat(" /t:{0}", solution_.Task);

			// Configuration
			buffer.AppendFormat(" /p:Configuration={0}", solution_.Configuration);

			// Configuration
			buffer.AppendFormat(";Platform=\"{0}\"", solution_.Platform);

			// Summary
			buffer.Append(" /clp:Summary");

			// Log file specification
			buffer.AppendFormat(" /fileLogger /fileLoggerParameters:LogFile=\"{0}\"", solution_.LogFileName);


            return buffer.ToString();
		}

		#endregion
	}
}
