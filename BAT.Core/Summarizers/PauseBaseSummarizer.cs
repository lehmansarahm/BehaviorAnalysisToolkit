using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Constants;

namespace BAT.Core.Summarizers
{
	public class PauseBaseSummarizer : ISummarizer
	{
		List<decimal> durations;

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>The header.</returns>
        public string[] Header => PauseOutput.SummaryHeader;

        /// <summary>
        /// Gets the header csv.
        /// </summary>
        /// <returns>The header csv.</returns>
        public string HeaderCsv => PauseOutput.SummaryHeaderCsv;

        /// <summary>
        /// Gets the footer labels.
        /// </summary>
        /// <returns>The footer labels.</returns>
        public string[] FooterLabels => PauseOutput.SummaryFooter;

        /// <summary>
        /// Gets the footer values.
        /// </summary>
        /// <returns>The footer values.</returns>
        public string[] FooterValues => new string[] {
                    "",
                    $"{durations.Count}",
                    $"{UtilityService.Total(durations)}",
                    $"{UtilityService.Average(durations)}"
                };

		/// <summary>
		/// Gets the footer csv.
		/// </summary>
		/// <returns>The footer csv.</returns>
		public string FooterCsv => PauseOutput.SummaryFooterCsv;

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
			durations = new List<decimal>();

			foreach (var key in input.Keys)
			{
                if (input[key] is List<PauseResult>)
				{
					List<PauseResult> analysisResults = (List<PauseResult>)input[key];
					var sourceDurations = analysisResults.Select(x => x.Duration).ToList();
                    durations.AddRange(sourceDurations);
                    results.Add(new string[] {
                        key,                                            // source
                        $"{sourceDurations.Count}",                     // num of pauses
                        $"{UtilityService.Total(sourceDurations)}",     // total time paused
                        $"{UtilityService.Average(sourceDurations)}"    // average time paused
                    });
                }
			}

			return results;
		}
	}
}