using System.Collections.Generic;
using BAT.Core.Config;

namespace BAT.Core.Common
{
    public class PhaseInput<T> where T : ICsvWritable
    {
        public List<PhaseData<T>> Input { get; set; }
        public List<Parameter> Parameters { get; set; }
    }
}