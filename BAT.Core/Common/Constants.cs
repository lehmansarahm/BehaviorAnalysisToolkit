using System;

namespace BAT.Core.Common
{
    public class Constants
    {
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

		public const string COMMAND_PARAMETER_GROUP_BY = "GroupBy";
		public const string COMMAND_PARAMETER_WHERE = "Where";

		public const string INPUT_FILE_START_TRIAL_FLAG = "Start";
		public const string INPUT_FILE_END_TRIAL_FLAG = "Quit";

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
	}
}