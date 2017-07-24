using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Filters.Impl
{
    public class CompletionFilter : IFilter
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
		public string[] GetHeader()
		{
			return SensorReading.Header;
		}

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv()
		{
			return SensorReading.HeaderCsv;
		}

		/// <summary>
		/// Filter the specified input and parameters.
		/// </summary>
		/// <returns>The filter.</returns>
		/// <param name="input">Input.</param>
		/// <param name="parameters">Parameters.</param>
		public IEnumerable<PhaseResult> Filter(IEnumerable<ICsvWritable> input, 
                                                IEnumerable<Parameter> parameters)
		{
			var results = new List<PhaseResult>();
			/*foreach (var record in input)
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
					isMatch = param.MatchesClause(filterValue);

					// if we still don't have a match, start over with next record
					// (ends parameter for-each)
					if (!isMatch) break;

					// if valid match AND split output, proceed with output split
					if (param.UseOutputSplit())
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
			}*/

			return results;
        }
    }
}