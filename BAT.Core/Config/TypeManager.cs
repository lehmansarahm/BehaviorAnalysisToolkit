using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BAT.Core.Config
{
    public class TypeManager
	{
		protected static IEnumerable<Type> GetInheritingTypes<T>()
		{
			var type = typeof(T);
			return Assembly.GetAssembly(type).GetTypes().Where(x => type.IsAssignableFrom(x) && !x.IsInterface);
			//you could later expand this to check for different assemblies and concat the types here
		}
    }
}