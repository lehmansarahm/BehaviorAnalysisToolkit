using System;
using BAT.Core.Common;

namespace BAT.Core.Test.SupportFiles
{
    public class DefaultInput
	{
        public static string Filename { get { return "OA5-Breakfast.csv"; } }

        public static int Index { get { return 0; } }

		public static int RawInputCount { get { return 1; } }

        // true raw record count = 3060
        // dumps records that don't have all expected fields on load
		public static int RawInputRecordCount { get { return 3057; } }

		public static int ProcessedInputRecordCount { get { return 3057; } }

        public static int SelectTaskCount { get { return 11; } }

		public static SensorReading FirstSelectBreadReading 
        {
            get
            {
				return new SensorReading
				{
					Time = DateTime.Parse("14:31:07"),
					RecordNum = 32,
					Azimuth = 2.9787378M,
					Pitch = 0.8177647M,
					Roll = 2.534702M,
					AccelX = -0.037580M, // -0.03299761 -0.004323006    0.05408764
					AccelY = -0.0016299M,
					AccelZ = 0.04918999M,
					Start = true,
					End = false,
					Label = "select-bread"
				};
            }
        }
    }
}