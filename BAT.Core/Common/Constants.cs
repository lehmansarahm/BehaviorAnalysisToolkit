using System;
using System.Globalization;

namespace BAT.Core.Common
{
    public class Constants
    {
        public static CultureInfo CULTURE = CultureInfo.CurrentCulture;

        public const string NAMESPACE_CORE = "BAT.Core";
        public const string NAMESPACE_ANALYZER_IMPL = NAMESPACE_CORE + ".Analyzers.Impl.";
        public const string NAMESPACE_FILTER_IMPL = NAMESPACE_CORE + ".Filters.Impl.";
        public const string NAMESPACE_SUMMARIZER_IMPL = NAMESPACE_CORE + ".Summarizers.Impl.";
        public const string NAMESPACE_TRANSFORMER_IMPL = NAMESPACE_CORE + ".Transformers.Impl.";

        public static string EXECUTION_DATE_TIME = DateTime.Now.ToString("MMddyyyy-hhmmss");
        public static string OUTPUT_DIR_BY_TIME = $"output-{EXECUTION_DATE_TIME}";

        public const string OUTPUT_DIR_ANALYZERS = "analysis";
        public const string OUTPUT_DIR_FILTERS = "filters";
        public const string OUTPUT_DIR_INPUT = "input";
        public const string OUTPUT_DIR_SUMMARIZERS = "summaries";
        public const string OUTPUT_DIR_TRANSFORMERS = "transforms";

        public const double SAMPLING_PERIOD = 34.4827586207; // milliseconds ... approx 29 times per second
        public const string DEFAULT_CONFIG_FILE = "configuration.json";

        #region Phase Implementation Suffixes
        public const string PHASE_IMPL_TRANSFORMER = "Transformer";
        public const string PHASE_IMPL_FILTER = "Filter";
        public const string PHASE_IMPL_ANALYZER = "Analysis";
        public const string PHASE_IMPL_SUMMARIZER = "Summarizer";
        #endregion

        #region Command Parameters - Functional Clauses
        public const string COMMAND_PARAM_CONTAINS = "Contains";
        public const string COMMAND_PARAM_EQUAL_TO = "EqualTo";
        public const string COMMAND_PARAM_GREATER_THAN = "GreaterThan";
        public const string COMMAND_PARAM_LESS_THAN = "LessThan";
        public const string COMMAND_PARAM_MATCHES = "Matches";
        public const string COMMAND_PARAM_NOT_EQUAL_TO = "NotEqualTo";
        public const string COMMAND_PARAM_SPLIT = "Split";
        #endregion

        #region Command Parameters - Explicit
        public const string COMMAND_PARAM_THRESHOLD = "Threshold";
        public const string COMMAND_PARAM_WINDOW = "Window";
        #endregion

        public const string INPUT_FILE_START_TRIAL_FLAG = "Start";
        public const string INPUT_FILE_END_TRIAL_FLAG = "Quit";
        public const string INPUT_FILE_NO_LABEL_PROVIDED = "no label provided";

        public enum INPUT_FILE_COLUMN_ORDER
        {
            TIME,
            RECORD_NUM,
            AZIMUTH,
            PITCH,
            ROLL,
            ACCEL_X,
            ACCEL_Y,
            ACCEL_Z,
            START_QUIT,
            LABEL
        };

		#region RESULTS

        /// <summary>
        /// Gets the pause result header.
        /// </summary>
        /// <value>The pause result header.</value>
		public static string[] PAUSE_RESULT_HEADER
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
        /// Gets the pause result header csv.
        /// </summary>
        /// <value>The pause result header csv.</value>
		public static string PAUSE_RESULT_HEADER_CSV 
        { 
            get { return string.Join(",", PAUSE_RESULT_HEADER); } 
        }

        /// <summary>
        /// Gets the task time result header.
        /// </summary>
        /// <value>The task time result header.</value>
		public static string[] TASK_TIME_RESULT_HEADER
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
        /// Gets the task time result header csv.
        /// </summary>
        /// <value>The task time result header csv.</value>
		public static string TASK_TIME_RESULT_HEADER_CSV
        { 
            get { return string.Join(",", TASK_TIME_RESULT_HEADER); } 
        }

		#endregion

		#region SUMMARIES

		/// <summary>
		/// Gets the task time summary header.
		/// </summary>
		/// <value>The task time summary header.</value>
		public static string[] PAUSE_SUMMARY_HEADER
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
		/// Gets the pause summary header csv.
		/// </summary>
		/// <value>The pause summary header csv.</value>
		public static string PAUSE_SUMMARY_HEADER_CSV
		{
			get { return string.Join(",", PAUSE_SUMMARY_HEADER); }
		}

		/// <summary>
		/// Gets the task time summary header.
		/// </summary>
		/// <value>The task time summary header.</value>
		public static string[] PAUSE_SUMMARY_FOOTER
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
		/// Gets the pause summary header csv.
		/// </summary>
		/// <value>The pause summary header csv.</value>
		public static string PAUSE_SUMMARY_FOOTER_CSV
		{
			get { return string.Join(",", PAUSE_SUMMARY_FOOTER); }
		}

		/// <summary>
		/// Gets the task time summary header.
		/// </summary>
		/// <value>The task time summary header.</value>
		public static string[] TASK_TIME_SUMMARY_HEADER
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
        /// Gets the task time summary header csv.
        /// </summary>
        /// <value>The task time summary header csv.</value>
        public static string TASK_TIME_SUMMARY_HEADER_CSV 
        { 
            get { return string.Join(",", TASK_TIME_SUMMARY_HEADER); } 
        }

        /// <summary>
        /// Gets the task time summary footer.
        /// </summary>
        /// <value>The task time summary footer.</value>
        public static string[] TASK_TIME_SUMMARY_FOOTER
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
        /// Gets the task time summary footer csv.
        /// </summary>
        /// <value>The task time summary footer csv.</value>
        public static string TASK_TIME_SUMMARY_FOOTER_CSV 
        {
            get { return string.Join(",", TASK_TIME_SUMMARY_FOOTER); } 
        }

        #endregion

    }
}