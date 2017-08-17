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
        public string[] Header => SensorReading.Header;

        /// <summary>
        /// Gets the header csv.
        /// </summary>
        /// <value>The header csv.</value>
        public string HeaderCsv => SensorReading.HeaderCsv;

        /// <summary>
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
										  IEnumerable<Parameter> parameters)
        {
            var results = new List<SensorReading>();
            foreach (var record in input)
            {
                var newRecord = new SensorReading(record)
                {
                    Label = record.Label.Contains("select") ? "select" : "non-select"
                };
                results.Add(newRecord);
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
            return data.Values.SelectMany(x => (List<SensorReading>)x).ToList();
        }
    }
}