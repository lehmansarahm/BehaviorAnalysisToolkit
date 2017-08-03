using System;
using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Transformers
{
    public class SmoothingTransformer : ITransformer
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
		public string[] GetHeader() { return SensorReading.Header; }

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv() { return SensorReading.HeaderCsv; }

        /// <summary>
        /// Transform the specified input.
        /// </summary>
        /// <returns>The transform.</returns>
        /// <param name="input">Input.</param>
        public List<SensorReading> Transform(IEnumerable<ICsvWritable> input)
        {
            var emaAzimuth = new ExponentialMovingAverage();
            var emaPitch = new ExponentialMovingAverage();
            var emaRoll = new ExponentialMovingAverage();

            var emaAccelX = new ExponentialMovingAverage();
            var emaAccelY = new ExponentialMovingAverage();
            var emaAccelZ = new ExponentialMovingAverage();

            var output = new List<SensorReading>();
            foreach (SensorReading reading in input)
            {
				var newReading = new SensorReading(reading);
                newReading.SetGyroVector(emaAzimuth.GetAverage(reading.Azimuth),
                                         emaPitch.GetAverage(reading.Pitch),
                                         emaRoll.GetAverage(reading.Roll));
                newReading.SetAccelVector(emaAccelX.GetAverage(reading.AccelX), 
                                          emaAccelY.GetAverage(reading.AccelY), 
                                          emaAccelZ.GetAverage(reading.AccelZ));
                output.Add(newReading);
            }
            return output;
        }
    }
}