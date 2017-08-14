using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Analyzers
{
    public class MLDataPrepAnalysis : IAnalyzer
	{
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>The header.</value>
        public string[] Header => MLDataOutput.Header;

        /// <summary>
        /// Gets the header csv.
        /// </summary>
        /// <value>The header csv.</value>
        public string HeaderCsv => MLDataOutput.HeaderCsv;

        /// <summary>
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
										  IEnumerable<Parameter> parameters)
        {
            var results = new List<MLDataResult>();
            var inputGroups = input.GroupBy(x => x.Label);

            foreach (var inputGroup in inputGroups)
            {
                var accelX = inputGroup.Select(x => x.AccelX);
                var accelY = inputGroup.Select(x => x.AccelY);
                var accelZ = inputGroup.Select(x => x.AccelZ);

                results.Add(new MLDataResult
                {
                    Source = inputGroup.Key,
                    Mean = new decimal[]
					{
						accelX.Average(),
						accelY.Average(),
						accelZ.Average()
                    },
                    Variance = new decimal[]
					{
						MathService.GetVariance(accelX.ToList()),
						MathService.GetVariance(accelY.ToList()),
						MathService.GetVariance(accelZ.ToList())
                    },
                    Skewness = new decimal[]
					{
						MathService.GetSkewness(accelX.ToList()),
						MathService.GetSkewness(accelY.ToList()),
						MathService.GetSkewness(accelZ.ToList())
                    },
                    Kurtosis = new decimal[]
					{
                        MathService.GetKurtosis(accelX.ToList()),
						MathService.GetKurtosis(accelY.ToList()),
						MathService.GetKurtosis(accelZ.ToList())
                    },
                    RMS = new decimal[]
					{
                        MathService.GetRMS(accelX.ToList()),
						MathService.GetRMS(accelY.ToList()),
						MathService.GetRMS(accelZ.ToList())
                    }
                });
            }

            return results;
        }

        /// <summary>
        /// Consolidates the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
		public IEnumerable<ICsvWritable> ConsolidateData(Dictionary<string, IEnumerable<ICsvWritable>> data)
        {
            return null;
        }
    }
}