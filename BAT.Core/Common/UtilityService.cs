using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Constants;

namespace BAT.Core.Common
{
    public class UtilityService
	{
		/// <summary>
		/// Total the specified valueList.
		/// </summary>
		/// <returns>The total.</returns>
		/// <param name="valueList">Value list.</param>
		public static decimal Total(IEnumerable<decimal> valueList)
		{
			return valueList.Sum(x => x);
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
				if (values.Count() > 0)
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
            var rawString = GetString(inputFields, field, Constants.BAT.EMPTY);
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
			var rawString = GetString(inputFields, field, Constants.BAT.EMPTY);
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
			var rawString = GetString(inputFields, field, Constants.BAT.EMPTY);
			return decimal.Parse(rawString);
		}
    }
}