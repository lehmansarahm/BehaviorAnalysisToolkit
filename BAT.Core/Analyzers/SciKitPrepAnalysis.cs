﻿using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Analyzers
{
    public class SciKitPrepAnalysis : IAnalyzer
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
			var labelParam = parameters.FirstOrDefault(x => x.Field.Equals(CommandParameters.Label));
            if (labelParam == null) return results;

            var labelValue = labelParam.GetClauseValue(CommandParameters.Contains);
            if (string.IsNullOrEmpty(labelValue)) return results;
            var labelValues = labelValue.Split(',');

			var inputGroups = input.GroupBy(x => x.Label);
			foreach (var inputGroup in inputGroups)
			{
				var newResult = (new SciKitResult
				{
                    Source = $"{CurrentInput}_{inputGroup.Key}",
                    FeatureVectors = new SciKitFeatureVector[]
					{
						new SciKitFeatureVector(inputGroup.Select(x => x.AccelX)),
						new SciKitFeatureVector(inputGroup.Select(x => x.AccelY)),
						new SciKitFeatureVector(inputGroup.Select(x => x.AccelZ))
					},
                    Label = GetNumericLabel(inputGroup.Key, labelValues) 
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

        /// <summary>
        /// Gets the numeric label.
        /// </summary>
        /// <returns>The numeric label.</returns>
        /// <param name="readingLabel">Key.</param>
        /// <param name="keywords">Label values.</param>
        static int GetNumericLabel(string readingLabel, string[] keywords)
		{
			var keywordList = keywords.ToList();
            var matchinglabel = keywordList.FirstOrDefault(readingLabel.StartsWith);

			if (string.IsNullOrEmpty(matchinglabel)) return 0;
			return (keywordList.FindIndex(x => x.Equals(matchinglabel)) + 1);
        }
    }
}