﻿using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Constants;

namespace BAT.Core.Summarizers
{
    public class TaskTimeSummarizer : ISummarizer
	{
        List<decimal> durations = new List<decimal>();

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>The header.</returns>
        public string[] Header => TaskTimeOutput.SummaryHeader;

        /// <summary>
        /// Gets the header csv.
        /// </summary>
        /// <returns>The header csv.</returns>
        public string HeaderCsv => TaskTimeOutput.SummaryHeaderCsv;

        /// <summary>
        /// Gets the footer labels.
        /// </summary>
        /// <returns>The footer labels.</returns>
        public string[] FooterLabels => TaskTimeOutput.SummaryFooter;

        /// <summary>
        /// Gets the footer values.
        /// </summary>
        /// <returns>The footer values.</returns>
        public string[] FooterValues => new string[] {
                "",
                $"{durations.Count()}",
                $"{MathService.Total(durations)}",
                $"{MathService.Average(durations)}",
                $"{MathService.StandardDeviation(durations)}"
            };

		/// <summary>
		/// Gets the footer csv.
		/// </summary>
		/// <returns>The footer csv.</returns>
		public string FooterCsv => TaskTimeOutput.SummaryFooterCsv;

		/// <summary>
		/// Initialize the specified InputData.
		/// </summary>
		/// <returns>The initialize.</returns>
		/// <param name="InputData">Input data.</param>
		public void Initialize(Dictionary<string, IEnumerable<SensorReading>> InputData)
		{
			// do nothing
		}

        /// <summary>
        /// Summarize the specified input.
        /// </summary>
        /// <returns>The summarize.</returns>
        /// <param name="input">Input.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IEnumerable<string[]> Summarize<T>(Dictionary<string, IEnumerable<T>> input) where T : ICsvWritable
        {
            var results = new List<string[]>();

            foreach (var key in input.Keys)
			{
                if (input[key] is List<TaskTimeResult> analysisResults)
                {
                    var currentDurations = analysisResults.Select(x => x.Duration).ToList();
                    durations.AddRange(currentDurations);
                    results.Add(new string[] {
                        key,
                        currentDurations.Count().ToString(),
                        currentDurations.Sum().ToString()
                    });
                }
            }

            return results;
        }
    }
}