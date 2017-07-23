using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Filters.Impl
{
	public class ActivityFilter : IFilter
	{
        public IEnumerable<FilterResult> Filter(IEnumerable<SensorReading> input, 
                                                IEnumerable<KeyValuePair<string,string>> parameters)
		{
			return null;
		}
	}
}