using System;
using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Analyzers
{
    public interface IAnalyzer
    {
        IEnumerable<AnalysisResult> Analyze(IEnumerable<SensorReading> input,
                                            IEnumerable<Parameter> parameters);
    }
}