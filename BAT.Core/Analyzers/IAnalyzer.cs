using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Analyzers
{
    public interface IAnalyzer
    {
        string[] GetHeader();
        string GetHeaderCsv();

        IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
                                          IEnumerable<Parameter> parameters);
    }
}