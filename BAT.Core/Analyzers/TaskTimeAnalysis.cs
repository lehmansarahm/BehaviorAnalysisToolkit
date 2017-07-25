using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Analyzers
{
	public class TaskTimeAnalysis : IAnalyzer
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
        public string[] GetHeader() { return Constants.TASK_TIME_RESULT_HEADER; }

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
        public string GetHeaderCsv() { return Constants.TASK_TIME_RESULT_HEADER_CSV; }

        /// <summary>
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
                                                 IEnumerable<Parameter> parameters)
		{
			var first = input.FirstOrDefault();
            if (first == null || !first.HasValidTimeData)
                return new List<TaskTimeResult>();

            var numOfRecordings = input.Count() - 1;
            var execTimeInMs = numOfRecordings * Constants.SAMPLING_PERIOD_IN_MS;
            var execTimeInSec = execTimeInMs / 1000.0M;

            var last = input.LastOrDefault();
            var results = new List<TaskTimeResult>
            {
                new TaskTimeResult {
                    Start = first.Time.Value,
                    StartNum = first.RecordNum.Value,
                    End = last.Time.Value,
                    EndNum = last.RecordNum.Value,
                    Duration = execTimeInSec
                }
            };

			return results;
		}
	}
}