using System.Collections.Generic;

namespace BAT.Core.Common
{
    public class PhaseData<T> where T : ICsvWritable
    {
        public string Name { get; set; }
        public List<T> Data { get; set; }
    }
}