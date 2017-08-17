namespace BAT.Core.Constants
{
	/// <summary>
	/// Pause output.
	/// </summary>
	public static class SciKitOutput
	{
		public static string[] Header => new string[] 
        {
			"Source",
            // ------------------
			"Mean-X",
			"Mean-Y",
			"Mean-Z",
			"Mean-Mag",
            // ------------------
			"Variance-X",
			"Variance-Y",
			"Variance-Z",
			"Variance-Mag",
            // ------------------
			"Skewness-X",
			"Skewness-Y",
			"Skewness-Z",
			"Skewness-Mag",
            // ------------------
			"Kurtosis-X",
			"Kurtosis-Y",
			"Kurtosis-Z",
			"Kurtosis-Mag",
            // ------------------
			"RMS-X",
			"RMS-Y",
			"RMS-Z",
			"RMS-Mag",
            // ------------------
            "Label"
        };

		public static string HeaderCsv => string.Join(",", Header);

        /// <summary>
        /// Gets the summary header.
        /// </summary>
        /// <value>The summary header.</value>
        public static string[] SummaryHeader => new string[] { };

        /// <summary>
        /// Gets the summary header csv.
        /// </summary>
        /// <value>The summary header csv.</value>
        public static string SummaryHeaderCsv => string.Join(",", SummaryHeader);

        /// <summary>
        /// Gets the summary footer.
        /// </summary>
        /// <value>The summary footer.</value>
        public static string[] SummaryFooter => new string[] { };

        /// <summary>
        /// Gets the summary footer csv.
        /// </summary>
        /// <value>The summary footer csv.</value>
        public static string SummaryFooterCsv => string.Join(",", SummaryFooter);
    }
}