using System.Globalization;

namespace BAT.Core.Constants
{
    public static class BAT
	{
		public static CultureInfo CULTURE = CultureInfo.CurrentCulture;

		public const string DEFAULT_CONFIG_FILE = "configuration.json";
		public const string DEFAULT_ERROR_LOG_FILE = "error.txt";
		public const string DEFAULT_DEBUG_LOG_FILE = "debug.txt";

		public const string DEFAULT_INPUT_FILE_EXT = ".csv";
		public const char DEFAULT_PATH_SEPARATOR = '/';
		public const char DEFAULT_NAME_SEPARATOR = '_';

		// milliseconds ... approx 29 times per second
		public const decimal SAMPLING_PERIOD_IN_MS = 34.4827586207M;
        public const decimal SAMPLING_PERIOD_IN_SEC = (SAMPLING_PERIOD_IN_MS / 1000.0M);
		public const string EMPTY = "";
    }
}