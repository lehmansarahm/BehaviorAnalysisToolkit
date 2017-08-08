using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Constants;

namespace BAT.Core.Summarizers
{
    public class ReachRetractSummarizer : ISummarizer
    {
        public static Dictionary<string, int> SelectTaskCounts = new Dictionary<string, int>();
        List<int> reachGrab, reachNoTouch;

		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
        public string[] GetHeader() { return ReachRetractOutput.SummaryHeader; }

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv() { return ReachRetractOutput.SummaryHeaderCsv; }

		/// <summary>
		/// Gets the footer labels.
		/// </summary>
		/// <returns>The footer labels.</returns>
		public string[] GetFooterLabels() { return ReachRetractOutput.SummaryFooter; }

		/// <summary>
		/// Gets the footer values.
		/// </summary>
		/// <returns>The footer values.</returns>
		public string[] GetFooterValues()
		{
			return new string[]
            {
                "", // source
                reachGrab.Sum(x => x).ToString(), 
                "", // select task count
                reachNoTouch.Sum(x => x).ToString()
            };
		}

		/// <summary>
		/// Gets the footer csv.
		/// </summary>
		/// <returns>The footer csv.</returns>
		public string GetFooterCsv() { return ReachRetractOutput.SummaryFooterCsv; }

		/// <summary>
		/// Summarize the specified input.
		/// </summary>
		/// <returns>The summarize.</returns>
		/// <param name="input">Input.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public IEnumerable<string[]> Summarize<T>(Dictionary<string, IEnumerable<T>> input) where T : ICsvWritable
		{
			var results = new List<string[]>();
			reachGrab = new List<int>();
            reachNoTouch = new List<int>();

			foreach (var key in input.Keys)
			{
                if (input[key] is List<RetractResult>)
				{
                    var analysisResults = ((List<RetractResult>)input[key]);
                    var grabCount = analysisResults.Where(x => x.WasGrab).Count();
                    var noTouchCount = analysisResults.Where(x => !x.WasGrab).Count();

                    var selectTaskCount = 0;
                    SelectTaskCounts.TryGetValue(key, out selectTaskCount);

					results.Add(new string[] {
						key,                    // source
                        $"{grabCount}",         // num of grab instances
                        $"{selectTaskCount}",   // number of "select" tasks for this input
                        $"{noTouchCount}"       // num of no-touch instances
                    });

					reachGrab.Add(grabCount);
					reachNoTouch.Add(noTouchCount);
				}
			}

			return results;
		}
    }
}