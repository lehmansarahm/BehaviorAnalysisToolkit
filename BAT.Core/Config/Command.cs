using System;
using System.Collections.Generic;
using System.Globalization;
using BAT.Core.Common;

namespace BAT.Core.Config
{
    public class Command
    {
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }

        /// <summary>
        /// Contains the specified source and target.
        /// </summary>
        /// <returns>The contains.</returns>
        /// <param name="source">Source.</param>
        /// <param name="target">Target.</param>
        public static bool Contains(string source, string target)
        {
            bool contains = (Constants.CULTURE
                             .CompareInfo.IndexOf(source, target, 
                                                  CompareOptions.IgnoreCase) >= 0);
            return contains;
        }

        /// <summary>
        /// Equals the specified source and target.
        /// </summary>
        /// <returns>The equals.</returns>
        /// <param name="source">Source.</param>
        /// <param name="target">Target.</param>
        public static bool EqualTo(string source, string target)
		{
			bool equalTo = (source.Equals(target, StringComparison.InvariantCultureIgnoreCase));
            return equalTo;
        }

        /// <summary>
        /// Nots the equals.
        /// </summary>
        /// <returns><c>true</c>, if equals was noted, <c>false</c> otherwise.</returns>
        /// <param name="source">Source.</param>
        /// <param name="target">Target.</param>
        public static bool NotEqualTo(string source, string target)
		{
            bool notEqualTo = !(source.Equals(target, StringComparison.InvariantCultureIgnoreCase));
			return notEqualTo;
        }
    }
}