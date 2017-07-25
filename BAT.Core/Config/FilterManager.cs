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

        public static string GetFilename(string key, string id)
        {
            var filenameComponents = key.Split('.');
            var fileName = filenameComponents[filenameComponents.Length - 2];
            var fileExtension = filenameComponents[filenameComponents.Length - 1];
            var newFilename = $"{fileName}_{id}.{fileExtension}";
            return newFilename;
        }
    }
}