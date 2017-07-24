using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;

namespace BAT.Core.Config
{
    public class Parameter
    {
        public string Field { get; set; }
        public List<KeyValuePair<string, string>> Clauses { get; set; }

        public bool SplitOutput()
        {
			bool splitOutput = Clauses.Where(x => x.Key.Contains(Constants.COMMAND_PARAM_SPLIT))
                                      .FirstOrDefault().Value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            return splitOutput;
        }
    }
}