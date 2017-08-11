using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Filters
{
    public class CompletionFilter : IFilter
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
            var param = phaseInput.Parameters.FirstOrDefault();
            if (param == null) return null;

            var filterField = typeof(SensorReading).GetProperty(param.Field);
            var success = decimal.TryParse(param.GetClauseValue(CommandParameters.Percentage), out decimal completionPercentage);
            if (filterField == null || !success)
                return null;

            // find a list of distinct values of the desired field across all data sets
            var distinctValues = phaseInput.Input.Select(x => x.Data.Select(y =>
                                 Parameter.GetFilterValue(filterField, y))).SelectMany(x => x).Distinct().ToList();

            // only include records from a given data set whose field values 
            // are present in a matching percentage of the other data sets
            for (int i = distinctValues.Count() - 1; i >= 0; i--)
            {
                var distinctValue = distinctValues[i];
				var matchingDataSetCount = phaseInput.Input.Count(x => x.Data.Select(y => Parameter.GetFilterValue(filterField, y)).Contains(distinctValue));
                var matchingPercentage = ((decimal)matchingDataSetCount / 
                                          (decimal)phaseInput.Input.Count()) * 100.0M;
                if (matchingPercentage < completionPercentage)
                    distinctValues.RemoveAt(i);
			}

			// double check ...
			LogManager.Info("The following distinct field values were found for Completion Filter:\n\t" +
			    string.Join("\n\t", distinctValues), this);

			// proceed with results gathering ...
			var results = new List<PhaseData<SensorReading>>();
            foreach (var input in phaseInput.Input)
            {
                foreach (var record in input.Data)
                {
                    var filterValue = Parameter.GetFilterValue(filterField, record);
                    var isMatch = distinctValues.Contains(filterValue);
					if (!isMatch) continue;

					// if valid match AND user chose to split output, 
					// update result names to produce output split
					string resultName = (param.UseOutputSplit()
										 ? FilterManager.GetFilterFilename(input.Name, filterValue)
										 : input.Name);

                    // add the data to the results list
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

			return results;
        }
    }
}