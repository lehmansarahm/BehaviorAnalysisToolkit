using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Filters
{
    public class FilterResult
    {
        public string Name { get; set; }
        public List<SensorReading> Data { get; set; }
    }
}