using System;
using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Summarizers
{
    public interface ISummarizer
	{
		void Summarize(List<SensorReading> input);
    }
}