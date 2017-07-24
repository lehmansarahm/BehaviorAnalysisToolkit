using System;
using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Transformers.Impl
{
	public class LabelCleanupTransformer : ITransformer
	{
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
			List<SensorReading> output = new List<SensorReading>();
			foreach (SensorReading reading in input)
			{
                // strip timestamp
                var timestampComponents = reading.Label.Split(':');
                var label = timestampComponents[timestampComponents.Length - 1];

                // remove leading, trailing whitespace
                label = label.Trim();

                // replace remaining whitespace with underscores
                label = label.Replace(' ', '-');

                // return transformed label
                reading.Label = label;
                output.Add(reading);
			}

			return output;
        }
    }
}