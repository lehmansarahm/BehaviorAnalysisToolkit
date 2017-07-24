using System;
using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Analyzers.Impl
{
    public class PauseCountAnalysis : IAnalyzer
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
		public string[] GetHeader()
		{
            return PauseResult.Header;
		}

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv()
		{
			return PauseResult.HeaderCsv;
		}

		/// <summary>
		/// Analyze the specified input and parameters.
		/// </summary>
		/// <returns>The analyze.</returns>
		/// <param name="input">Input.</param>
		/// <param name="parameters">Parameters.</param>
		public IEnumerable<ICsvWritable> Analyze(IEnumerable<ICsvWritable> input,
                                                IEnumerable<Parameter> parameters)
		{
            var results = new List<PauseResult>();
			foreach (var param in parameters)
			{
                var rawThreshold = param.GetClauseValue(Constants.COMMAND_PARAM_THRESHOLD);
                var threshold = Double.Parse(rawThreshold);

                var rawWindowSize = param.GetClauseValue(Constants.COMMAND_PARAM_WINDOW);
                var windowSize = Int32.Parse(rawWindowSize);

                DateTime startTime = DateTime.Now, endTime;
                bool currentlyPaused = false;
                int startNo = 0, endNo, windowCount = 0, pauseCount = 0;
                double currentDuration = 0.0d, totalDuration = 0.0d;

				foreach (var inputItem in input)
				{
                    if (inputItem.GetType() != typeof(SensorReading)) continue;
                    var record = (SensorReading)inputItem;

					var filterField = record.GetType().GetProperty(param.Field);
					if (filterField == null) continue;

                    var filterValue = Double.Parse(filterField.GetValue(record, null).ToString());
                    if (record.HasValidAccelVector && filterValue < threshold)
					{
						// we've found a pause instance
						if (!currentlyPaused)
						{
                            // new pause instance ... log the starting details
                            startTime = record.Time.Value;
                            startNo = record.RecordNum.Value;
							currentlyPaused = true;
						}
						// whether starting a new pause or continuing an old, bump the pause counter
						windowCount++;
					}
					else
					{
						// measurement denotes active movement ... reset pause details
						currentlyPaused = false;
						windowCount = 0;
					}

					// check to see if we've completed a pause window
                    if (currentlyPaused && windowCount == windowSize)
					{
						// log ending details
						endTime = record.Time.Value;
						endNo = record.RecordNum.Value;

						// determine the current duration and add to our running total
						currentDuration = windowCount * Constants.SAMPLING_PERIOD;
                        totalDuration += currentDuration;

                        // output pause details to file
                        var pauseResult = new PauseResult
                        {
                            Start = startTime,
                            StartNum = startNo,
                            End = endTime,
                            EndNum = endNo,
                            Duration = currentDuration
                        };
                        results.Add(pauseResult);
                        pauseCount++;

						// reset pause details
						currentlyPaused = false;
						windowCount = 0;
					}
				}
			}

			return results;
        }
    }
}