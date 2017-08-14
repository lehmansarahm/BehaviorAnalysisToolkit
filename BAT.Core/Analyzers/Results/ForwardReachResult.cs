using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
    public class ForwardReachResult : ICsvWritable
	{
        public const int X_INDEX = 0, Y_INDEX = 1;

		public int?[] FirstShiftRecordNum { get; set; }
		public int?[] SecondShiftRecordNum { get; set; }
		public int?[] ThirdShiftRecordNum { get; set; }

        // Duration of action from first to final shift
        public decimal Duration
        {
            get
			{
                // add one to include the last record of the shift
                return ((ThirdShiftRecordNum.Max().Value - FirstShiftRecordNum.Min().Value + 1)
                         * Constants.BAT.SAMPLING_PERIOD_IN_SEC);
            }
        }

        /// <summary>
        /// Instantiating a record from a list of shifts
        /// </summary>
        /// <param name="xShift">X shifts.</param>
        /// <param name="yShift">Y shifts.</param>
        public ForwardReachResult(MotionShift xShift, MotionShift yShift)
		{
			FirstShiftRecordNum = new int?[]
			{
				xShift.GetPhaseShiftPoint(0),
				yShift.GetPhaseShiftPoint(0)
			};

			SecondShiftRecordNum = new int?[]
			{
				xShift.GetPhaseShiftPoint(1),
				yShift.GetPhaseShiftPoint(1)
			};

			ThirdShiftRecordNum = new int?[]
			{
				xShift.GetPhaseShiftPoint(2),
				yShift.GetPhaseShiftPoint(2)
			};
		}

        /// <summary>
        /// Matches the specified other.
        /// </summary>
        /// <returns>The matches.</returns>
        /// <param name="other">Other.</param>
        public bool Matches(ForwardReachResult other)
        {
            return (FirstShiftRecordNum[X_INDEX].Equals(other.FirstShiftRecordNum[X_INDEX]) &&
					FirstShiftRecordNum[Y_INDEX].Equals(other.FirstShiftRecordNum[Y_INDEX]) && 
                    SecondShiftRecordNum[X_INDEX].Equals(other.SecondShiftRecordNum[X_INDEX]) &&
					SecondShiftRecordNum[Y_INDEX].Equals(other.SecondShiftRecordNum[Y_INDEX]) &&
                    ThirdShiftRecordNum[X_INDEX].Equals(other.ThirdShiftRecordNum[X_INDEX]) &&
					ThirdShiftRecordNum[Y_INDEX].Equals(other.ThirdShiftRecordNum[Y_INDEX]));
        }

        /// <summary>
        /// Tos the csv.
        /// </summary>
        /// <returns>The csv.</returns>
        public string ToCsv()
		{
			string[] props = {
				Convert.ToString(FirstShiftRecordNum[X_INDEX]),
				Convert.ToString(FirstShiftRecordNum[Y_INDEX]),
				Convert.ToString(SecondShiftRecordNum[X_INDEX]),
				Convert.ToString(SecondShiftRecordNum[Y_INDEX]),
				Convert.ToString(ThirdShiftRecordNum[X_INDEX]),
				Convert.ToString(ThirdShiftRecordNum[Y_INDEX]),
				Duration.ToString()
			};
			return string.Join(",", props);
        }
    }
}