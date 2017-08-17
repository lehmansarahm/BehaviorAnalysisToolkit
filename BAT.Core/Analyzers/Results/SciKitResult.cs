using System;
using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
    public class SciKitResult : ICsvWritable
	{
		public string Source { get; set; }
        public IEnumerable<SensorReading> Data { get; set; }

		public decimal[] Mean { get; set; }
        decimal MeanMag => MathService.GetMagnitude(Mean[0], Mean[1], Mean[2]);

		public decimal[] Variance { get; set; }
        decimal VarianceMag => MathService.GetMagnitude(Variance[0], Variance[1], Variance[2]);

		public decimal[] Skewness { get; set; }
		decimal SkewnessMag => MathService.GetMagnitude(Skewness[0], Skewness[1], Skewness[2]);

		public decimal[] Kurtosis { get; set; }
		decimal KurtosisMag => MathService.GetMagnitude(Kurtosis[0], Kurtosis[1], Kurtosis[2]);

		public decimal[] RMS { get; set; }
		decimal RMSMag => MathService.GetMagnitude(RMS[0], RMS[1], RMS[2]);

		public string Label { get; set; }

		public static string[] Header => new string[]
		{
			"Source",
            // ------------------
            "Mean-X",
			"Mean-Y",
			"Mean-Z",
			"Mean-Mag",
            // ------------------
            "Variance-X",
			"Variance-Y",
			"Variance-Z",
			"Variance-Mag",
            // ------------------
            "Skewness-X",
			"Skewness-Y",
			"Skewness-Z",
			"Skewness-Mag",
            // ------------------
            "Kurtosis-X",
			"Kurtosis-Y",
			"Kurtosis-Z",
			"Kurtosis-Mag",
            // ------------------
            "RMS-X",
			"RMS-Y",
			"RMS-Z",
			"RMS-Mag",
            // ------------------
            "Label"
		};

		public static string HeaderCsv => string.Join(",", Header);

        public bool IsValid => (MeanMag != 0.0M && 
                                VarianceMag != 0.0M && 
                                SkewnessMag != 0.0M && 
                                KurtosisMag != 0.0M && 
                                RMSMag != 0.0M);

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
                    MeanMag.ToString(),
                    // ----------------------------------------
                    Variance[0].ToString(),
                    Variance[1].ToString(),
                    Variance[2].ToString(),
                    VarianceMag.ToString(),
                    // ----------------------------------------
                    Skewness[0].ToString(),
                    Skewness[1].ToString(),
					Skewness[2].ToString(),
                    SkewnessMag.ToString(),
                    // ----------------------------------------
                    Kurtosis[0].ToString(),
                    Kurtosis[1].ToString(),
					Kurtosis[2].ToString(),
                    KurtosisMag.ToString(),
                    // ----------------------------------------
                    RMS[0].ToString(),
                    RMS[1].ToString(),
					RMS[2].ToString(),
                    RMSMag.ToString(),
                    // ----------------------------------------
                    Label
                };
            }
        }

        /// <summary>
        /// Tos the csv.
        /// </summary>
        /// <returns>The csv.</returns>
        public string CsvString => string.Join(",", CsvArray);
    }
}