using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BAT.Core.Common;

namespace BAT.Core.Config
{
    public class Parameter
    {
        public string Field { get; set; }
        public List<KeyValuePair<string, string>> Clauses { get; set; }

        /// <summary>
        /// Matcheses the clause.
        /// </summary>
        /// <returns><c>true</c>, if clause was matchesed, <c>false</c> otherwise.</returns>
        /// <param name="source">Input value.</param>
        public bool MatchesClause(string source)
        {
            bool isMatch = true;
			foreach (var clause in Clauses)
			{
				switch (clause.Key)
				{
					case Constants.COMMAND_PARAM_CONTAINS:
						isMatch = (Constants.CULTURE
                                   .CompareInfo.IndexOf(source, clause.Value,
                                                        CompareOptions.IgnoreCase) >= 0);
						break;
					case Constants.COMMAND_PARAM_EQUAL_TO:
						isMatch = (source.Equals(clause.Key, 
                                                 StringComparison.InvariantCultureIgnoreCase));
						break;
					case Constants.COMMAND_PARAM_NOT_EQUAL_TO:
						isMatch = !(source.Equals(clause.Key, 
                                                  StringComparison.InvariantCultureIgnoreCase));
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
			bool splitOutput = Clauses.Where(x => x.Key.Contains(Constants.COMMAND_PARAM_SPLIT))
                                      .FirstOrDefault().Value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            return splitOutput;
        }

        /// <summary>
        /// Gets the clause value.
        /// </summary>
        /// <returns>The clause value.</returns>
        /// <param name="clauseKey">Clause key.</param>
        public string GetClauseValue(string clauseKey)
        {
            var clauseValue = Clauses.Where(x => x.Key.Equals(clauseKey, StringComparison.InvariantCultureIgnoreCase))
                                     .FirstOrDefault().Value;
            return clauseValue;
        }
    }
}