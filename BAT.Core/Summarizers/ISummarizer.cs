using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Summarizers
{
    public interface ISummarizer
	{
		string[] GetHeader();
		string GetHeaderCsv();

		string[] GetFooterLabels();
		string[] GetFooterValues();
		string GetFooterCsv();

        IEnumerable<string[]> Summarize<T>(Dictionary<string, IEnumerable<T>> input) where T : ICsvWritable;
    }
}