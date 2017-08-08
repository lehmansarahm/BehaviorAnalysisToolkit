namespace BAT.Core.Constants
{
	/// <summary>
	/// Task time output.
	/// </summary>
	public static class ReachRetractOutput
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
					"Start Label",
					"End Time",
					"End Num",
					"End Label",
					"Reach Duration (sec)",
					"Was Grab?"
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
					"Reach and Grab Count",
					"Select Task Count",
					"Reach and No-Touch Count"
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
					"", // source
                    "Total Reach and Grab Count",
					"", // select task
                    "Total Reach and No-Touch Count"
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