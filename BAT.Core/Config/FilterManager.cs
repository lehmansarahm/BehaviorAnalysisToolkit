using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Filters;

namespace BAT.Core.Config
{
    public class FilterManager : TypeManager
	{
		public static IFilter GetFilter(string name)
		{
			var filters = GetInheritingTypes<IFilter>();
			var type = filters.FirstOrDefault(x => x.Name == name + "Filter" || x.Name == name);
			if (type != null)
				return (IFilter)Activator.CreateInstance(type);
			else
				LogManager.Error($"Could not find filter named {name}");
			return null;
		}

        /// <summary>
        /// Containses the filter.
        /// </summary>
        /// <returns><c>true</c>, if filter was containsed, <c>false</c> otherwise.</returns>
        /// <param name="filterCommands">Filter commands.</param>
        /// <param name="expType">Exp type.</param>
        public static bool ContainsFilter(List<Command> filterCommands, Type expType)
        {
            return filterCommands.Select(x => $"{x.Name}Filter")
                                 .Where(x => x.Equals(expType.Name)).Any();
        }

        /// <summary>
        /// Gets the labeled filename.
        /// </summary>
        /// <returns>The labeled filename.</returns>
        /// <param name="key">Key.</param>
        /// <param name="id">Identifier.</param>
        public static string GetFilterFilename(string key, string id)
        {
            return $"{key}_{id}{Constants.DEFAULT_INPUT_FILE_EXT}";
        }
    }
}