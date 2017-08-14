using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Summarizers
{
    public interface ISummarizer
	{
        string[] Header { get; }
        string HeaderCsv { get; }

        string[] FooterLabels { get; }
        string[] FooterValues { get; }
        string FooterCsv { get; }

        void Initialize(Dictionary<string, IEnumerable<SensorReading>> InputData);
        IEnumerable<string[]> Summarize<T>(Dictionary<string, IEnumerable<T>> input) where T : ICsvWritable;
    }
}