using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Impl
{
	public class PauseDurationAnalysis : IAnalyzer
	{
		public IEnumerable<AnalysisResult> Analyze(IEnumerable<SensorReading> input,
											IEnumerable<KeyValuePair<string, string>> parameters)
		{
			return null;
		}
	}
}