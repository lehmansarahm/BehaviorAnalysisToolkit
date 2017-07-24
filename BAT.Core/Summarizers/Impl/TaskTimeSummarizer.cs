using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;

namespace BAT.Core.Summarizers.Impl
{
    public class TaskTimeSummarizer : ISummarizer
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
		public string[] GetHeader() { return Constants.TASK_TIME_SUMMARY_HEADER; }

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv() { return Constants.TASK_TIME_SUMMARY_HEADER_CSV; }

        /// <summary>
        /// Gets the type of the phase result.
        /// </summary>
        /// <value>The type of the phase result.</value>
        public Type PhaseResultType 
        {
            get { return typeof(TaskTimeResult); }
        }

        /// <summary>
        /// Summarize the specified input.
        /// </summary>
        /// <returns>The summarize.</returns>
        /// <param name="input">Input.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IEnumerable<KeyValuePair<string, string>> Summarize<T>(Dictionary<string, 
                                                                      IEnumerable<T>> input) where T : ICsvWritable
        {
            // if so, consolidate the task times by source
            var results = new List<KeyValuePair<string, string>>();
            foreach (var key in input.Keys)
            {
                List<TaskTimeResult> analysisResults = (List<TaskTimeResult>)input[key];
                var taskTimeResult = analysisResults.FirstOrDefault();
                if (taskTimeResult != null)
                    results.Add(new KeyValuePair<string, string>(key, taskTimeResult.Duration.ToString()));
            }

            return results;
        }
    }
}