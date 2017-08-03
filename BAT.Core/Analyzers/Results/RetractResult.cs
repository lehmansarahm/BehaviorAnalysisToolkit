using System;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
    public class RetractResult : ICsvWritable
	{
		public DateTime Start { get; set; }
        public int StartNum { get; set; }
        public string StartLabel { get; set; }

        public DateTime End { get; set; }
        public int EndNum { get; set; }
        public string EndLabel { get; set; }

        public decimal Duration
        {
            get
			{
                return ((EndNum - StartNum) * Constants.SAMPLING_PERIOD_IN_MS) / 1000.0M;
            }
        }
        public bool WasGrab { get; set; }

        /// <summary>
        /// Tos the csv.
        /// </summary>
        /// <returns>The csv.</returns>
        public string ToCsv()
		{
            string[] props = {
                Start.ToString(),
                StartNum.ToString(),
                StartLabel,
                End.ToString(),
                EndNum.ToString(),
                EndLabel,
				Duration.ToString(),
                WasGrab.ToString()
			};
			return string.Join(",", props);
        }
    }
}