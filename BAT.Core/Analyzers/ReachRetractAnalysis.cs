using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

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
					{
						LogManager.Debug($"\tCurrent window record count: {windowRecords.Count}" +
										 $"\n\tAdding new record num: {record.RecordNum}", this);
						windowRecords.Add(record);
					}

                    if (windowRecords.Count == windowSize)
					{
						LogManager.Debug("Window record collection is full", this);
                        var peaks = new List<SensorReading[]>();
                        SensorReading peakStart = null;
						var aboveThreshold = false;

                        // check to see if we have one or more peaks along the X axis
                        foreach (var windowRecord in windowRecords)
						{
                            if (windowRecord.AccelX > threshold)
                            {
                                peakStart = windowRecord;
                                aboveThreshold = true;
                            }
                            else
                            {
								if (aboveThreshold && peakStart != null)
									peaks.Add(new SensorReading[] { peakStart, windowRecord });
								aboveThreshold = false;
								peakStart = null;
                            }
                        }

                        // update match result set
                        if (peaks.Any()) AddMatches(results, peaks);

						// shift window and try again
						var initialWindowRecordCount = windowRecords.Count;
						windowRecords.RemoveAt(0);
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

        /// <summary>
        /// Adds the matches.
        /// </summary>
        /// <param name="peaks">Peaks.</param>
        void AddMatches(List<RetractResult> results, List<SensorReading[]> peaks)
		{
			var first = peaks.First()[0];
			var last = peaks.Last()[1];

			// input verification ...
            LogManager.Debug($"Peaks found: {peaks.Count}; logging new match " +
                             $"from start record num: {first.RecordNum} to " +
                             $"end record num: {last.RecordNum}", this);

            // check to see if dups of individual peaks exist (remove if true)
            for (int i = 0; i < peaks.Count; i++)
			{
                var peak = peaks[i];
				results.RemoveAll(x => x.StartNum == peak[0].RecordNum
                                  && x.EndNum == peak[1].RecordNum);
                for (int j = (i + 1); j < peaks.Count; j++)
                {
					var nextPeak = peaks[j];
					results.RemoveAll(x => x.StartNum == peak[0].RecordNum
                                      && x.EndNum == nextPeak[1].RecordNum);
                }
            }

            // add new match to results ...
            // if mult peaks exist in a single window, it's a reach-and-grab
            var newResult = (new RetractResult
            {
                Start = first.Time,
                StartNum = first.RecordNum,
                StartLabel = first.Label,
                End = last.Time,
                EndNum = last.RecordNum,
                EndLabel = last.Label,
                WasGrab = (peaks.Count > 1)
            });

            // only add this new result if it is NOT already encompassed by 
            // the start / end nums of a previously stored result
            if (!results.Where(x => x.StartNum <= newResult.StartNum 
                               && newResult.EndNum <= x.EndNum).Any())
                results.Add(newResult);

			// output match results ...
			string output = "Current matches found: " + results.Count;
			foreach (var result in results)
				output += $"\n\tStart record num: {result.StartNum}, " +
						  $"End record num: {result.EndNum}";
			LogManager.Debug(output, this);
        }
    }
}