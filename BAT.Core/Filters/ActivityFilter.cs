using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Filters
{
	public class ActivityFilter : IFilter
	{
        /// <summary>
        /// Gets the get header.
        /// </summary>
        /// <value>The get header.</value>
        public string[] Header => SensorReading.Header;

        /// <summary>
        /// Gets the get header csv.
        /// </summary>
        /// <value>The get header csv.</value>
		public string HeaderCsv => SensorReading.HeaderCsv;

        /// <summary>
        /// Filter the specified phaseInput.
        /// </summary>
        /// <returns>The filter.</returns>
        /// <param name="phaseInput">Phase input.</param>
        public IEnumerable<PhaseData<SensorReading>> Filter(PhaseInput<SensorReading> phaseInput)
		{
            var results = new List<PhaseData<SensorReading>>();
            foreach (var input in phaseInput.Input)
			{
                // input = name plus collection of data records
				foreach (var record in input.Data)
				{
                    // each data record is a single sensor reading
					foreach (var param in phaseInput.Parameters)
					{
						// check to see if desired field exists for this reading
						// if not, proceed to next parameter
						var filterField = record.GetType().GetProperty(param.Field);
						if (filterField == null) continue;

						// if it does exist, check to see if value satisfies clause
                        // collection for this parameter ...
						var filterValue = filterField.GetValue(record, null).ToString();
						var isMatch = param.MatchesClause(filterValue);

						// if we fail to match, start over with next record
						// (ends parameter for-each)
						if (!isMatch) break;

						// if valid match AND user chose to split output, 
                        // update result names to produce output split
						string resultName = (param.UseOutputSplit() 
                                             ? FilterManager.GetFilterFilename(input.Name, filterValue)
                                             : input.Name);
						if (!results.Select(x => x.Name).Contains(resultName))
						{
							var newResult = new PhaseData<SensorReading>
							{
								Name = resultName,
								Data = new List<SensorReading> { record }
							};
							results.Add(newResult);
						}
						else
						{
							var existingResult = results.Where(x => x.Name.Equals(resultName)).FirstOrDefault();
							existingResult.Data.Add(record);
						}
					}
				}
            }

			return results;
		}
	}
}