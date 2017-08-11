namespace BAT.Core.Constants
{
	/// <summary>
	/// Pause output.
	/// </summary>
	public static class PauseOutput
	{
		/// <summary>
		/// Gets the result header.
		/// </summary>
		/// <value>The result header.</value>
		public static string[] ResultHeader
		{
			get
			{
				return new string[]
				{
					"Start Time",
					"Start Num",
					"End Time",
					"End Num",
					"Pause Duration (sec)"
				};
			}
		}

		/// <summary>
		/// Gets the result header csv.
		/// </summary>
		/// <value>The result header csv.</value>
		public static string ResultHeaderCsv
		{
			get { return string.Join(",", ResultHeader); }
		}

		/// <summary>
		/// Gets the summary header.
		/// </summary>
		/// <value>The summary header.</value>
		public static string[] SummaryHeader
		{
			get
			{
				return new string[]
				{
					"Source",
					"Number of Pauses",
					"Time Paused (sec)",
					"Average Pause Duration (sec)"
				};
			}
		}

		/// <summary>
		/// Gets the summary header csv.
		/// </summary>
		/// <value>The summary header csv.</value>
		public static string SummaryHeaderCsv
		{
			get { return string.Join(",", SummaryHeader); }
		}

		/// <summary>
		/// Gets the summary footer.
		/// </summary>
		/// <value>The summary footer.</value>
		public static string[] SummaryFooter
		{
			get
			{
				return new string[]
				{
                    "",
					"Total Number of Pauses",
					"Total Time Paused (sec)",
					"Total Average Pause Duration (sec)"
				};
			}
		}

		/// <summary>
		/// Gets the summary footer csv.
		/// </summary>
		/// <value>The summary footer csv.</value>
		public static string SummaryFooterCsv
		{
			get { return string.Join(",", SummaryFooter); }
		}
	}
}