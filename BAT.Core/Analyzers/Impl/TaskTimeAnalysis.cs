using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Analyzers.Impl
{
	public class TaskTimeAnalysis : IAnalyzer
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
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
                                                 IEnumerable<Parameter> parameters)
		{
			return null;
		}
	}
}