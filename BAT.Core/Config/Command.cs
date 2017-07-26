using System.Collections.Generic;
using System.Linq;

namespace BAT.Core.Config
{
    public class Command
    {
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }
        public bool HasParameters 
        {
            get
            {
                return Parameters != null && Parameters.Any();
            }
        }
    }
}