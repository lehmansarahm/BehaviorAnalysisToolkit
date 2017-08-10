using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Integration;

namespace BAT.Core.Common
{
    public static class MathService
	{
		static List<double> currentVals;

		/// <summary>
		/// Simpsonses the rule integral.
		/// </summary>
		/// <returns>The rule integral.</returns>
		/// <param name="values">Values.</param>
		public static List<decimal> SimpsonsRuleIntegral(List<decimal> values)
		{
			var integralVals = new List<decimal>();
			if (values.Any())
			{
                currentVals = values.Select(x => (double)x).ToList();
				for (int i = 1; i < currentVals.Count(); i++)
				{
                    var simpsonIntegral =
                        SimpsonRule.IntegrateComposite(GetValue, 0, i, (i * 2));
                    integralVals.Add((decimal)simpsonIntegral);
				}
			}

			return integralVals;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="index">Index.</param>
		static double GetValue(double index)
		{
			return currentVals[(int)index];
		}
    }
}
