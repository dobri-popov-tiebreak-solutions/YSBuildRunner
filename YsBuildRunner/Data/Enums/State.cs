namespace YsBuildRunner.Data.Enums
{
	/// <summary>
	/// Defines build and solution states.
	/// </summary>
	public enum State
	{
		/// <summary>
		/// Unknown state.
		/// </summary>
		Unknown,

		/// <summary>
		/// Running state.
		/// </summary>
		Running,

		/// <summary>
		/// Build succeeded.
		/// </summary>
		Succeeded,

		/// <summary>
		/// Build failed.
		/// </summary>
		Failed
	}
}
