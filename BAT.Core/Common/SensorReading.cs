using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace BAT.Core.Common
{
    public class SensorReading
    {
        // General info
        public DateTime? Time { get; set; }
        public int? RecordNum { get; set; }

        // Gyroscope
        public double? Azimuth { get; set; }
        public double? Pitch { get; set; }
        public double? Roll { get; set; }

        // Accelerometer
        public double? AccelX { get; set; }
        public double? AccelY { get; set; }
        public double? AccelZ { get; set; }

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
                {
                    return Math.Sqrt(Math.Pow(AccelX.Value, 2) + Math.Pow(AccelY.Value, 2) + Math.Pow(AccelZ.Value, 2));
                }
                else return 0.0d;
            }
        }

        /// <summary>
        /// Gets or sets the instant speed.
        /// </summary>
        /// <value>The instant speed.</value>
        public double? InstantSpeed { get; set; }

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
            this.Time = oldReading.Time;
            this.RecordNum = oldReading.RecordNum;
            this.Azimuth = oldReading.Azimuth;
            this.Pitch = oldReading.Pitch;
            this.Roll = oldReading.Roll;
            this.AccelX = oldReading.AccelX;
            this.AccelY = oldReading.AccelY;
            this.AccelZ = oldReading.AccelZ;
            this.InstantSpeed = oldReading.InstantSpeed;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BAT.Core.Common.SensorReading"/> class.
        /// </summary>
        /// <param name="inputFields">Input fields.</param>
        public SensorReading(string[] inputFields)
        {
            this.Time = DateTime.Parse(inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.TIME]);
			this.RecordNum = Int32.Parse(inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.RECORD_NUM]);
			this.Azimuth = Double.Parse(inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.AZIMUTH]);
            this.Pitch = Double.Parse(inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.PITCH]);
            this.Roll = Double.Parse(inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.ROLL]);
            this.AccelX = Double.Parse(inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.ACCEL_X]);
            this.AccelY = Double.Parse(inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.ACCEL_Y]);
            this.AccelZ = Double.Parse(inputFields[(int)Constants.INPUT_FILE_COLUMN_ORDER.ACCEL_Z]);
        }

        /// <summary>
        /// Sets the accel vector.
        /// </summary>
        /// <param name="accelX">Accel x.</param>
        /// <param name="accelY">Accel y.</param>
        /// <param name="accelZ">Accel z.</param>
        public void SetAccelVector(double? accelX, double? accelY, double? accelZ)
		{
			this.AccelX = accelX;
			this.AccelY = accelY;
			this.AccelZ = accelZ;
		}

        /// <summary>
        /// Reads the sensor file.
        /// </summary>
        /// <returns>The sensor file.</returns>
        /// <param name="filepath">Filepath.</param>
        public static List<SensorReading> ReadSensorFile(string filepath) {
            List<SensorReading> sensorReadings = new List<SensorReading>();
            using (TextFieldParser parser = new TextFieldParser(@filepath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    sensorReadings.Add(new SensorReading(fields));
                }
            }
            return sensorReadings;
        }
    }
}