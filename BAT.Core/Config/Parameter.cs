using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BAT.Core.Common;
using BAT.Core.Constants;

namespace BAT.Core.Config
{
    public class Parameter
    {
        public string Field { get; set; }
        public List<KeyValuePair<string, string>> Clauses { get; set; }

        /// <summary>
        /// Gets the filter value.
        /// </summary>
        /// <returns>The filter value.</returns>
        /// <param name="filterField">Filter field.</param>
        /// <param name="input">Input.</param>
        public static string GetFilterValue(PropertyInfo filterField, SensorReading input)
        {
            return filterField.GetValue(input, null).ToString();
        }

        /// <summary>
        /// Matcheses the clause.
        /// </summary>
        /// <returns><c>true</c>, if clause was matchesed, <c>false</c> otherwise.</returns>
        /// <param name="filterField">Filter field.</param>
        /// <param name="input">Input.</param>
        public bool MatchesClause(PropertyInfo filterField, SensorReading input)
        {
            var filterValue = GetFilterValue(filterField, input);
            return MatchesClause(filterValue);
        }

        /// <summary>
        /// Matcheses the clause.
        /// </summary>
        /// <returns><c>true</c>, if clause was matchesed, <c>false</c> otherwise.</returns>
        /// <param name="source">Input value.</param>
        public bool MatchesClause(string source)
        {
            bool isMatch = true, isLocalMatch = false;
            foreach (var clause in Clauses)
            {
                switch (clause.Key)
				{
					case CommandParameters.Contains:
						isLocalMatch = (Constants.BAT.CULTURE
										.CompareInfo.IndexOf(source, clause.Value,
															 CompareOptions.IgnoreCase) >= 0);
						isMatch &= isLocalMatch;    // isMatch = true if both are true
						break;
                    case CommandParameters.DoesNotContain:
						isLocalMatch = (Constants.BAT.CULTURE
										.CompareInfo.IndexOf(source, clause.Value,
															 CompareOptions.IgnoreCase) < 0);
						isMatch &= isLocalMatch;    // isMatch = true if both are true
						break;
                    case CommandParameters.EqualTo:
                        isLocalMatch = (source.Equals(clause.Key,
                                                      StringComparison.InvariantCultureIgnoreCase));
                        isMatch &= isLocalMatch;
                        break;
                    case CommandParameters.NotEqualTo:
                        isLocalMatch = !(source.Equals(clause.Key,
                                                       StringComparison.InvariantCultureIgnoreCase));
                        isMatch &= isLocalMatch;
                        break;
                }
            }

            return isMatch;
        }

        /// <summary>
        /// Splits the output.
        /// </summary>
        /// <returns><c>true</c>, if output was split, <c>false</c> otherwise.</returns>
        public bool UseOutputSplit()
        {
            var splitClause = Clauses.Where(x => x.Key.Contains(CommandParameters.Split));
            bool splitOutput = splitClause.Any() && splitClause.FirstOrDefault().Value
                                                    .Equals("true", StringComparison.InvariantCultureIgnoreCase);
            return splitOutput;
        }

        /// <summary>
        /// Gets the clause value.
        /// </summary>
        /// <returns>The clause value.</returns>
        /// <param name="clauseKey">Clause key.</param>
        public string GetClauseValue(string clauseKey)
        {
            var clauseValue = Clauses.FirstOrDefault(x => x.Key.Equals(clauseKey, StringComparison.InvariantCultureIgnoreCase)).Value;
            return clauseValue;
        }
    }
}