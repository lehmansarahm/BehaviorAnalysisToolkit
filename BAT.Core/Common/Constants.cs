using System;
using System.Globalization;

namespace BAT.Core.Common
{
    public static class CommandParameters
    {
        public const string Contains = "Contains";
        public const string EqualTo = "EqualTo";
        public const string GreaterThan = "GreaterThan";
        public const string LessThan = "LessThan";
        public const string Matches = "Matches";
        public const string NotEqualTo = "NotEqualTo";
        public const string Split = "Split";

        public const string Threshold = "Threshold";
        public const string Window = "Window";
    }

    public enum InputFileColumnOrder
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

    public static class Constants
    {
        public static CultureInfo CULTURE = CultureInfo.CurrentCulture;

        public static string EXECUTION_DATE_TIME = DateTime.Now.ToString("MMddyyyy-hhmmss");
        public static string OUTPUT_DIR_BY_TIME = $"output-{EXECUTION_DATE_TIME}";

        public const string OUTPUT_DIR_ANALYZERS = "analysis";
        public const string OUTPUT_DIR_FILTERS = "filters";
        public const string OUTPUT_DIR_INPUT = "input";
        public const string OUTPUT_DIR_SUMMARIZERS = "summaries";
        public const string OUTPUT_DIR_TRANSFORMERS = "transforms";

        public const double SAMPLING_PERIOD = 34.4827586207; // milliseconds ... approx 29 times per second
        public const string DEFAULT_CONFIG_FILE = "configuration.json";



        public const string INPUT_FILE_START_TRIAL_FLAG = "Start";
        public const string INPUT_FILE_END_TRIAL_FLAG = "Quit";
        public const string INPUT_FILE_NO_LABEL_PROVIDED = "no label provided";

        #region RESULTS

        //todo: move this to appropriate class for this csv file
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

        //todo: move this to appropriate class for this csv file
        /// <summary>
        /// Gets the pause result header csv.
        /// </summary>
        /// <value>The pause result header csv.</value>
		public static string PAUSE_RESULT_HEADER_CSV
        {
            get { return string.Join(",", PAUSE_RESULT_HEADER); }
        }

        //todo: move this to appropriate class for this csv file
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

        //todo: move this to appropriate class for this csv file
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

        //todo: move this to appropriate class for this csv file
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

        //todo: move this to appropriate class for this csv file
        /// <summary>
        /// Gets the pause summary header csv.
        /// </summary>
        /// <value>The pause summary header csv.</value>
        public static string PAUSE_SUMMARY_HEADER_CSV
        {
            get { return string.Join(",", PAUSE_SUMMARY_HEADER); }
        }

        //todo: move this to appropriate class for this csv file
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

        //todo: move this to appropriate class for this csv file
        /// <summary>
        /// Gets the pause summary header csv.
        /// </summary>
        /// <value>The pause summary header csv.</value>
        public static string PAUSE_SUMMARY_FOOTER_CSV
        {
            get { return string.Join(",", PAUSE_SUMMARY_FOOTER); }
        }

        //todo: move this to appropriate class for this csv file
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

        //todo: move this to appropriate class for this csv file
        /// <summary>
        /// Gets the task time summary header csv.
        /// </summary>
        /// <value>The task time summary header csv.</value>
        public static string TASK_TIME_SUMMARY_HEADER_CSV
        {
            get { return string.Join(",", TASK_TIME_SUMMARY_HEADER); }
        }

        //todo: move this to appropriate class for this csv file
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

        //todo: move this to appropriate class for this csv file
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