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
						ContainedPauses &&
						WasSingleTrajectory);
			}
		}

        public decimal Duration { get; set; }
        public int TaskStartRecordNum { get; set; }
        public int MovementStartRecordNum { get; set; }
        public bool StartedImmediately
        {
            get
            {
                return (TaskStartRecordNum == MovementStartRecordNum);
            }
        }

		public int NumOfPauses { get; set; }
		public bool ContainedPauses
        {
            get
            {
                return (NumOfPauses != 0);
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
                MovementStartRecordNum.ToString(),
                NumOfPauses.ToString(),
                AccelXStdDev.ToString(),
                AccelYStdDev.ToString(),
                AccelZStdDev.ToString(),
                StartedImmediately.ToString(),
                ContainedPauses.ToString(),
                WasSingleTrajectory.ToString(),
                WasNormal.ToString()
			};

			return string.Join(",", props);
        }
    }
}