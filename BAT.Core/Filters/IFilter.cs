using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Filters
{
	public interface IFilter
	{
        string[] Header { get; }
        string HeaderCsv { get; }

		IEnumerable<PhaseData<SensorReading>> Filter(PhaseInput<SensorReading> input);
	}
}