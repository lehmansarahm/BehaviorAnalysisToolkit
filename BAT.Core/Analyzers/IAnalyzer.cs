using System;
using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Analyzers
{
    public interface IAnalyzer
	{
        string[] Header { get; }
        string HeaderCsv { get; }

        IEnumerable<ICsvWritable> Analyze(IEnumerable<SensorReading> input,
										  IEnumerable<Parameter> parameters);
		IEnumerable<ICsvWritable> ConsolidateData(Dictionary<string, IEnumerable<ICsvWritable>> data);
    }
}