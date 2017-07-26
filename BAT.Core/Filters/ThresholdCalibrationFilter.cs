using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;

namespace BAT.Core.Filters
{
    public class ThresholdCalibrationFilter : IFilter
	{
		/// <summary>
		/// Gets the header.
		/// </summary>
		/// <returns>The header.</returns>
        public string[] GetHeader() { return CalibrationResult.Header; }

		/// <summary>
		/// Gets the header csv.
		/// </summary>
		/// <returns>The header csv.</returns>
		public string GetHeaderCsv() { return CalibrationResult.HeaderCsv; }

        /// <summary>
        /// Filter the specified input and parameters.
        /// </summary>
        /// <returns>The filter.</returns>
        /// <param name="input">Input.</param>
        /// <param name="parameters">Parameters.</param>
        public IEnumerable<PhaseResult<SensorReading>> Filter(string source,
                                                              IEnumerable<SensorReading> input,
                                                              IEnumerable<Parameter> parameters)
        {
            var calibrationResults = new List<CalibrationResult>();
            foreach (var param in parameters)
			{
                var calibrationField = typeof(SensorReading).GetProperty(param.Field);
                var fieldTypeCode = Type.GetTypeCode(calibrationField.PropertyType);
				var calibStep = param.GetClauseValue(CommandParameters.Step);
                var calibPercentage = param.GetClauseValue(CommandParameters.Percentage);

                if (fieldTypeCode == TypeCode.Decimal && calibStep != null && calibPercentage != null)
				{
					var calibRecords = input.Where(x => x.Label.Contains(calibStep)).ToList();
                    var calibVals = calibRecords.Select(x => (decimal)calibrationField.GetValue(x, null)).ToList();
					var avgVal = UtilityService.Average(calibVals);
                    var threshVal = avgVal * (decimal.Parse(calibPercentage) / 100.0M);

                    calibrationResults.Add(new CalibrationResult{
                        Source = source,
                        AvgVal = avgVal,
                        ThresholdVal = threshVal
                    });
				}
            }

            // dump own results to file
            CsvFileWriter.WriteResultsToFile
                         (new string[] { OutputDirs.Filters, "ThresholdCalibration" },
                          source, GetHeaderCsv(), calibrationResults);

            // return empty list ... we don't actually want to 
            // affect the post-phase data set
            return new List<PhaseResult<SensorReading>>();
        }
    }

    class CalibrationResult : ICsvWritable
    {
        public string Source { get; set; }
        public decimal AvgVal { get; set; }
        public decimal ThresholdVal { get; set; }

        public static string[] Header
        {
            get
			{
				return new string[] { "Source", "Avg. Val.", "Threshold Val." };
            }
        }

        public static string HeaderCsv
        {
            get
			{
                return string.Join(",", Header);
            }
        }

        public string ToCsv()
        {
            return string.Join(",", new string[]
            {
                Source, AvgVal.ToString(), ThresholdVal.ToString()
            });
        }
    }
}