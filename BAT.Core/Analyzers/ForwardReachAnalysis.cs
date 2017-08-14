using System.Collections.Generic;
using System.Linq;
using BAT.Core.Analyzers.Results;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Analyzers
{
    /// <summary>
    /// Searches sensor reading input collections for the following trends:
    ///     - accel shifts along x axis: neg to pos to neg to pos
    ///     - accel shifts along y axis: pos to neg to pos to neg
    ///     - shifts should happen more or less concurrently between axes 
    ///         within provided window size
    /// 
    /// Expected configuration parameters:
    ///     - ACCELERATION:
    ///         - "window size" (default value - 30)
    ///         - "margin" (how many readings between axis shifts we will allow)
    ///     - ACCEL-X/Y:
    ///         - "threshold" (default value - 0.6)
    /// </summary>
    public class ForwardReachAnalysis : IAnalyzer
    {
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>The header.</returns>
        public string[] Header => ForwardReachOutput.ResultHeader;

        /// <summary>
        /// Gets the header csv.
        /// </summary>
        /// <returns>The header csv.</returns>
        public string HeaderCsv => ForwardReachOutput.ResultHeaderCsv;

        /// <summary>
        /// Analyze the specified input and parameters.
        /// </summary>
        /// <returns>The analyze.</returns>
        /// <param name="inputs">Input.</param>
        /// <param name="parameters">Parameters.</param>
		public IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> inputs,
                                                 IEnumerable<Parameter> parameters)
        {
            var results = new List<ForwardReachResult>();

            // parse out parameters
            var accelParam = parameters.FirstOrDefault(x => x.Field.Equals(CommandParameters.Acceleration));
            var accelXParam = parameters.FirstOrDefault(x => x.Field.Equals(CommandParameters.AccelX));
            var accelYParam = parameters.FirstOrDefault(x => x.Field.Equals(CommandParameters.AccelY));
            if (accelParam == null || accelXParam == null || accelYParam == null) 
                return null;

            // parse out clause values
            var windowSize = int.Parse(accelParam.GetClauseValue(CommandParameters.Window));
			var margin = int.Parse(accelParam.GetClauseValue(CommandParameters.Margin));
			var accelXThreshold = decimal.Parse(accelXParam.GetClauseValue(CommandParameters.Threshold));
			var accelYThreshold = decimal.Parse(accelYParam.GetClauseValue(CommandParameters.Threshold));

            // iterate through input records with a single-record sliding window
            var currentInputs = new List<SensorReading>();
            foreach (var input in inputs)
            {
                // add new inputs to the collection until it is full
                if (currentInputs.Count() < windowSize) currentInputs.Add(input);

                // once it is full, check for reach behavior match
                if (currentInputs.Count() == windowSize)
                {
                    // convert raw accel-x vals into motion shift obj
                    var xShift = new MotionShift { Axis = Constants.BAT.Axes.X };
                    xShift.SetPhases(currentInputs);

                    // convert raw accel-y vals into motion shift obj
                    var yShift = new MotionShift { Axis = Constants.BAT.Axes.Y };
                    yShift.SetPhases(currentInputs);

                    // determine if shifts meet "forward reach" behavior expectations
                    var isForwardReachOnX = xShift.IsForwardReach(accelXThreshold);
                    var isForwardReachOnY = yShift.IsForwardReach(accelYThreshold);
                    var shiftsAreWithinMargins = xShift.WithinMargins(yShift, margin);
                    if (isForwardReachOnX && isForwardReachOnY && shiftsAreWithinMargins)
                    {
                        var newResult = new ForwardReachResult(xShift, yShift);
                        if (!results.Any(x => x.Matches(newResult)))
                            results.Add(newResult);

                        // if match found, shift current input collection
                        // by one half window size and continue
                        for (int i = 0; i < (windowSize / 2); i++)
                            currentInputs.RemoveAt(i);
                        continue;
                    }

                    // if no match found, remove the first record and go again
                    currentInputs.RemoveAt(0);
                }
            }

            return results.Distinct();
        }

        /// <summary>
        /// Consolidates the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="data">Data.</param>
		public IEnumerable<ICsvWritable> ConsolidateData(Dictionary<string, IEnumerable<ICsvWritable>> data)
        {
            return null;
        }

        /// <summary>
        /// Evaluates the current inputs.
        /// </summary>
        /// <param name="currentInputs">Current inputs.</param>
        /// <param name="xShifts">X shifts.</param>
        /// <param name="yShifts">Y shifts.</param>
        /*void EvaluateCurrentInputs(List<SensorReading> currentInputs, 
                                   out List<MotionShift> xShifts, 
                                   out List<MotionShift> yShifts)
		{
			SensorReading prev = null;
            xShifts = new List<MotionShift>();
            yShifts = new List<MotionShift>();

			foreach (var currentInput in currentInputs)
			{
				if (prev == null)
				{
					prev = currentInput;
					continue;
				}

				MotionShift currentShift = new MotionShift
				{
					First = prev,
					Second = currentInput
				};

				// check to see if we're a match along the X axis
				switch (xShifts.Count())
				{
					case 0:
					case 2:
						if (currentShift.IsSpeedingUp(Constants.BAT.Axes.X))
							xShifts.Add(currentShift);
						break;
					case 1:
						if (currentShift.IsSlowingDown(Constants.BAT.Axes.X))
							xShifts.Add(currentShift);
						break;
				}

				// check to see if we're a match along the Y axis
				switch (yShifts.Count())
				{
					case 0:
					case 2:
						if (currentShift.IsSlowingDown(Constants.BAT.Axes.Y))
							yShifts.Add(currentShift);
						break;
					case 1:
						if (currentShift.IsSpeedingUp(Constants.BAT.Axes.Y))
							yShifts.Add(currentShift);
						break;
				}

				prev = currentInput;
			}
        }

        /// <summary>
        /// Ises the forward reach.
        /// </summary>
        /// <returns><c>true</c>, if forward reach was ised, <c>false</c> otherwise.</returns>
        /// <param name="xShifts">X shifts.</param>
        /// <param name="yShifts">Y shifts.</param>
        /// <param name="margin">Margin.</param>
        bool IsForwardReach(List<MotionShift> xShifts, List<MotionShift> yShifts, int margin)
		{
			// It is a forward reach IFF:
			//      - we made it through the above loop and all xShift,
			//        yShift objects are populated
			//      - the shift point for related 0, 1, 2 index objects 
			//        between two axes are within marginal allowance
			bool isForwardReach = xShifts.Count() == 3 && yShifts.Count() == 3 &&
                       AreWithinMargin(xShifts[0], yShifts[0], margin) &&
                       AreWithinMargin(xShifts[1], yShifts[1], margin) &&
                       AreWithinMargin(xShifts[2], yShifts[2], margin);
            return isForwardReach;
        }

        /// <summary>
        /// Ares the within margin.
        /// </summary>
        /// <returns><c>true</c>, if within margin was ared, <c>false</c> otherwise.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="margin">Margin.</param>
        bool AreWithinMargin(MotionShift x, MotionShift y, int margin)
        {
            var absDiff = System.Math.Abs(x.ShiftPoint - y.ShiftPoint);
            return (absDiff <= margin);
        }*/
    }
}