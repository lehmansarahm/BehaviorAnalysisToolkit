using System.Collections.Generic;
using System.Linq;

namespace BAT.Core.Common
{
    public class MotionShift
	{
        List<ShiftPhase> Phases = new List<ShiftPhase>();
        public Constants.BAT.Axes Axis { get; set; }

        /// <summary>
        /// Sets the phases.
        /// </summary>
        /// <param name="values">Values.</param>
        public void SetPhases(IEnumerable<SensorReading> records)
        {
            SensorReading prev = records.First();
            var currentPhase = new ShiftPhase
            {
				Axis = Axis,
                Records = (new SensorReading[] { prev }).ToList()
            };

            for (int i = 1; i < records.Count(); i++)
            {
                SensorReading next = records.ElementAt(i);
                if (AreSameSign(prev, next, Axis)) currentPhase.Records.Add(next);
                else
				{
					Phases.Add(currentPhase);
                    currentPhase = new ShiftPhase
                    { 
                        Axis = Axis,
                        Records = (new SensorReading[] { next }).ToList()
                    };  
                }
                
                prev = next;
            }

            Phases.Add(currentPhase);
        }

        /// <summary>
        /// Gets the phase shift point.
        /// </summary>
        /// <returns>The phase shift point.</returns>
        /// <param name="phaseIndex">Phase index.</param>
        public int? GetPhaseShiftPoint(int phaseIndex)
        {
            if (Phases.Count() > phaseIndex)
                return Phases[phaseIndex].ShiftPoint;
            return null;
        }

        /// <summary>
        /// Determines if the motion shift follows expected "forward reach" 
        /// behavior along the selected axis
        /// </summary>
        /// <returns><c>true</c>, if forward reach was identified, <c>false</c> otherwise.</returns>
        public bool IsForwardReach(decimal threshold)
        {
            if (Phases.Count() < 4) return false;
            switch (Axis)
			{
				case Constants.BAT.Axes.X:
                    // neg, pos, neg, pos
                    return (Phases[0].IsNegative &&
                            Phases[1].IsPositive && Phases[1].MaxAbsVal > threshold &&
                            Phases[2].IsNegative && Phases[2].MaxAbsVal > threshold &&
                            Phases[3].IsPositive);
				case Constants.BAT.Axes.Y:
					// pos, neg, pos, neg
					return (Phases[0].IsPositive &&
							Phases[1].IsNegative && Phases[1].MaxAbsVal > threshold &&
							Phases[2].IsPositive && Phases[2].MaxAbsVal > threshold &&
							Phases[3].IsNegative);
                 default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the shift points of another Motion Shift object are 
        /// within acceptable margins
        /// </summary>
        /// <returns><c>true</c>, if margins was upheld, <c>false</c> otherwise.</returns>
        /// <param name="other">Other motion shift, against which to compare.</param>
        /// <param name="margin">Margin.</param>
        public bool WithinMargins(MotionShift other, int margin)
        {
            // take the smaller of the two phase counts
            var phaseCount = other.Phases.Count() < this.Phases.Count() ?
                                  other.Phases.Count() : this.Phases.Count();

            // get shift points
            bool withinMargins = true;
            for (int i = 0; i < phaseCount; i++)
            {
                var shiftDiffAbs = System.Math.Abs(other.Phases[i].ShiftPoint - Phases[i].ShiftPoint);
                if (shiftDiffAbs > margin)
                {
                    withinMargins = false;
                    break;
                }
            }

            return withinMargins;
        }

        /// <summary>
        /// Checks to see if two decimal values are both pos. or both neg.
        /// </summary>
        /// <returns><c>true</c>, if same sign was same, <c>false</c> otherwise.</returns>
        /// <param name="first">first value to compare.</param>
        /// <param name="second">second value to compare.</param>
        bool AreSameSign(SensorReading first, SensorReading second, Constants.BAT.Axes axis)
        {
            switch (axis)
            {
				case Constants.BAT.Axes.X:
                    return (first.AccelX < 0 && second.AccelX < 0) || 
                        (first.AccelX > 0 && second.AccelX > 0) ||
                        (first.AccelX == 0 && second.AccelX == 0);
				case Constants.BAT.Axes.Y:
					return (first.AccelY < 0 && second.AccelY < 0) ||
						(first.AccelY > 0 && second.AccelY > 0) ||
						(first.AccelY == 0 && second.AccelY == 0);
                default:
                    return false;
            }
        }

        /*public SensorReading First { get; set; }
		public SensorReading Second { get; set; }

		// numeric representation of where the motion shift occurred
		public decimal ShiftPoint => (Second.RecordNum - First.RecordNum) / 2.0M;

		// shift from pos to neg means user is slowing down
        public bool IsSlowingDown(Constants.BAT.Axes axis)
        {
            switch (axis)
			{
				case Constants.BAT.Axes.X:
					return (First.AccelX > 0 && Second.AccelX < 0);
				case Constants.BAT.Axes.Y:
					return (First.AccelY > 0 && Second.AccelY < 0);
				case Constants.BAT.Axes.Z:
					return (First.AccelZ > 0 && Second.AccelZ < 0);
                default:
                    return false;
            }
        }

		// shift from neg to pos means user is speeding up
		public bool IsSpeedingUp(Constants.BAT.Axes axis)
		{
			switch (axis)
			{
				case Constants.BAT.Axes.X:
					return (First.AccelX < 0 && Second.AccelX > 0);
				case Constants.BAT.Axes.Y:
					return (First.AccelY < 0 && Second.AccelY > 0);
				case Constants.BAT.Axes.Z:
					return (First.AccelZ < 0 && Second.AccelZ > 0);
				default:
					return false;
			}
		}*/
    }

    class ShiftPhase
    {
		public Constants.BAT.Axes Axis { get; set; }
        public List<SensorReading> Records { get; set; }
        public IEnumerable<decimal> Values
        {
            get
            {
				return (Axis.Equals(Constants.BAT.Axes.X) ?
								 Records.Select(x => x.AccelX) :
								 (Axis.Equals(Constants.BAT.Axes.Y) ?
								  Records.Select(x => x.AccelY) :
								  new List<decimal>()));
            }
        }

        public int ShiftPoint => (Records.First().RecordNum);
        public decimal MaxAbsVal => (Values.Select(x => System.Math.Abs(x)).Max());
        public bool IsNegative => (Values.First() < 0.0M);
        public bool IsPositive => (Values.First() >= 0.0M);
    }
}