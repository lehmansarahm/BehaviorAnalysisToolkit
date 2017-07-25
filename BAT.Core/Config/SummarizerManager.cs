using System;
using BAT.Core.Common;
using BAT.Core.Summarizers;

namespace BAT.Core.Config
{
    public class SummarizerManager : TypeManager
	{
		public static ISummarizer GetSummarizer(string name)
		{
            var summarizerName = name.EndsWith("Summarizer") ? name : name + "Summarizer";
			var typeName = typeof(ISummarizer).Namespace + "." + summarizerName;
			var type = Type.GetType(typeName);
			if (type != null)
				return (ISummarizer)Activator.CreateInstance(type);
			else
			{
				LogManager.Error($"Could not find summarizer named {name}");
				return null;
            }
		}
    }
}