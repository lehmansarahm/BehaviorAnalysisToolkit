using System.Collections.Generic;

namespace BAT.Core.Config
{
    public class Command
    {
        public string Name { get; set; }
        public List<KeyValuePair<string, string>> Parameters { get; set; }
    }
}