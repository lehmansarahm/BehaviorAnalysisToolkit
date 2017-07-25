using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;

namespace BAT.Core.Summarizers.Impl
{
	public class PauseCountSummarizer : ISummarizer
	{
		List<double> durations;

		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
		public string[] GetHeader() { return Constants.PAUSE_SUMMARY_HEADER; }

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv() { return Constants.PAUSE_SUMMARY_HEADER_CSV; }

		/// <summary>
		/// Gets the footer labels.
		/// </summary>
		/// <returns>The footer labels.</returns>
		public string[] GetFooterLabels()
		{
			return new string[] { };
		}

		/// <summary>
		/// Gets the footer values.
		/// </summary>
		/// <returns>The footer values.</returns>
		public string[] GetFooterValues()
		{
			return new string[] { };
		}

		/// <summary>
		/// Gets the footer csv.
		/// </summary>
		/// <returns>The footer csv.</returns>
		public string GetFooterCsv() { return Constants.TASK_TIME_SUMMARY_FOOTER_CSV; }

		/// <summary>
		/// Summarize the specified input.
		/// </summary>
		/// <returns>The summarize.</returns>
		/// <param name="input">Input.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public IEnumerable<string[]> Summarize<T>(Dictionary<string, IEnumerable<T>> input) where T : ICsvWritable
		{
			var results = new List<string[]>();
			durations = new List<double>();

			foreach (var key in input.Keys)
			{
                if (input[key] is List<PauseResult>)
				{
					List<PauseResult> analysisResults = (List<PauseResult>)input[key];
					durations.AddRange(analysisResults.Select(x => x.Duration).ToList());
					results.AddRange(analysisResults.Select(x => new string[] {
					key, x.Duration.ToString()}).ToList());
                }
			}

			return results;
		}
	}
}