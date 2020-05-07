using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YsBuildRunner.Data;
using YsBuildRunner.Data.Enums;

namespace UnitTests
{
	internal static class DataGenerator
	{
		#region Fields

		/// <summary>
		/// Holds solution number.
		/// </summary>
		private static int SolutionNumber;

		/// <summary>
		/// Holds build number.
		/// </summary>
		private static int BuildNumber;
		#endregion

		#region Constructors

		static DataGenerator()
		{
			var random = new Random(DateTime.Now.Second);
			SolutionNumber = random.Next(1, 100);
			BuildNumber = random.Next(1, 100);
		}

		#endregion

		#region Methods
		
		/// <summary>
		/// Creates new Solution object.
		/// </summary>
		/// <returns>The new Solution object.</returns>
		public static Solution CreateSolution()
		{
			var solution = new Solution
			               	{
			               		Condition = Lists.CreateConditions().First(),
			               		Path = String.Format(@"c:\work\solution_{0}.sln", SolutionNumber),
			               		Configuration = Lists.CreateConditions().First(),
			               		Platform = Lists.CreatePlatforms().First(),
			               		SolutionName = String.Format("solution_{0}", SolutionNumber),
			               		State = State.Unknown,
			               		Task = Lists.CreateTasks().First(),
			               	};
			++SolutionNumber;

			return solution;
		}

		/// <summary>
		/// Creates build with specified number of the solutions.
		/// </summary>
		/// <param name="numberOfSolutions">Number of solutions.</param>
		/// <returns>Build with specified number of the solutions.</returns>
		public static Build CreateBuild(int numberOfSolutions)
		{
			var build = new Build
			            	{
                                BuildName = String.Format("build_{0}", BuildNumber),
                                State = State.Unknown
			            	};

			for(var i = 0; i < numberOfSolutions; ++i)
			{
				build.Solutions.Add(CreateSolution());
			}

			++BuildNumber;

			return build;
		}

		/// <summary>
		/// Creates build with specified number of the builds and solutions.
		/// </summary>
		/// <param name="numberOfBuilds">Number of builds.</param>
		/// <param name="numberOfSolutions">Number of solutions.</param>
		/// <returns>Build with specified number of the builds and solutions.</returns>
		public static Storage CreateStorage(int numberOfBuilds, int numberOfSolutions)
		{
			var storage = new Storage
			              	{
                                Version = "1.0"
			              	};
			
			for (var i = 0; i < numberOfBuilds; ++i)
			{
				storage.Builds.Add(CreateBuild(numberOfSolutions));
			}

			return storage;
		}

		#endregion
	}
}
