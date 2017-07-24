using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;

namespace BAT.Core.Filters.Impl
{
	public class ActivityFilter : IFilter
	{
        /// <summary>
        /// Filter the specified input and parameters.
        /// </summary>
        /// <returns>The filter.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<FilterResult> Filter(IEnumerable<SensorReading> input, 
                                                IEnumerable<KeyValuePair<string,string>> parameters)
		{
            var results = new List<FilterResult>();
            var whereParam = parameters.Where(x => x.Key.Equals(Constants.COMMAND_PARAMETER_WHERE)).FirstOrDefault();
            var groupParam = parameters.Where(x => x.Key.Equals(Constants.COMMAND_PARAMETER_GROUP_BY)).FirstOrDefault();

            // TODO - split input by lambda expression
            // input = input.Where(whereParam.Value);

            foreach (var reading in input) 
            {
                var groupVal = reading.GetType().GetProperty(groupParam.Value).GetValue(reading, null).ToString();
                if (!string.IsNullOrEmpty(groupVal)) 
                {
                    if (!results.Select(x => x.Name).Contains(groupVal))
                    {
                        var newResult = new FilterResult()
                        {
                            Name = groupVal,
                            Data = new List<SensorReading>() { reading }
                        };
                        results.Add(newResult);
                    }
                    else
                    {
                        var existingResult = results.Where(x => x.Name.Equals(groupVal)).FirstOrDefault();
                        existingResult.Data.Add(reading);
                    }
                }
            }

			return results;
		}
	}
}