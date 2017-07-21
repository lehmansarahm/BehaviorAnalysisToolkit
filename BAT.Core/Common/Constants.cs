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

		public const string OUTPUT_DIR_ANALYZERS = "/out-analysis";
		public const string OUTPUT_DIR_FILTERS = "/out-filters/";
		public const string OUTPUT_DIR_INPUT = "/out-input/";
        public const string OUTPUT_DIR_SUMMARIZERS = "/out-summaries/";
        public const string OUTPUT_DIR_TRANSFORMERS = "/out-transforms/";

		public const double SAMPLING_PERIOD = 34.4827586207; // milliseconds ... approx 29 times per second
        public const string DEFAULT_CONFIG_FILE = "configuration.json";

	    public enum INPUT_FILE_COLUMN_ORDER { TIME, RECORD_NUM, AZIMUTH, PITCH, ROLL, ACCEL_X, ACCEL_Y, ACCEL_Z };
	}
}