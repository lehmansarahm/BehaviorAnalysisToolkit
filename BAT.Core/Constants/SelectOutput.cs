namespace BAT.Core.Constants
{
	/// <summary>
	/// Task time output.
	/// </summary>
	public static class SelectOutput
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
					"Source",
                    "Task Length (sec)",
                    "Task Start Record Num",
                    "First Pause Record Num",
                    "Number of Pauses",
                    "Accel-X Std. Dev",
					"Accel-Y Std. Dev",
					"Accel-Z Std. Dev",
					"Immediate Start",
                    "Contained Pauses",
                    "Was Single Trajectory",
					"Was Normal"
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
					"User",
                    "Select Task Count",
                    "Avg Task Time",
                    "Min Task Time",
                    "Max Task Time",
                    "Normal Select Count",
                    "Abnormal Select Count"
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
					"Avg Select Task Count",
                    "Avg Task Time (ALL)",
					"Avg Min Task Time (ALL)",
					"Avg Max Task Time (ALL)",
					"Normal Select Total",
					"Abnormal Select Total"
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