using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Filters
{
	public interface IFilter
	{
		string[] GetHeader();
		string GetHeaderCsv();

		IEnumerable<PhaseResult> Filter(IEnumerable<ICsvWritable> input,
										 IEnumerable<Parameter> predicates);
	}
}