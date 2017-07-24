using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Filters.Impl
{
    public class CompletionFilter : IFilter
    {
        public IEnumerable<FilterResult> Filter(IEnumerable<SensorReading> input, 
                                                IEnumerable<Parameter> parameters) {
            return null;
        }
    }
}