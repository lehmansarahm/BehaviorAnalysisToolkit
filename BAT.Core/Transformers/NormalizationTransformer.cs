using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Transformers
{
    public class NormalizationTransformer : ITransformer
	{
        const int MIN = 0, MAX = 1;
		const int ACCEL_X = 0, ACCEL_Y = 1, ACCEL_Z = 2;

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
				new decimal[] { 0.0M, 0.0M },   // accel-x
				new decimal[] { 0.0M, 0.0M },   // accel-y
				new decimal[] { 0.0M, 0.0M }    // accel-z
            };

            // iterate through once to find the min / max speed values
            foreach (SensorReading reading in input)
			{
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