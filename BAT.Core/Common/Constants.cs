using System;
using System.Globalization;

namespace BAT.Core.Common
{
    /// <summary>
    /// Constants.
    /// </summary>
	public static class Constants
	{
		public static CultureInfo CULTURE = CultureInfo.CurrentCulture;
        public const string DEFAULT_CONFIG_FILE = "configuration.json";
        public const string DEFAULT_INPUT_FILE_EXT = ".csv";
        public const char DEFAULT_PATH_SEPARATOR = '/';
        public const char DEFAULT_NAME_SEPARATOR = '_';

		// milliseconds ... approx 29 times per second
		public const decimal SAMPLING_PERIOD_IN_MS = 34.4827586207M; 
		public const string EMPTY = "";
	}

    /// <summary>
    /// Command parameters.
    /// </summary>
    public static class CommandParameters
    {
        public const string Contains = "Contains";
        public const string EqualTo = "EqualTo";
        public const string GreaterThan = "GreaterThan";
        public const string LessThan = "LessThan";
        public const string Matches = "Matches";
        public const string NotEqualTo = "NotEqualTo";
        public const string Split = "Split";

		public const string Percentage = "Percentage";
		public const string Step = "Step";
        public const string Threshold = "Threshold";
        public const string Window = "Window";
    }

    /// <summary>
    /// Output dirs.
    /// </summary>
    public static class OutputDirs
	{
        static string ExecDateTime = DateTime.Now.ToString("MMddyyyy-hhmmss");
        public static string ExecTime = $"output-{ExecDateTime}";

		public const string Analyzers = "analysis";
		public const string Filters = "filters";
		public const string Inputs = "input";
		public const string Summarizers = "summaries";
		public const string Transformers = "transforms";
    }

    /// <summary>
    /// Input file.
    /// </summary>
    public static class InputFile
	{
		public const string StartFlag = "Start";
		public const string EndFlag = "Quit";
		public const string NoLabelProvided = "no label provided";

		public enum ColumnOrder
		{
			Time,
			RecordNumber,
			Azimuth,
			Pitch,
			Roll,
			AccelerationX,
			AccelarationY,
			AccelerationZ,
			StartQuit,
			Label
		};
    }

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
					"Task Duration (sec)"
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