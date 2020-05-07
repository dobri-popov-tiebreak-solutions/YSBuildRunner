using System.Collections.Generic;
using YsBuildRunner.Data;

namespace YsBuildRunner.Solution
{
	/// <summary>
	/// Model view for Solution Window
	/// </summary>
	public class Model
	{
		#region Properties

		/// <summary>
		/// Gets the TaskList.
		/// </summary>
		public IEnumerable<string> TaskList
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the PlatformList.
		/// </summary>
		public IEnumerable<string> PlatformList
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the ConfigurationList.
		/// </summary>
		public IEnumerable<string> ConfigurationList
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the ConditionList.
		/// </summary>
		public IEnumerable<string> ConditionList
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the Solution.
		/// </summary>
		public Data.Solution Solution
		{
			get;
			private set;
		}		
		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance Model class.
		/// </summary>
		/// <param name="solution">The Solution.</param>
		public Model(Data.Solution solution)
		{
			Solution = solution;

			TaskList = Lists.CreateTasks();
			PlatformList = Lists.CreatePlatforms();
			ConfigurationList = Lists.CreateConfigurations();
			ConditionList = Lists.CreateConditions();
		}
		#endregion
 	}
}
