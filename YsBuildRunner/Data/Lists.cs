using System.Collections.Generic;

namespace YsBuildRunner.Data
{
	internal static class Lists
	{
		/// <summary>
		/// Creates list of the conditions.
		/// </summary>
		/// <returns>The list of the conditions.</returns>
		public static IEnumerable<string> CreateConditions()
		{
			return new List<string>
			       	{
                        "Stop",
                        "Continue",
			       	};
		}

		/// <summary>
		/// Creates list of the configurations.
		/// </summary>
		/// <returns>The list of the configurations.</returns>
		public static IEnumerable<string> CreateConfigurations()
		{
			return new List<string>
			       	{
                        "Debug",
                        "Release",
						"Unusable"
			       	};
		}

		/// <summary>
		/// Creates list of the platforms.
		/// </summary>
		/// <returns>The list of the platforms.</returns>
		public static IEnumerable<string> CreatePlatforms()
		{
			return new List<string>
			       	{
                        "Any CPU",
                        "x64",
                        "x86",
						"Unusable"
			       	};
		}

		/// <summary>
		/// Creates list of the tasks.
		/// </summary>
		/// <returns>The list of the tasks.</returns>
		public static IEnumerable<string> CreateTasks()
		{
			return new List<string>
			       	{
                        "Rebuild",
                        "Clean",
                        "Unusable"
			       	};
		}
	}
}
