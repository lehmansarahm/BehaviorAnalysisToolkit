using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Analyzers
{
	public class SelectSimpsonAnalysis : IAnalyzer
	{
        const int X_INDEX = 0, Y_INDEX = 1, Z_INDEX = 2;

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

			var results = new List<SelectResult>();
            foreach (var inputGroup in inputGroups)
			{
                // converting accel to velocity
                List<decimal>[] velocities =
                {
                    MathService.SimpsonsRuleIntegral(inputGroup.Select(x => x.AccelX).ToList()),
					MathService.SimpsonsRuleIntegral(inputGroup.Select(x => x.AccelY).ToList()),
					MathService.SimpsonsRuleIntegral(inputGroup.Select(x => x.AccelZ).ToList())
                };

				// converting velocity to position
				List<decimal>[] positions =
				{
					MathService.SimpsonsRuleIntegral(velocities[X_INDEX]),
					MathService.SimpsonsRuleIntegral(velocities[Y_INDEX]),
					MathService.SimpsonsRuleIntegral(velocities[Z_INDEX])
				};

                // generating a result
                var result = new SelectResult
                {
                    Label = inputGroup.First().Label,
                    Duration = ((inputGroup.Last().RecordNum -
                                inputGroup.First().RecordNum) *
                                Constants.BAT.SAMPLING_PERIOD_IN_SEC),
					TaskStartRecordNum = inputGroup.First().RecordNum,
					Pauses = PauseDurationAnalysis.EvaluatePause(inputGroup,
																 CommandParameters.InstantaneousSpeed,
																 pauseThreshold, pauseWindow),
                    AccelXStdDev = positions[X_INDEX].Last(),
                    AccelYStdDev = positions[Y_INDEX].Last(), 
                    AccelZStdDev = positions[Z_INDEX].Last(), 
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