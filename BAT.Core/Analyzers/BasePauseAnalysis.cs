using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Analyzers
{
    public abstract class BasePauseAnalysis : IAnalyzer
	{
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>The header.</returns>
        public string[] Header => PauseOutput.ResultHeader;

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
        public string HeaderCsv => PauseOutput.ResultHeaderCsv;

        /// <summary>
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public abstract IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
														  IEnumerable<Parameter> parameters);

		/// <summary>
		/// Consolidates the data.
		/// </summary>
		/// <returns>The data.</returns>
		/// <param name="data">Data.</param>
		public IEnumerable<ICsvWritable> ConsolidateData(Dictionary<string, IEnumerable<ICsvWritable>> data)
		{
			return data.Values.SelectMany(x => (List<PauseResult>)x).ToList();
		}
    }
}