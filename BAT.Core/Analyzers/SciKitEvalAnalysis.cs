using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Analyzers
{
    public class SciKitEvalAnalysis : IAnalyzer
	{
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>The header.</value>
        public string[] Header => SciKitResult.Header;

        /// <summary>
        /// Gets the header csv.
        /// </summary>
        /// <value>The header csv.</value>
        public string HeaderCsv => SciKitResult.HeaderCsv;

        /// <summary>
        /// Gets or sets the current input.
        /// </summary>
        /// <value>The current input.</value>
        public string CurrentInput { get; set; }

        /// <summary>
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
                                                 IEnumerable<Parameter> parameters)
        {
			var results = new List<SciKitResult>();
            var param = parameters.FirstOrDefault(x => x.Field.Equals(CommandParameters.All));
            if (param == null) return results;

            var windowSizeRaw = param.GetClauseValue(CommandParameters.Window);
            if (!int.TryParse(windowSizeRaw, out int windowSize)) return results;

            for (int i = 0; i < input.Count(); i += (windowSize / 2))
            {
                var inputGroup = (input.Skip(i).Take(windowSize));
				var newResult = (new SciKitResult
				{
                    Source = $"{CurrentInput}_{inputGroup.First().RecordNum}",
                    FeatureVectors = new SciKitFeatureVector[]
					{
						new SciKitFeatureVector(inputGroup.Select(x => x.AccelX)),
						new SciKitFeatureVector(inputGroup.Select(x => x.AccelY)),
						new SciKitFeatureVector(inputGroup.Select(x => x.AccelZ))
					}
				});

                if (newResult.IsValid) results.Add(newResult);
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
            return data.Values.SelectMany(x => (List<SciKitResult>)x).ToList();
        }
    }
}