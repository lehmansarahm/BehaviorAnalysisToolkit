using System.Collections.Generic;
using BAT.Core.Analyzers;
using BAT.Core.Filters;
using BAT.Core.Summarizers;
using BAT.Core.Transformers;

namespace BAT.Core.Config
{
    public class Configuration
    {
        public List<ITransformer> transformers;
        public List<IFilter> filters;
        public List<IAnalyzer> analyzers;
        public List<ISummarizer> summarizers;
    }
}
