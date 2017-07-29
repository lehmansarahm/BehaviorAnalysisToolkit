using System;
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
        public static decimal Total(List<decimal> valueList)
        {
            return valueList.Sum(x => x);
        }

        /// <summary>
        /// Average the specified valueList.
        /// </summary>
        /// <returns>The average.</returns>
        /// <param name="valueList">Value list.</param>
        public static decimal Average(List<decimal> valueList)
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
		public static decimal StandardDeviation(List<decimal> valueList)
		{
			double M = 0.0;
			double S = 0.0;
			int k = 1;

            try
			{
				foreach (double value in valueList)
				{
					double tmpM = M;
					M += (value - tmpM) / k;
					S += (value - tmpM) * (value - M);
					k++;
				}

				return (decimal)Math.Sqrt(S / (k - 2));
            }
            catch (OverflowException ex)
            {
                LogManager.Error("Something went wrong while attempting to calculate "
                                 + "standard deviation", ex, typeof(UtilityService));
                return 0.0M;
            }
		}

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="inputFields">Input fields.</param>
        /// <param name="field">Field.</param>
        /// <param name="defaultVal">Default value.</param>
		public static string GetString(string[] inputFields, InputFile.ColumnOrder field, string defaultVal)
		{
				var index = (int)field;
				return inputFields.Length < index + 1 ? defaultVal : inputFields[index];
		}

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <returns>The date.</returns>
        /// <param name="inputFields">Input fields.</param>
        /// <param name="field">Field.</param>
        public static DateTime GetDate(string[] inputFields, InputFile.ColumnOrder field)
		{
            var rawString = GetString(inputFields, field, Constants.EMPTY);
			return DateTime.Parse(rawString);
		}

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="inputFields">Input fields.</param>
        /// <param name="field">Field.</param>
		public static int GetInt(string[] inputFields, InputFile.ColumnOrder field)
		{
			var rawString = GetString(inputFields, field, Constants.EMPTY);
			return int.Parse(rawString);
		}

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <returns>The decimal.</returns>
        /// <param name="inputFields">Input fields.</param>
        /// <param name="field">Field.</param>
		public static decimal GetDecimal(string[] inputFields, InputFile.ColumnOrder field)
		{
			var rawString = GetString(inputFields, field, Constants.EMPTY);
			return decimal.Parse(rawString);
		}
    }
}