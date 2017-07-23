using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Analyzers
{
    public class AnalysisResult
    {
        public string Name { get; set; }
        public List<SensorReading> Data { get; set; }
    }
}