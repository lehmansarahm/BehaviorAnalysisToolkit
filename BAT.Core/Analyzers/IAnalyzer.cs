using System;
using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Analyzers
{
    public interface IAnalyzer
    {
        IEnumerable<AnalysisResult> Analyze(IEnumerable<SensorReading> input,
                                            IEnumerable<KeyValuePair<string, string>> parameters);
    }
}