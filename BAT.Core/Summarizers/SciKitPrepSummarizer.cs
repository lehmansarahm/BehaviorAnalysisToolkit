using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;

namespace BAT.Core.Summarizers
{
    public class SciKitPrepSummarizer : ISummarizer
	{
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>The header.</returns>
        public string[] Header => SciKitResult.Header;

        /// <summary>
        /// Gets the header csv.
        /// </summary>
        /// <returns>The header csv.</returns>
        public string HeaderCsv => SciKitResult.HeaderCsv;

        /// <summary>
        /// Gets the footer labels.
        /// </summary>
        /// <returns>The footer labels.</returns>
        public string[] FooterLabels => new string[] {};

        /// <summary>
        /// Gets the footer values.
        /// </summary>
        /// <returns>The footer values.</returns>
        public string[] FooterValues => new string[] {};

		/// <summary>
		/// Gets the footer csv.
		/// </summary>
		/// <returns>The footer csv.</returns>
        public string FooterCsv => string.Empty;

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
                if (input[key] is List<SciKitResult>)
                {
                    List<SciKitResult> analysisResults = (List<SciKitResult>)input[key];
                    results.AddRange(analysisResults.Select(x => x.CsvArray).ToList());
                }
            }

            return results;
        }
    }
}