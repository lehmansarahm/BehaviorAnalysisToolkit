﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BAT.Core.Common
{
    public class UtilityService
	{
        /// <summary>
        /// Total the specified valueList.
        /// </summary>
        /// <returns>The total.</returns>
        /// <param name="valueList">Value list.</param>
        public static double Total(List<double> valueList)
        {
            return valueList.Sum(x => x);
        }

        /// <summary>
        /// Average the specified valueList.
        /// </summary>
        /// <returns>The average.</returns>
        /// <param name="valueList">Value list.</param>
        public static double Average(List<double> valueList)
        {
            return (valueList != null && valueList.Any()) 
                ? (valueList.Sum(x => x) / valueList.Count)
                : 0;
        }

		/// <summary>
		/// Standards the deviation.
		/// </summary>
		/// <returns>The deviation.</returns>
		/// <param name="valueList">Value list.</param>
		public static double StandardDeviation(List<double> valueList)
		{
			double M = 0.0;
			double S = 0.0;
			int k = 1;
			foreach (double value in valueList)
			{
				double tmpM = M;
				M += (value - tmpM) / k;
				S += (value - tmpM) * (value - M);
				k++;
			}
			return Math.Sqrt(S / (k - 2));
		}
    }
}
