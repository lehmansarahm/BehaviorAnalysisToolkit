using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Constants;

namespace BAT.Core.Summarizers
{
    public class ForwardReachSummarizer : ISummarizer
    {
        static Dictionary<string, SelectSummary> SelectSummaries { get; set; }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>The header.</returns>
        public string[] Header => ForwardReachOutput.SummaryHeader;

        /// <summary>
        /// Gets the header csv.
        /// </summary>
        /// <returns>The header csv.</returns>
        public string HeaderCsv => ForwardReachOutput.SummaryHeaderCsv;

        /// <summary>
        /// Gets the footer labels.
        /// </summary>
        /// <returns>The footer labels.</returns>
        public string[] FooterLabels => new string[] { };

        /// <summary>
        /// Gets the footer values.
        /// </summary>
        /// <returns>The footer values.</returns>
        public string[] FooterValues => new string[] { };

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
            if (SelectSummaries == null) SelectSummaries = new Dictionary<string, SelectSummary>();
            foreach (var key in InputData.Keys)
            {
                if (!SelectSummaries.ContainsKey(key) || SelectSummaries[key] == null)
                    SelectSummaries[key] = new SelectSummary();

                SelectSummaries[key].SelectTasks = InputData[key]
                    .Where(x => x.Label.Contains("select"))
                    .GroupBy(x => x.Label)
                    .ToDictionary(x => x.First().Label, x => x.First().RecordNum);
            }
        }

        /// <summary>
        /// Summarize the specified input.
        /// </summary>
        /// <returns>The summarize.</returns>
        /// <param name="input">Input.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IEnumerable<string[]> Summarize<T>(Dictionary<string, IEnumerable<T>> input) where T : ICsvWritable
        {
            var results = new List<string[]> { };

            foreach (var inputKey in input.Keys)
            {
                var reachResults = (IEnumerable<ForwardReachResult>)input[inputKey];
                if (!SelectSummaries.ContainsKey(inputKey) || SelectSummaries[inputKey] == null)
                    SelectSummaries[inputKey] = new SelectSummary();
                if (reachResults != null && reachResults.Any())
                    SelectSummaries[inputKey].ForwardReachStartNums = reachResults
                        .Select(x => UtilityService.GetMinInt(x.FirstShiftRecordNum[0], x.FirstShiftRecordNum[1]).Value).ToList();
            }

            foreach (var summary in SelectSummaries)
            {
                var source = summary.Key;
                var selectTasks = summary.Value.SelectTasks;
                var reachNums = summary.Value.ForwardReachStartNums;

                var selectTaskCount = selectTasks.Keys.Count();
                var reachCount = reachNums.Count();
                var count = UtilityService.GetMaxInt(selectTaskCount, reachCount);

                var firstLinePrinted = false;
                for (int i = 0; i < count; i++)
                {
                    var selectTaskName = selectTaskCount > i ? selectTasks.Keys.ElementAt(i) : "N/A";
                    var selectTaskStartNum = selectTaskCount > i ? selectTasks[selectTaskName].ToString() : "N/A";
                    var reachStartNum = reachCount > i ? reachNums[i].ToString() : "N/A";

                    results.Add(new string[]
                    {
                        !firstLinePrinted ? source : "N/A",
                        selectTaskName,
                        selectTaskStartNum,
                        reachStartNum,
                        summary.Value.IsMatch(reachStartNum).ToString()
                    });

                    firstLinePrinted = true;
                }
            }

            return results;
        }
    }

    class SelectSummary
	{
        //static int MARGIN_TIME = 2; /
		static int MARGIN = 40;

        public Dictionary<string, int> SelectTasks = new Dictionary<string, int>();
        public List<int> ForwardReachStartNums = new List<int>();

        public bool IsMatch(string reachStartNum)
        {
            var parseSuccess = int.TryParse(reachStartNum, out int reachStartVal);
            if (!parseSuccess) return false;
            return SelectTasks.Any(x => System.Math.Abs(x.Value - reachStartVal) < MARGIN);
        }
    }
}