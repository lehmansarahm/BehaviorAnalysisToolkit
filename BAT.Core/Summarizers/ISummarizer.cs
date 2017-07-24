using System;
using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Summarizers
{
    public interface ISummarizer
	{
		string[] GetHeader();
		string GetHeaderCsv();

		Type PhaseResultType { get; }
        IEnumerable<KeyValuePair<string, string>> 
            Summarize<T>(Dictionary<string, IEnumerable<T>> input) where T : ICsvWritable;
    }
}