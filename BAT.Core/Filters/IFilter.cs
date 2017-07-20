using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Filters
{
	public interface IFilter
	{
		List<SensorReading> Filter(List<SensorReading> input);
	}
}