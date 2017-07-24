using System;
using System.Collections.Generic;
using System.IO;

namespace BAT.Core.Common
{
    public class SensorReading : ICsvWritable
    {
        // General purpose field header
        public static string[] Header 
        {
            get 
            {
                return new string[]
                {
                    "Time",
                    "Record Num",
                    "Azimuth",
                    "Pitch",
                    "Roll",
                    "AccelX",
                    "AccelY",
                    "AccelZ",
                    "AccelMag",
                    "InstantSpeed",
                    "StartQuit",
                    "Label"
                };
            }
        }
        public static string HeaderCsv { get { return string.Join(",", Header); } }

        // ---------------------------------------------------------------------
		// General info
		// ---------------------------------------------------------------------
		public DateTime? Time { get; set; }
		public int? RecordNum { get; set; }
		// ---------------------------------------------------------------------

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:BAT.Core.Common.SensorReading"/> has valid time data.
        /// </summary>
        /// <value><c>true</c> if has valid time data; otherwise, <c>false</c>.</value>
        public bool HasValidTimeData
        {
            get
            {
                return (Time.HasValue && RecordNum.HasValue);
            }
        }

		// ---------------------------------------------------------------------
		// Gyroscope
		// ---------------------------------------------------------------------
		public double? Azimuth { get; set; }
        public double? Pitch { get; set; }
		public double? Roll { get; set; }
		// ---------------------------------------------------------------------

		// ---------------------------------------------------------------------
		// Accelerometer
		// ---------------------------------------------------------------------
		public double? AccelX { get; set; }
        public double? AccelY { get; set; }
		public double? AccelZ { get; set; }
		// ---------------------------------------------------------------------

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:BAT.Core.Common.SensorReading"/> has valid accel vector.
		/// </summary>
		/// <value><c>true</c> if has valid accel vector; otherwise, <c>false</c>.</value>
		public Boolean HasValidAccelVector
        {
            get
            {
                return (AccelX != null && AccelY != null && AccelZ != null);
            }
        }

        /// <summary>
        /// Gets the accel mag.
        /// </summary>
        /// <value>The accel mag.</value>
        public double AccelMag
        {
            get
            {
                if (HasValidAccelVector)
                    return Math.Sqrt(Math.Pow(AccelX.Value, 2) 
                                     + Math.Pow(AccelY.Value, 2) 
                                     + Math.Pow(AccelZ.Value, 2));
                return 0.0d;
            }
        }

        /// <summary>
        /// Gets the instant speed.
        /// </summary>
        /// <value>The instant speed.</value>
        public double InstantSpeed
        { 
            get
            {
				return AccelMag * (Constants.SAMPLING_PERIOD / 1000.0d);
            }
        }

		// ---------------------------------------------------------------------
		// Record keeping (Tania)
		// ---------------------------------------------------------------------
		public bool Start { get; set; }
        public bool End { get; set; }
		public string Label { get; set; }
		// ---------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="T:BAT.Core.Common.SensorReading"/> class.
		/// </summary>
		public SensorReading() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BAT.Core.Common.SensorReading"/> class.
        /// </summary>
        /// <param name="oldReading">Old reading.</param>
        public SensorReading(SensorReading oldReading)
		{
            Time = oldReading.Time;
            RecordNum = oldReading.RecordNum;
            Azimuth = oldReading.Azimuth;
            Pitch = oldReading.Pitch;
            Roll = oldReading.Roll;
            AccelX = oldReading.AccelX;
            AccelY = oldReading.AccelY;
            AccelZ = oldReading.AccelZ;
            Start = oldReading.Start;
            End = oldReading.End;
            Label = (string.IsNullOrEmpty(oldReading.Label) 
                     ? Constants.INPUT_FILE_NO_LABEL_PROVIDED 
                     : oldReading.Label);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BAT.Core.Common.SensorReading"/> class.
        /// </summary>
        /// <param name="inputFields">Input fields.</param>
        public SensorReading(string[] inputFields)
        {
            string rawTime = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.TIME];
            if (!String.IsNullOrEmpty(rawTime)) Time = DateTime.Parse(rawTime);

            string rawRecordNum = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.RECORD_NUM];
			if (!String.IsNullOrEmpty(rawRecordNum)) RecordNum = Int32.Parse(rawRecordNum);

			string rawAzimuth = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.AZIMUTH];
			if (!String.IsNullOrEmpty(rawAzimuth)) Azimuth = Double.Parse(rawAzimuth);

            string rawPitch = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.PITCH];
            if (!String.IsNullOrEmpty(rawPitch)) Pitch = Double.Parse(rawPitch);

            string rawRoll = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.ROLL];
			if (!String.IsNullOrEmpty(rawRoll)) Roll = Double.Parse(rawRoll);

            string rawAccelX = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.ACCEL_X];
			if (!String.IsNullOrEmpty(rawAccelX)) AccelX = Double.Parse(rawAccelX);

            string rawAccelY = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.ACCEL_Y];
			if (!String.IsNullOrEmpty(rawAccelY)) AccelY = Double.Parse(rawAccelY);

            string rawAccelZ = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.ACCEL_Z];
			if (!String.IsNullOrEmpty(rawAccelZ)) AccelZ = Double.Parse(rawAccelZ);

            string rawStartQuit = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.START_QUIT];
            Start = rawStartQuit.Equals(Constants.INPUT_FILE_START_TRIAL_FLAG);
            End = rawStartQuit.Equals(Constants.INPUT_FILE_END_TRIAL_FLAG);

			string rawLabel = inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.LABEL];
			Label = (string.IsNullOrEmpty(rawLabel) ? Constants.INPUT_FILE_NO_LABEL_PROVIDED : rawLabel);
        }

        /// <summary>
        /// Sets the accel vector.
        /// </summary>
        /// <param name="accelX">Accel x.</param>
        /// <param name="accelY">Accel y.</param>
        /// <param name="accelZ">Accel z.</param>
        public void SetAccelVector(double? accelX, double? accelY, double? accelZ)
		{
			AccelX = accelX;
			AccelY = accelY;
			AccelZ = accelZ;
		}

        /// <summary>
        /// Tos the csv.
        /// </summary>
        /// <returns>The csv.</returns>
        public String ToCsv() {
			string[] props = {
				Time.ToString(), 
                RecordNum.ToString(),
				Azimuth.ToString(), 
                Pitch.ToString(), 
                Roll.ToString(),
				AccelX.ToString(), 
                AccelY.ToString(), 
                AccelZ.ToString(),
				AccelMag.ToString(), 
                InstantSpeed.ToString(),
                (Start ? "Start" : (End ? "Quit" : "")),
                Label
			};
            return string.Join(",", props);
        }

        /// <summary>
        /// Reads the sensor file.
        /// </summary>
        /// <returns>The sensor file.</returns>
        /// <param name="filepath">Filepath.</param>
        public static List<SensorReading> ReadSensorFile(string filepath) {
            try
			{
				List<SensorReading> sensorReadings = new List<SensorReading>();
				using (var reader = new StreamReader(@filepath))
				{
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						string[] fields = line.Split(',');
						sensorReadings.Add(new SensorReading(fields));
					}
				}
				return sensorReadings;
            } catch (FileNotFoundException ex) {
                LogManager.Error("Unable to read data from input file.  Exiting program.", 
                                 ex, typeof(SensorReading));
                return null;
            }
        }
    }
}