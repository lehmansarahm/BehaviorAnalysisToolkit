using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;

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
                                                IEnumerable<Parameter> parameters)
		{
            var results = new List<FilterResult>();
            foreach (var record in input)
			{
                bool isMatch = true;

                foreach (var param in parameters)
				{
                    // check to see if desired field exists for this object
                    // if not, proceed to next parameter
                    var filterField = record.GetType().GetProperty(param.Field);
                    if (filterField == null) continue;

                    // if it does exist, continue with processing ...
                    var filterValue = filterField.GetValue(record, null).ToString();
                    foreach (var clause in param.Clauses)
                    {
                        switch (clause.Key)
						{
							case Constants.COMMAND_PARAM_CONTAINS:
                                isMatch = (Constants.CULTURE.CompareInfo.IndexOf(filterValue, 
                                                                                 clause.Value, CompareOptions.IgnoreCase) >= 0);
								break;
							case Constants.COMMAND_PARAM_EQUAL_TO:
								isMatch = (filterValue.Equals(clause.Value, 
                                                              StringComparison.InvariantCultureIgnoreCase));
								break;
							case Constants.COMMAND_PARAM_NOT_EQUAL_TO:
								isMatch = (!filterValue.Equals(clause.Value, 
                                                               StringComparison.InvariantCultureIgnoreCase));
								break;
                        }
					}

                    // if we still don't have a match, start over with next record
                    // (ends parameter for-each)
					if (!isMatch) break;

					// if valid match AND split output, proceed with output split
					bool splitOutput = param.Clauses.Where(x => x.Key.Contains(Constants.COMMAND_PARAM_SPLIT))
											.FirstOrDefault().Value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
					if (splitOutput)
					{
						if (!results.Select(x => x.Name).Contains(filterValue))
						{
							var newResult = new FilterResult()
							{
								Name = filterValue,
								Data = new List<SensorReading>() { record }
							};
							results.Add(newResult);
						}
						else
						{
							var existingResult = results.Where(x => x.Name.Equals(filterValue)).FirstOrDefault();
							existingResult.Data.Add(record);
						}
                    }
                }
            }

			return results;
		}
	}
}