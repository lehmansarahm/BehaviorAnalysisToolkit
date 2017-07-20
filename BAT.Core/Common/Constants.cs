using System;

namespace BAT.Core.Common
{
    public class Constants
    {
		public const string CORE_NAMESPACE = "BAT.Core";
		public const double SAMPLING_PERIOD = 34.4827586207; // milliseconds ... approx 29 times per second

	    public enum INPUT_FILE_COLUMN_ORDER { TIME, RECORD_NUM, AZIMUTH, PITCH, ROLL, ACCEL_X, ACCEL_Y, ACCEL_Z };
	}
}