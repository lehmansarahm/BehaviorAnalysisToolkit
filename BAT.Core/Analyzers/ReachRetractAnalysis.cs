using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Analyzers
{
    public class ReachRetractAnalysis : IAnalyzer
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
        public string[] GetHeader() { return ReachRetractOutput.ResultHeader; }

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv() { return ReachRetractOutput.ResultHeaderCsv; }

        /// <summary>
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
                                                 IEnumerable<Parameter> parameters)
        {
            var results = new List<RetractResult>();
            foreach (var param in parameters.Where(x => x.Field.Equals("AccelX")))
            {
                var threshold = Decimal.Parse(param.GetClauseValue(CommandParameters.Threshold));
                var windowSize = Int32.Parse(param.GetClauseValue(CommandParameters.Window));

                var windowRecords = new List<SensorReading>();
                foreach (var record in input)
                {
                    if (windowRecords.Count < windowSize && record.HasValidAccelVector)
                        windowRecords.Add(record);

                    if (windowRecords.Count == windowSize)
                    {
                        SensorReading[] firstPeak = null, secondPeak = null;
						bool aboveThreshold = false;
						
                        // we have a full window ... check to see if we have two threshold peaks
						foreach (var windowRecord in windowRecords)
                        {
                            if (windowRecord.AccelX > threshold)
							{
								if (!aboveThreshold && firstPeak == null)
								{
									firstPeak = new SensorReading[2];
									firstPeak[0] = windowRecord;
								}
								else if (!aboveThreshold && secondPeak == null)
								{
									secondPeak = new SensorReading[2];
									secondPeak[0] = windowRecord;
								}
                                aboveThreshold = true;
                            }
                            else
							{
                                if (aboveThreshold && firstPeak[1] == null)
									firstPeak[1] = windowRecord;
								else if (aboveThreshold && secondPeak[1] == null)
									secondPeak[1] = windowRecord;
                                aboveThreshold = false;
                            }
                        }

                        if (firstPeak?[0] != null && secondPeak?[1] != null)
						{
                            // if match found, add match to results
                            results.Add(new RetractResult
                            {
                                Start = firstPeak[0].Time,
                                StartNum = firstPeak[0].RecordNum,
                                End = secondPeak[1].Time,
                                EndNum = secondPeak[1].RecordNum,
                                WasGrab = false
                            });

							// then remove first 50% of window and fill again
							for (int i = 0; i < (windowSize / 2); i++)
                            {
                                windowRecords.RemoveAt(i);
                            }
						}
                        else
						{
                            // slide window by a single record and try again
                            windowRecords.RemoveAt(0);
						}
                    }
                }
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
            return data.Values.SelectMany(x => (List<RetractResult>)x).ToList();
		}
    }
}