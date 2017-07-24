using System.Collections.Generic;

namespace BAT.Core.Common
{
    public class PhaseResult
    {
        public string Name { get; set; }
        public List<ICsvWritable> Data { get; set; }
    }
}