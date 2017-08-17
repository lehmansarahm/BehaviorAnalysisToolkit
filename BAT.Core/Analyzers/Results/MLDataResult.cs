using System;
using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
    public class MLDataResult : ICsvWritable
	{
		public string Source { get; set; }
        public IEnumerable<SensorReading> Data { get; set; }

		public decimal[] Mean { get; set; }
		public decimal[] Variance { get; set; }
		public decimal[] Skewness { get; set; }
		public decimal[] Kurtosis { get; set; }
		public decimal[] RMS { get; set; }

        /// <summary>
        /// Tos the csv array.
        /// </summary>
        /// <returns>The csv array.</returns>
        public string[] CsvArray
        {
            get
            {
                return new string[] {
                Source,
                // ----------------------------------------
                Mean[0].ToString(),
                Mean[1].ToString(),
                Mean[2].ToString(),
                MathService.GetMagnitude(Mean[0], Mean[1], Mean[2]).ToString(),
                // ----------------------------------------
                Variance[0].ToString(),
                Variance[1].ToString(),
                Variance[2].ToString(),
                MathService.GetMagnitude(Variance[0], Variance[1], Variance[2]).ToString(),
                // ----------------------------------------
                Skewness[0].ToString(),
                Skewness[1].ToString(),
                Skewness[2].ToString(),
                MathService.GetMagnitude(Skewness[0], Skewness[1], Skewness[2]).ToString(),
                // ----------------------------------------
                Kurtosis[0].ToString(),
                Kurtosis[1].ToString(),
                Kurtosis[2].ToString(),
                MathService.GetMagnitude(Kurtosis[0], Kurtosis[1], Kurtosis[2]).ToString(),
                // ----------------------------------------
                RMS[0].ToString(),
                RMS[1].ToString(),
                RMS[2].ToString(),
                MathService.GetMagnitude(RMS[0], RMS[1], RMS[2]).ToString()
            };
            }
        }

        /// <summary>
        /// Tos the csv.
        /// </summary>
        /// <returns>The csv.</returns>
        public string CsvString
        {
            get
            {
                return string.Join(",", CsvArray);
            }
        }
    }
}