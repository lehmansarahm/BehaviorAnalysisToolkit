namespace BAT.Core.Constants
{
	/// <summary>
	/// Task time output.
	/// </summary>
	public static class TaskTimeOutput
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
					"Task Duration (sec)"
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
                    "Total Task Count",
					"Total Task Duration (sec)"
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
                    "Total Task Count",
					"Total Task Time (sec)",
					"Average Task Time (sec)",
					"Standard Deviation"
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