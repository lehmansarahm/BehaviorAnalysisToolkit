using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;

namespace BAT.Core.Analyzers.Results
{
    public class SciKitFeatureVector
	{
		public decimal Mean { get; }
		public decimal Variance { get; }
		public decimal Skewness { get; }
		public decimal Kurtosis { get; }
		public decimal RMS { get; }

        public SciKitFeatureVector(IEnumerable<decimal> values)
        {
            Mean = values.Average();
            Variance = MathService.GetVariance(values.ToList());
            Skewness = MathService.GetSkewness(values.ToList());
            Kurtosis = MathService.GetKurtosis(values.ToList());
            RMS = MathService.GetRMS(values.ToList());
        }
    }
}
