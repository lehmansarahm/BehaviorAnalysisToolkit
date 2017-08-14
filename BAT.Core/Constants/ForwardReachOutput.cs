namespace BAT.Core.Constants
{
	/// <summary>
	/// Task time output.
	/// </summary>
	public static class ForwardReachOutput
	{
        /// <summary>
        /// Gets the result header.
        /// </summary>
        /// <value>The result header.</value>
        public static string[] ResultHeader => new string[]
                {
                    "First Shift Record Num (X)",
                    "First Shift Record Num (Y)",
                    "Second Shift Record Num (X)",
                    "Second Shift Record Num (Y)",
                    "Third Shift Record Num (X)",
                    "Third Shift Record Num (Y)",
                    "Reach Duration (sec)"
                };

        /// <summary>
        /// Gets the result header csv.
        /// </summary>
        /// <value>The result header csv.</value>
        public static string ResultHeaderCsv => string.Join(",", ResultHeader);

        /// <summary>
        /// Gets the summary header.
        /// </summary>
        /// <value>The summary header.</value>
        public static string[] SummaryHeader => new string[]
                {
                    "Source",
                    "Select Task Name",
                    "Select Task Start Num",
                    "Forward Reach Start Num",
                    "Is Match"
                };

        /// <summary>
        /// Gets the summary header csv.
        /// </summary>
        /// <value>The summary header csv.</value>
        public static string SummaryHeaderCsv => string.Join(",", SummaryHeader);
    }
}