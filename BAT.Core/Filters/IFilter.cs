using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Filters
{
	public interface IFilter
	{
        IEnumerable<FilterResult> Filter(IEnumerable<SensorReading> input, 
                                         IEnumerable<KeyValuePair<string,string>> parameters);
	}
}