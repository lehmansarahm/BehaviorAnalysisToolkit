using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
    public class SelectResult : ICsvWritable
	{
		public string Label { get; set; }
		public bool WasNormal
		{
			get
			{
				return (StartedImmediately &&
						WasContinuous &&
						WasSingleTrajectory);
			}
		}

        public decimal Duration { get; set; }
		public int TaskStartRecordNum { get; set; }
        public IEnumerable<PauseResult> Pauses { get; set; }
		public bool StartedImmediately
		{
			get
			{
                var firstPause = Pauses.FirstOrDefault();
                if (firstPause == null) return true;
                return (firstPause.StartNum != TaskStartRecordNum);
			}
		}
		public bool WasContinuous
		{
			get
			{
                // either there are no pauses at all, or there is a single pause 
                // and it occurred right at the beginning (all movement after 
                // that point was continuous)
                return (!Pauses.Any() || (!StartedImmediately && Pauses?.Count() == 1));
			}
		}

		public decimal AccelXStdDev { get; set; }
		public decimal AccelYStdDev { get; set; }
		public decimal AccelZStdDev { get; set; }
		public decimal StdDevThreshold { get; set; }
        public bool WasSingleTrajectory
        {
            get
            {
                return (AccelXStdDev <= StdDevThreshold &&
                        AccelYStdDev <= StdDevThreshold &&
                        AccelZStdDev <= StdDevThreshold);
            }
        }

        /// <summary>
        /// Tos the csv array.
        /// </summary>
        /// <returns>The csv array.</returns>
        public string[] CsvArray
        {
            get
            {
                return new string[] {
                Label,
                Duration.ToString(),
                TaskStartRecordNum.ToString(),
                Pauses?.Count().ToString() ?? "0",                  // pause count
                Pauses?.Select(x => x.Duration)?.Sum().ToString(),  // time spent paused
                AccelXStdDev.ToString(),
                AccelYStdDev.ToString(),
                AccelZStdDev.ToString(),
                StartedImmediately.ToString(),
                WasContinuous.ToString(),
                WasSingleTrajectory.ToString(),
                WasNormal.ToString()
            };
            }
        }

        /// <summary>
        /// Tos the csv.
        /// </summary>
        /// <returns>The csv.</returns>
        public string CsvString
        {
            get
            {
                return string.Join(",", CsvArray);
            }
        }
    }
}