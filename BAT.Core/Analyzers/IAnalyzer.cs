using System;
using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Analyzers
{
    public interface IAnalyzer
    {
        void Analyze(List<SensorReading> input, string[] args);
    }
}