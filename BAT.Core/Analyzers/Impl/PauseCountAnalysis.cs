using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Analyzers.Impl
{
    public class PauseCountAnalysis : IAnalyzer
	{
		public IEnumerable<AnalysisResult> Analyze(IEnumerable<SensorReading> input,
											IEnumerable<Parameter> parameters)
        {
            return null;
        }
    }
}