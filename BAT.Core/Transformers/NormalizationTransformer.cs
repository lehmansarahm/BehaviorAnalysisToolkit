﻿using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Transformers
{
    public class NormalizationTransformer : ITransformer
	{
        const int MIN = 0, MAX = 1;
		const int AZIMUTH = 0, PITCH = 1, ROLL = 2, 
                  ACCEL_X = 3, ACCEL_Y = 4, ACCEL_Z = 5;

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
			List<SensorReading> output = new List<SensorReading>();
            var minMax = new List<decimal[]>
			{
				new decimal[] { 0.0M, 0.0M },   // azimuth
                new decimal[] { 0.0M, 0.0M },   // pitch
                new decimal[] { 0.0M, 0.0M },   // roll
                new decimal[] { 0.0M, 0.0M },   // accel-x
				new decimal[] { 0.0M, 0.0M },   // accel-y
				new decimal[] { 0.0M, 0.0M }    // accel-z
            };

            // iterate through once to find the min / max speed values
            foreach (SensorReading reading in input)
			{
                if (reading.Azimuth < minMax[AZIMUTH][MIN])
					minMax[AZIMUTH][MIN] = reading.Azimuth;
				if (reading.Azimuth > minMax[AZIMUTH][MAX])
					minMax[AZIMUTH][MAX] = reading.Azimuth;

                if (reading.Pitch < minMax[PITCH][MIN])
					minMax[PITCH][MIN] = reading.Pitch;
				if (reading.Pitch > minMax[PITCH][MAX])
					minMax[PITCH][MAX] = reading.Pitch;

                if (reading.Roll < minMax[ROLL][MIN])
					minMax[ROLL][MIN] = reading.Roll;
				if (reading.Roll > minMax[ROLL][MAX])
					minMax[ROLL][MAX] = reading.Roll;
                
                // -----------------------------------------------

				if (reading.AccelX < minMax[ACCEL_X][MIN])
					minMax[ACCEL_X][MIN] = reading.AccelX;
				if (reading.AccelX > minMax[ACCEL_X][MAX])
					minMax[ACCEL_X][MAX] = reading.AccelX;

				if (reading.AccelY < minMax[ACCEL_Y][MIN])
                    minMax[ACCEL_Y][MIN] = reading.AccelY;
                if (reading.AccelY > minMax[ACCEL_Y][MAX])
                    minMax[ACCEL_Y][MAX] = reading.AccelY;

                if (reading.AccelZ < minMax[ACCEL_Z][MIN])
					minMax[ACCEL_Z][MIN] = reading.AccelZ;
				if (reading.AccelZ > minMax[ACCEL_Z][MAX])
					minMax[ACCEL_Z][MAX] = reading.AccelZ;
            }

            // go through a second time to calculate normalized values
            foreach (SensorReading reading in input)
            {
				var newReading = new SensorReading(reading);
                newReading.Azimuth = (reading.Azimuth - minMax[AZIMUTH][MIN]) /
					(minMax[AZIMUTH][MAX] - minMax[AZIMUTH][MIN]);
                newReading.Pitch = (reading.Pitch - minMax[PITCH][MIN]) /
					(minMax[PITCH][MAX] - minMax[PITCH][MIN]);
				newReading.Roll = (reading.Roll - minMax[ROLL][MIN]) /
					(minMax[ROLL][MAX] - minMax[ROLL][MIN]);
				newReading.AccelX = (reading.AccelX - minMax[ACCEL_X][MIN]) /
					(minMax[ACCEL_X][MAX] - minMax[ACCEL_X][MIN]);
                newReading.AccelY = (reading.AccelY - minMax[ACCEL_Y][MIN]) /
                    (minMax[ACCEL_Y][MAX] - minMax[ACCEL_Y][MIN]);
                newReading.AccelZ = (reading.AccelZ - minMax[ACCEL_Z][MIN]) /
                    (minMax[ACCEL_Z][MAX] - minMax[ACCEL_Z][MIN]);
                output.Add(newReading);
            }

            return output;
        }
    }
}