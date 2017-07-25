using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Transformers;

namespace BAT.Core.Config
{
    public class TransformerManager : TypeManager
	{
		public static IEnumerable<ITransformer> GetTransformers(List<string> Transformers)
		{
			//this finds all ITransfomers
			var transformers = GetInheritingTypes<ITransformer>();
			foreach (var name in Transformers)
			{
				var type = transformers.FirstOrDefault(x => x.Name == name + "Transformer" || x.Name == name);
				if (type != null)
					yield return (ITransformer)Activator.CreateInstance(type);
				else
					LogManager.Error($"Could not find transformer named {name}");
			}
		}
    }
}