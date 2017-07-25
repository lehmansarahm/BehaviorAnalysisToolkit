using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public SensorReading() { }

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
            Time = GetDate(inputFields, InputFileColumnOrder.Time);
            RecordNum = GetInt(inputFields, InputFileColumnOrder.RecordNumber);

            Azimuth = GetDouble(inputFields, InputFileColumnOrder.Azimuth);
            Pitch = GetDouble(inputFields, InputFileColumnOrder.Pitch);
            Roll = GetDouble(inputFields, InputFileColumnOrder.Roll);
            AccelX = GetDouble(inputFields, InputFileColumnOrder.AccelerationX);
            AccelY = GetDouble(inputFields, InputFileColumnOrder.AccelarationY);
            AccelZ = GetDouble(inputFields, InputFileColumnOrder.AccelerationZ);

            string rawStartQuit = inputFields[(int)InputFileColumnOrder.StartQuit];
            Start = rawStartQuit.Equals(Constants.INPUT_FILE_START_TRIAL_FLAG);
            End = rawStartQuit.Equals(Constants.INPUT_FILE_END_TRIAL_FLAG);

            string rawLabel = inputFields[(int)InputFileColumnOrder.Label];
            Label = string.IsNullOrEmpty(rawLabel) ? Constants.INPUT_FILE_NO_LABEL_PROVIDED : rawLabel;
        }

        private DateTime? GetDate(string[] inputFields, InputFileColumnOrder field)
        {
            var ret = inputFields[(int)field];
            if (!string.IsNullOrWhiteSpace(ret))
                DateTime.Parse(ret);
            return null;
        }

        private int? GetInt(string[] inputFields, InputFileColumnOrder field)
        {
            var ret = inputFields[(int)field];
            if (!string.IsNullOrWhiteSpace(ret))
                if (int.TryParse(ret, out int i))
                    return i;
            return null;
        }

        private double? GetDouble(string[] inputFields, InputFileColumnOrder field)
        {
            var ret = inputFields[(int)field];
            if (!string.IsNullOrWhiteSpace(ret))
                if (double.TryParse(ret, out double i))
                    return i;
            return null;
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
        public String ToCsv()
        {
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
        public static List<SensorReading> ReadSensorFile(string filepath)
        {
            List<SensorReading> sensorReadings = new List<SensorReading>();
            if (File.Exists(filepath))
            {
                LogManager.Error($"Unable to locate input file: {filepath}.  Exiting program.");
                return sensorReadings;
            }
            try
            {
                return File.ReadAllLines(filepath)
                    .Select(x => new SensorReading(x.Split(',').ToArray()))
                    .ToList();
            }
            catch (FormatException ex)
            {
                LogManager.Error($"Unable to parse data from input file: {filepath}.  Exiting program.",
                                 ex, typeof(SensorReading));
            }

            return sensorReadings;
        }
    }
}