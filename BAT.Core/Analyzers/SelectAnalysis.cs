using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Analyzers
{
	public class SelectAnalysis : IAnalyzer
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
        public string[] Header => SelectOutput.ResultHeader;

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
        public string HeaderCsv => SelectOutput.ResultHeaderCsv;

        /// <summary>
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
                                                 IEnumerable<Parameter> parameters)
		{
            // first, split records according to distinct labels containing the word "select"
            var inputGroups = input.Where(x => x.Label.Contains("select")).GroupBy(x => x.Label);
            if (inputGroups == null || !inputGroups.Any()) 
                return new List<SelectResult>();

            // next, parse out the parameters we need
            var pauseCommand = parameters.Where(x => 
                x.Field.Equals(CommandParameters.InstantaneousSpeed)).FirstOrDefault();
            var pauseThreshold = decimal.Parse(pauseCommand.Clauses.Where(x => 
                x.Key.Equals(CommandParameters.Threshold)).FirstOrDefault().Value);
            var pauseWindow = int.Parse(pauseCommand.Clauses.Where(x => 
                x.Key.Equals(CommandParameters.Window)).FirstOrDefault().Value);

            var varianceCommand = parameters.Where(x => 
                x.Field.Equals(CommandParameters.Acceleration)).FirstOrDefault();
            var varianceThreshold = varianceCommand.Clauses.Where(x => 
                x.Key.Equals(CommandParameters.Variance)).FirstOrDefault().Value;

			// next, for each group with a distinct label, calculate:
			//      total task time
			//      num of first record (when task starts)
			//      num of first record when user starts moving
			//      count of significant pauses
			//      accel-x std. dev.
			//      accel-y std. dev.
			//      accel-z std. dev.
			var results = new List<SelectResult>();
            foreach (var inputGroup in inputGroups)
			{
				// generating a result
				var result = new SelectResult
                {
                    Label = inputGroup.First().Label,
                    Duration = ((inputGroup.Last().RecordNum -
                                inputGroup.First().RecordNum) *
                                Constants.BAT.SAMPLING_PERIOD_IN_MS) / 1000.0M,
                    TaskStartRecordNum = inputGroup.First().RecordNum,
                    Pauses = PauseDurationAnalysis.EvaluatePause(inputGroup, 
                                                                 CommandParameters.InstantaneousSpeed, 
                                                                 pauseThreshold, pauseWindow),
					AccelXStdDev = UtilityService.StandardDeviation(inputGroup.Select(x => x.AccelX)),
					AccelYStdDev = UtilityService.StandardDeviation(inputGroup.Select(x => x.AccelY)),
					AccelZStdDev = UtilityService.StandardDeviation(inputGroup.Select(x => x.AccelZ)),
                    StdDevThreshold = decimal.Parse(varianceThreshold)
                };
                results.Add(result);
			}

            // finally, return the results (one record per input group)
			return results;
		}

        /// <summary>
        /// Consolidates the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
		public IEnumerable<ICsvWritable> ConsolidateData(Dictionary<string, IEnumerable<ICsvWritable>> data)
		{
            return data.Values.SelectMany(x => (List<SelectResult>)x).ToList();
		}
	}
}