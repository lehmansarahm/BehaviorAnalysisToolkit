using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
    public class SciKitResult : ICsvWritable
	{
		public string Source { get; set; }

        public IEnumerable<SensorReading> Data { get; set; }

		public SciKitFeatureVector[] FeatureVectors { get; set; }

		public int Label { get; set; }

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

		/// <summary>
		/// Tos the csv array.
		/// </summary>
		/// <returns>The csv array.</returns>
		public string[] CsvArray => new string[] {
					Source,
                    // ----------------------------------------
                    FeatureVectors[0].Mean.ToString(),
					FeatureVectors[1].Mean.ToString(),
					FeatureVectors[2].Mean.ToString(),
					MeanMag.ToString(),
                    // ----------------------------------------
                    FeatureVectors[0].Variance.ToString(),
					FeatureVectors[1].Variance.ToString(),
					FeatureVectors[2].Variance.ToString(),
					VarianceMag.ToString(),
                    // ----------------------------------------
                    FeatureVectors[0].Skewness.ToString(),
					FeatureVectors[1].Skewness.ToString(),
					FeatureVectors[2].Skewness.ToString(),
					SkewnessMag.ToString(),
                    // ----------------------------------------
                    FeatureVectors[0].Kurtosis.ToString(),
					FeatureVectors[1].Kurtosis.ToString(),
					FeatureVectors[2].Kurtosis.ToString(),
					KurtosisMag.ToString(),
                    // ----------------------------------------
                    FeatureVectors[0].RMS.ToString(),
					FeatureVectors[1].RMS.ToString(),
					FeatureVectors[2].RMS.ToString(),
					RMSMag.ToString(),
                    // ----------------------------------------
                    Label.ToString()
				};

		/// <summary>
		/// Tos the csv.
		/// </summary>
		/// <returns>The csv.</returns>
		public string CsvString => string.Join(",", CsvArray);

        public bool IsValid => (MeanMag != 0.0M && 
                                VarianceMag != 0.0M && 
                                SkewnessMag != 0.0M && 
                                KurtosisMag != 0.0M && 
                                RMSMag != 0.0M);

        decimal MeanMag => MathService.GetMagnitude(FeatureVectors[0].Mean, 
                                                    FeatureVectors[1].Mean, 
                                                    FeatureVectors[2].Mean);
        
        decimal VarianceMag => MathService.GetMagnitude(FeatureVectors[0].Variance, 
                                                        FeatureVectors[1].Variance, 
                                                        FeatureVectors[2].Variance);

        decimal SkewnessMag => MathService.GetMagnitude(FeatureVectors[0].Skewness, 
                                                        FeatureVectors[1].Skewness, 
                                                        FeatureVectors[2].Skewness);

        decimal KurtosisMag => MathService.GetMagnitude(FeatureVectors[0].Kurtosis, 
                                                        FeatureVectors[1].Kurtosis, 
                                                        FeatureVectors[2].Kurtosis);

        decimal RMSMag => MathService.GetMagnitude(FeatureVectors[0].RMS, 
                                                   FeatureVectors[1].RMS, 
                                                   FeatureVectors[2].RMS);
    }
}