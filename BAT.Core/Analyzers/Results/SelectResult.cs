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
		public int? FirstPauseRecordNum { get; set; }
		public bool WasContinuous
        {
            get
            {
                return (!FirstPauseRecordNum.HasValue);
            }
		}
		public bool StartedImmediately
		{
			get
			{
				return (!FirstPauseRecordNum.HasValue ||
						TaskStartRecordNum != FirstPauseRecordNum.Value);
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
        /// Tos the csv.
        /// </summary>
        /// <returns>The csv.</returns>
        public string ToCsv()
		{
			string[] props = {
                Label,
                Duration.ToString(),
                TaskStartRecordNum.ToString(),
                FirstPauseRecordNum.HasValue 
                    ? FirstPauseRecordNum.Value.ToString() 
                    : "N/A",
                FirstPauseRecordNum.ToString(),
                AccelXStdDev.ToString(),
                AccelYStdDev.ToString(),
                AccelZStdDev.ToString(),
                StartedImmediately.ToString(),
                WasContinuous.ToString(),
                WasSingleTrajectory.ToString(),
                WasNormal.ToString()
			};

			return string.Join(",", props);
        }
    }
}