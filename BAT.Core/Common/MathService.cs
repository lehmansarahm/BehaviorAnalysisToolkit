using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Integration;

namespace BAT.Core.Common
{
    public static class MathService
    {
        static List<double> currentVals;
        static double EPSILON = 1e-6;

        /// <summary>
        /// Total the specified valueList.
        /// </summary>
        /// <returns>The total.</returns>
        /// <param name="valueList">Value list.</param>
        public static decimal Total(IEnumerable<decimal> valueList)
        {
            return valueList.Sum();
        }

        /// <summary>
        /// Average the specified valueList.
        /// </summary>
        /// <returns>The average.</returns>
        /// <param name="valueList">Value list.</param>
        public static decimal Average(IEnumerable<decimal> valueList)
        {
            return (valueList != null && valueList.Any()) ? valueList.Average() : 0;
        }

        /// <summary>
        /// Standards the deviation.
        /// </summary>
        /// <returns>The deviation.</returns>
        /// <param name="values">Value list.</param>
        public static decimal StandardDeviation(IEnumerable<decimal> values)
        {
            try
            {
                double ret = 0;
                if (values.Any())
                {
                    //Compute the Average      
                    double avg = (double)values.Average();
                    //Perform the Sum of (value-avg)_2_2      
                    double sum = values.Sum(d => Math.Pow((double)d - avg, 2));
                    //Put it all together      
                    ret = Math.Sqrt((sum) / (values.Count() - 1));
                }
                return (decimal)ret;
            }
            catch (OverflowException ex)
            {
                //LogManager.Error("Something went wrong while attempting to calculate "
                //                 + "standard deviation", ex, typeof(UtilityService));
                return 0.0M;
            }
        }

        /// <summary>
        /// Gets the magnitude.
        /// </summary>
        /// <returns>The magnitude.</returns>
        /// <param name="values">Values.</param>
        public static decimal GetMagnitude(params decimal[] values)
        {
            var squares = values.Select(x => Math.Pow((double)x, 2));
            var sum = squares.ToList().Sum();
            return (decimal)Math.Sqrt(sum);
        }

        /// <summary>
        /// Gets the variance.
        /// </summary>
        /// <returns>The variance.</returns>
        /// <param name="values">Values.</param>
		public static decimal GetVariance(List<decimal> values)
        {
            var mean = values.Average();
            var squaresOfDiffs = values.Select(x => Math.Pow(Math.Abs((double)(x - mean)), 2));
            return (decimal)squaresOfDiffs.Average();
        }

        /// <summary>
        /// Gets the skewness.
        /// </summary>
        /// <returns>The skewness.</returns>
        /// <param name="values">Values.</param>
        public static decimal GetSkewness(List<decimal> values)
        {
            var mean = values.Average();
            var stdDev = StandardDeviation(values);
			var nMinusOne = (values.Count() - 1);
            if (nMinusOne == 0) return 0.0M;

            var cubesOfDiffs = values.Select(x => Math.Pow(Math.Abs((double)(x - mean)), 3));
            var sum = cubesOfDiffs.Sum();
            var stdDevCubed = Math.Pow((double)stdDev, 3);
            if (Math.Abs(stdDevCubed) < EPSILON) return 0.0M;

            return (decimal)(sum / nMinusOne * stdDevCubed);
        }

        /// <summary>
        /// Gets the kurtosis.
        /// </summary>
        /// <returns>The kurtosis.</returns>
        /// <param name="values">Values.</param>
        public static decimal GetKurtosis(List<decimal> values)
		{
			var mean = values.Average();
			var stdDev = StandardDeviation(values);
			var nMinusOne = (values.Count() - 1);
			if (nMinusOne == 0) return 0.0M;

            var foursOfDiffs = values.Select(x => Math.Pow(Math.Abs((double)(x - mean)), 4));
			var sum = foursOfDiffs.Sum();
			var stdDevFourthed = Math.Pow((double)stdDev, 4);
			if (Math.Abs(stdDevFourthed) < EPSILON) return 0.0M;

			return (decimal)(sum / nMinusOne * stdDevFourthed);
        }

        /// <summary>
        /// Gets the rms.
        /// </summary>
        /// <returns>The rms.</returns>
        /// <param name="values">Values.</param>
		public static decimal GetRMS(List<decimal> values)
        {
            var squares = values.Select(x => Math.Pow(Math.Abs((double)x), 2));
            var average = squares.Average();
            return (decimal)Math.Sqrt(average);
        }

        /// <summary>
        /// Simpsonses the rule integral.
        /// </summary>
        /// <returns>The rule integral.</returns>
        /// <param name="values">Values.</param>
        public static List<decimal> SimpsonsRuleIntegral(List<decimal> values)
        {
            var integralVals = new List<decimal>();
            if (values.Any())
            {
                currentVals = values.Select(x => (double)x).ToList();
                for (int i = 1; i < currentVals.Count(); i++)
                {
                    var simpsonIntegral =
                        SimpsonRule.IntegrateComposite(GetValue, 0, i, (i * 2));
                    integralVals.Add((decimal)simpsonIntegral);
                }
            }

            return integralVals;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="index">Index.</param>
        static double GetValue(double index)
        {
            return currentVals[(int)index];
        }
    }
}
