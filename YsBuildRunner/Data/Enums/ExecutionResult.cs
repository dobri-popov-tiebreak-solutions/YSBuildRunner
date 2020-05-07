
namespace YsBuildRunner.Data.Enums
{
	/// <summary>
	/// Defines solution build result.
	/// </summary>
	public enum ExecutionResult
	{
		/// <summary>
		/// Solution was compiled successfully.
		/// </summary>
		Succeeded,

		/// <summary>
		/// Failed compilation a solution.
		/// </summary>
		Failed,

		/// <summary>
		/// Skipped compilation a solution.
		/// </summary>
		Skipped
	}
}
