﻿using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Transformers
{
    public class LinearAccelerationTransformer : ITransformer
	{
		static decimal[] GRAVITY = { 0.0M, 0.0M, 0.0M };
        const int ACCEL_X = 0, ACCEL_Y = 1, ACCEL_Z = 2;

		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
		public string[] GetHeader()
		{
			return SensorReading.Header;
		}

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv()
		{
			return SensorReading.HeaderCsv;
		}

		/// <summary>
		/// Transform the specified input.
		/// </summary>
		/// <returns>The transform.</returns>
		/// <param name="input">Input.</param>
		public List<SensorReading> Transform(IEnumerable<ICsvWritable> input)
		{
			const decimal alpha = 0.8M;
            List<SensorReading> output = new List<SensorReading>();

            foreach(SensorReading reading in input)
            {
                decimal xAccel = reading.AccelX, 
                        yAccel = reading.AccelY, 
                        zAccel = reading.AccelZ;

				GRAVITY[ACCEL_X] = alpha * GRAVITY[ACCEL_X] + (1 - alpha) * xAccel;
				GRAVITY[ACCEL_Y] = alpha * GRAVITY[ACCEL_Y] + (1 - alpha) * yAccel;
				GRAVITY[ACCEL_Z] = alpha * GRAVITY[ACCEL_Z] + (1 - alpha) * zAccel;

				decimal newAccelX = (xAccel - GRAVITY[ACCEL_X]), 
                        newAccelY = (yAccel - GRAVITY[ACCEL_Y]), 
                        newAccelZ = (zAccel - GRAVITY[ACCEL_Z]);

                SensorReading newReading = new SensorReading(reading);
                newReading.SetAccelVector(newAccelX, newAccelY, newAccelZ);
                output.Add(newReading);
            }

            return output;
        }
    }
}