using System;
using System.Collections.Generic;
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
                bool isMatch = true, splitOutput = false;
                string splitField;

                foreach (var param in parameters)
				{
					var filterField = record.GetType().GetProperty(param.Field)
                                            .GetValue(record, null).ToString();
                    foreach (var clause in param.Clauses)
                    {
                        switch (clause.Key)
						{
							case Constants.COMMAND_PARAM_CONTAINS:
								isMatch = (filterField.Contains(clause.Value));
								break;
							case Constants.COMMAND_PARAM_EQUAL_TO:
								isMatch = (filterField.Equals(clause.Value));
								break;
							case Constants.COMMAND_PARAM_NOT_EQUAL_TO:
								isMatch = (!filterField.Equals(clause.Value));
								break;
							case Constants.COMMAND_PARAM_SPLIT:
								if (!splitOutput) // only set once
								{
									splitOutput = true;
									splitField = param.Field;
                                }
                                break;
                        }
					}

                    // if we still don't have a match, start over with next record
                    // (ends parameter for-each)
					if (!isMatch) break;

					// if valid match AND split output, proceed with output split
					if (splitOutput)
					{
						if (!results.Select(x => x.Name).Contains(filterField))
						{
							var newResult = new FilterResult()
							{
								Name = filterField,
								Data = new List<SensorReading>() { record }
							};
							results.Add(newResult);
						}
						else
						{
							var existingResult = results.Where(x => x.Name.Equals(filterField)).FirstOrDefault();
							existingResult.Data.Add(record);
						}
                    }
                }
            }

			return results;
		}
	}
}