using System.Collections.Generic;

namespace BAT.Core.Config
{
    public class Parameter
    {
        public string Field { get; set; }
        public List<KeyValuePair<string, string>> Clauses { get; set; }
    }
}