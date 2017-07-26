using System;
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
        /// Gets the labeled filename.
        /// </summary>
        /// <returns>The labeled filename.</returns>
        /// <param name="key">Key.</param>
        /// <param name="id">Identifier.</param>
        public static string GetLabeledFilename(string key, string id)
        {
            return $"{key}_{id}{Constants.DEFAULT_INPUT_FILE_EXT}";
        }
    }
}