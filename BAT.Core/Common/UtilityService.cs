using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Constants;

namespace BAT.Core.Common
{
    public class UtilityService
	{
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

        /// <summary>
        /// Gets the max.
        /// </summary>
        /// <returns>The max.</returns>
        /// <param name="vals">Vals.</param>
        public static int? GetMaxInt(params int?[] vals)
        {
            return (vals.ToList().Max());
        }

        public static int? GetMinInt(params int?[] vals)
        {
            return (vals.ToList().Min());
        }
    }
}