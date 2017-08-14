using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAT.Core.Constants;

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
        public DateTime Time { get; set; }
        public int RecordNum { get; set; }
        // ---------------------------------------------------------------------

		// ---------------------------------------------------------------------
		// Gyroscope
		// ---------------------------------------------------------------------
		public decimal Azimuth { get; set; }
        public decimal Pitch { get; set; }
		public decimal Roll { get; set; }
		// ---------------------------------------------------------------------

		// ---------------------------------------------------------------------
		// Accelerometer
		// ---------------------------------------------------------------------
		public decimal AccelX { get; set; }
        public decimal AccelY { get; set; }
		public decimal AccelZ { get; set; }
        public bool HasValidAccelVector
        {
            get
			{
				return (AccelX != 0.0M && AccelY != 0.0M && AccelZ != 0.0M);   
            }
        }
		// ---------------------------------------------------------------------

        /// <summary>
        /// Gets the accel mag.
        /// </summary>
        /// <value>The accel mag.</value>
        public decimal AccelMag
        {
            get
			{
                if (HasValidAccelVector)
                    return MathService.GetMagnitude(AccelX, AccelY, AccelZ);
                return 0.0M;
            }
        }

        /// <summary>
        /// Gets the instant speed.
        /// </summary>
        /// <value>The instant speed.</value>
        public decimal InstantSpeed
        { 
            get
			{
				if (HasValidAccelVector)
                    return (decimal)Math.Sqrt(Math.Pow((double)(AccelX * Constants.BAT.SAMPLING_PERIOD_IN_SEC), 2)
											  + Math.Pow((double)(AccelY * Constants.BAT.SAMPLING_PERIOD_IN_SEC), 2)
											  + Math.Pow((double)(AccelZ * Constants.BAT.SAMPLING_PERIOD_IN_SEC), 2));
				return 0.0M;
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
                     ? InputFile.NoLabelProvided
                     : oldReading.Label);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BAT.Core.Common.SensorReading"/> class.
        /// </summary>
        /// <param name="inputFields">Input fields.</param>
        public SensorReading(string[] inputFields)
        {
            Time = UtilityService.GetDate(inputFields, InputFile.ColumnOrder.Time);
            RecordNum = UtilityService.GetInt(inputFields, InputFile.ColumnOrder.RecordNumber);

            Azimuth = UtilityService.GetDecimal(inputFields, InputFile.ColumnOrder.Azimuth);
            Pitch = UtilityService.GetDecimal(inputFields, InputFile.ColumnOrder.Pitch);
            Roll = UtilityService.GetDecimal(inputFields, InputFile.ColumnOrder.Roll);

            AccelX = UtilityService.GetDecimal(inputFields, InputFile.ColumnOrder.AccelerationX);
            AccelY = UtilityService.GetDecimal(inputFields, InputFile.ColumnOrder.AccelarationY);
            AccelZ = UtilityService.GetDecimal(inputFields, InputFile.ColumnOrder.AccelerationZ);

            if (inputFields.Length > (int)InputFile.ColumnOrder.StartQuit)
			{
				string rawStartQuit = inputFields[(int)InputFile.ColumnOrder.StartQuit];
				Start = rawStartQuit.Equals(InputFile.StartFlag);
				End = rawStartQuit.Equals(InputFile.EndFlag);
            }

            if (inputFields.Length > (int)InputFile.ColumnOrder.Label)
			{
				Label = UtilityService.GetString(inputFields, 
                                                 InputFile.ColumnOrder.Label, 
                                                 InputFile.NoLabelProvided);
            }
		}

        /// <summary>
        /// Sets the gyro vector.
        /// </summary>
        /// <param name="azimuth">Azimuth.</param>
        /// <param name="pitch">Pitch.</param>
        /// <param name="roll">Roll.</param>
		public void SetGyroVector(decimal azimuth, decimal pitch, decimal roll)
		{
            Azimuth = azimuth;
            Pitch = pitch;
            Roll = roll;
		}

		/// <summary>
		/// Sets the accel vector.
		/// </summary>
		/// <param name="accelX">Accel x.</param>
		/// <param name="accelY">Accel y.</param>
		/// <param name="accelZ">Accel z.</param>
		public void SetAccelVector(decimal accelX, decimal accelY, decimal accelZ)
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
			List<string> failedInputLines = new List<string>();

            if (!File.Exists(filepath))
            {
                LogManager.Error($"Unable to locate input file: {filepath}.  Exiting program.");
                return sensorReadings;
            }

			var fileContent = File.ReadAllLines(filepath).Select(x => x.Split(',').ToArray());
			foreach (var contentLine in fileContent)
			{
				try
				{
					if (!contentLine[0].Equals(Header[0]))
						sensorReadings.Add(new SensorReading(contentLine));
				}
				catch (FormatException ex)
				{
                    // We've encountered an input line that is missing crucial data.
                    // Add to list of failed lines.  (do something with the ex message?)
					failedInputLines.Add(contentLine[0]);
                    continue;
				}
			}

            if (failedInputLines.Any())
			{
				LogManager.Error("Unable to parse data from input file: " +
                                 $"{filepath}, at entry lines:\n\t{string.Join("\n\t", failedInputLines)}",
                                 typeof(SensorReading));
            }

            return sensorReadings;
        }
    }
}