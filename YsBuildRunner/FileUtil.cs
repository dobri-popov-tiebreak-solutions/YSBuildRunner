using System;
using System.IO;
using System.Reflection;
using YsBuildRunner.Data;


namespace YsBuildRunner
{
	/// <summary>
	/// Supplies IO service.
	/// </summary>
	internal static class FileUtil
	{
		#region Methods
		
		/// <summary>
		/// Gets the log file name.
		/// </summary>
		/// <param name="solution">Specifies a solution.</param>
		/// <returns>The log file name for the solution.</returns>
		public static string GetLogName(Data.Solution solution)
		{
			return solution.IsSolution ? GetSolutionLogName(solution) : GetScriptLogName(solution);
		}

		/// <summary>
		/// Gets the log file name for solution execution.
		/// </summary>
		/// <param name="solution">Specifies a solution.</param>
		/// <returns>The log file name for the solution.</returns>
		private static string GetSolutionLogName(Data.Solution solution)
		{
			var fileName = String.Format("{0}-{1}-{2}-{3}.txt", solution.SolutionName, solution.Configuration, solution.Task, solution.Platform);

			// Replace spaces with underline.
			fileName = fileName.Replace(' ', '_');

			fileName = Path.Combine(GetFolder(), fileName);

			return fileName;
		}

		/// <summary>
		/// Gets the log file name for script execution.
		/// </summary>
		/// <param name="solution">Specifies a solution.</param>
		/// <returns>The log file name for the solution.</returns>
		private static string GetScriptLogName(Data.Solution solution)
		{
			var fileName = String.Format("{0}-script.txt", solution.SolutionName);

			// Replace spaces with underline.
			fileName = fileName.Replace(' ', '_');

			fileName = Path.Combine(GetFolder(), fileName);

			return fileName;
		}

		/// <summary>
		/// Gets the storage file name.
		/// </summary>
		/// <remarks>
		/// Creates the path to file, if necessarry.
		/// </remarks>
		/// <param name="fileDoesnotExist">Receives true, if the file doesn't exist.</param>
		/// <returns>The full path to file.</returns>
		public static string GetStorageFileName(out bool fileDoesnotExist)
		{
			var folderName = GetFolder();

			var fullpath = Path.Combine(folderName, Assembly.GetExecutingAssembly().GetName().Name + ".xml");

			fileDoesnotExist = !File.Exists(fullpath);

			return fullpath;
		}

		/// <summary>
		/// Gets the file name for window configuration.
		/// </summary>
		/// <param name="fileDoesnotExist">Receives true, if the file doesn't exist.</param>
		/// <returns>The full path to file.</returns>
		public static string GetWindowConfigFileName(out bool fileDoesnotExist)
		{
			var folderName = GetFolder();

			var fullpath = Path.Combine(folderName, "WindowConfiguration.xml");

			fileDoesnotExist = !File.Exists(fullpath);

			return fullpath;
		}

		/// <summary>
		/// Gets the application data folder.
		/// </summary>
		/// <returns></returns>
		private static string GetFolder()
		{
			var folderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
																		   Assembly.GetExecutingAssembly().GetName().Name);
			if (!Directory.Exists(folderName))
			{
				Directory.CreateDirectory(folderName);
			}

			return folderName;
		}

		#endregion
	}
}
