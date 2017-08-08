using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Constants;

namespace BAT.Core.Summarizers
{
    public class SelectSummarizer : ISummarizer
	{
		List<decimal> avgDurs, minDurs, maxDurs;
		List<int> taskCounts, normalCounts, abnormalCounts;

		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
        public string[] GetHeader() { return SelectOutput.SummaryHeader; }

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv() { return SelectOutput.SummaryHeaderCsv; }

        /// <summary>
        /// Gets the footer labels.
        /// </summary>
        /// <returns>The footer labels.</returns>
        public string[] GetFooterLabels() { return SelectOutput.SummaryFooter; }

        /// <summary>
        /// Gets the footer values.
        /// </summary>
        /// <returns>The footer values.</returns>
        public string[] GetFooterValues()
		{
			return new string[] {
				"",
				taskCounts.Any() ? taskCounts.Average().ToString() : "N/A",
				avgDurs.Any() ? avgDurs.Average().ToString() : "N/A",
				minDurs.Any() ? minDurs.Average().ToString() : "N/A",
				maxDurs.Any() ? maxDurs.Average().ToString() : "N/A",
				normalCounts.Any() ? normalCounts.Average().ToString() : "N/A",
				abnormalCounts.Any() ? abnormalCounts.Average().ToString() : "N/A"
			};
        }

        /// <summary>
        /// Gets the footer csv.
        /// </summary>
        /// <returns>The footer csv.</returns>
		public string GetFooterCsv() { return SelectOutput.SummaryFooterCsv; }

        /// <summary>
        /// Summarize the specified input.
        /// </summary>
        /// <returns>The summarize.</returns>
        /// <param name="input">Input.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IEnumerable<string[]> Summarize<T>(Dictionary<string, IEnumerable<T>> input) where T : ICsvWritable
        {
			var results = new List<string[]>();
			avgDurs = new List<decimal>();
			minDurs = new List<decimal>();
			maxDurs = new List<decimal>();

			taskCounts = new List<int>();
			normalCounts = new List<int>();
            abnormalCounts = new List<int>();

            foreach (var key in input.Keys)
			{
                if (input[key] is List<SelectResult>)
                {
                    var analysisResults = (List<SelectResult>)input[key];
                    if (analysisResults.Count == 0)
                        results.Add(new string[] { key, "0", "N/A", "N/A", "N/A", "N/A", "N/A" });
                    else
					{
                        var taskCount = analysisResults.Count;
                        taskCounts.Add(taskCount);

						var analysisDurations = analysisResults.Select(x => x.Duration);
                        var avgDur = analysisDurations.Average();
                        avgDurs.Add(avgDur);

                        var minDur = analysisDurations.Min();
                        minDurs.Add(minDur);

                        var maxDur = analysisDurations.Max();
                        maxDurs.Add(maxDur);

						var normalCount = analysisResults.Where(x => x.WasNormal).Count();
                        normalCounts.Add(normalCount);

						var abnormalCount = analysisResults.Where(x => !x.WasNormal).Count();
                        abnormalCounts.Add(abnormalCount);

						results.Add(new string[]
						{
							key,
                            taskCount.ToString(),
							avgDur.ToString(),
							minDur.ToString(),
							maxDur.ToString(),
                            normalCount.ToString(),
                            abnormalCount.ToString()
						});
                    }
                }
            }

            return results;
        }
    }
}