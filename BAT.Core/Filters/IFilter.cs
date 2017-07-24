using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Filters
{
	public interface IFilter
	{
		IEnumerable<FilterResult> Filter(IEnumerable<SensorReading> input,
										 IEnumerable<Parameter> predicates);
	}
}