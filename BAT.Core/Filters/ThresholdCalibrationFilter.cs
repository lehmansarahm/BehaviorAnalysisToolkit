using System;
using System.Collections.Generic;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Constants;

namespace BAT.Core.Filters
{
    public class ThresholdCalibrationFilter : IFilter
	{
		/// <summary>
		/// Gets the get header.
		/// </summary>
		/// <value>The get header.</value>
        public string[] Header => CalibrationResult.Header;

		/// <summary>
		/// Gets the get header csv.
		/// </summary>
		/// <value>The get header csv.</value>
		public string HeaderCsv => CalibrationResult.HeaderCsv;

		/// <summary>
		/// Gets or sets the calibrations.
		/// </summary>
		/// <value>The calibrations.</value>
		public List<KeyValuePair<string, decimal>> CalibratedThresholds { get; set; }

		/// <summary>
		/// Filter the specified phaseInput.
		/// </summary>
		/// <returns>The filter.</returns>
		/// <param name="phaseInput">Phase input.</param>
		public IEnumerable<PhaseData<SensorReading>> Filter(PhaseInput<SensorReading> phaseInput)
		{
			CalibratedThresholds = new List<KeyValuePair<string, decimal>>();
            var calibrationResults = new List<CalibrationResult>();

            foreach (var input in phaseInput.Input)
			{
				foreach (var param in phaseInput.Parameters)
				{
					var calibrationField = typeof(SensorReading).GetProperty(param.Field);
					var fieldTypeCode = Type.GetTypeCode(calibrationField.PropertyType);
					var calibStep = param.GetClauseValue(CommandParameters.Step);
					var calibPercentage = param.GetClauseValue(CommandParameters.Percentage);

					if (fieldTypeCode == TypeCode.Decimal && calibStep != null && calibPercentage != null)
					{
                        var calibRecords = input.Data.Where(x => x.Label.Contains(calibStep)).ToList();
						var calibVals = calibRecords.Select(x => (decimal)calibrationField.GetValue(x, null)).ToList();
						var avgVal = MathService.Average(calibVals);
						var threshVal = avgVal * (decimal.Parse(calibPercentage) / 100.0M);

						CalibratedThresholds.Add(new KeyValuePair<string, decimal>(param.Field, threshVal));
						calibrationResults.Add(new CalibrationResult
						{
							Source = input.Name,
							AvgVal = avgVal,
							ThresholdVal = threshVal
						});
					}
				}

				// dump own results to file
				CsvFileWriter.WriteResultsToFile
							 (new string[] { OutputDirs.Filters, "ThresholdCalibration" },
                              input.Name, HeaderCsv, calibrationResults);
            }

            // return empty list ... we don't actually want to 
            // affect the post-phase data set
            return new List<PhaseData<SensorReading>>();
        }

        /// <summary>
        /// Calibrates the parameters.
        /// </summary>
        /// <returns>The parameters.</returns>
        public static List<Parameter> CalibrateParameters(List<Parameter> inputParams, 
                                                          IEnumerable<KeyValuePair<string, decimal>> calibrationData)
        {
            foreach (var inputParam in inputParams)
            {
                var fieldCalibrationData = calibrationData.Where(x => x.Key.Equals(inputParam.Field));
                var thresholdData = inputParam.Clauses.Where((x => x.Key.Equals(CommandParameters.Threshold)));

                if (fieldCalibrationData.Any() && thresholdData.Any())
                {
                    var oldThresholdClause = thresholdData.FirstOrDefault();
                    var newThresholdClause = new KeyValuePair<string,string>
                        (oldThresholdClause.Key, fieldCalibrationData.FirstOrDefault().Value.ToString());
                    inputParam.Clauses.Remove(oldThresholdClause);
                    inputParam.Clauses.Add(newThresholdClause);
                }
            }

            return inputParams;
        }
    }

    class CalibrationResult : ICsvWritable
    {
        public string Source { get; set; }
        public decimal AvgVal { get; set; }
        public decimal ThresholdVal { get; set; }

        public static string[] Header => new string[] { "Source", "Avg. Val.", "Threshold Val." };
        public static string HeaderCsv => string.Join(",", Header);

        public string[] CsvArray => new string[] { Source, AvgVal.ToString(), ThresholdVal.ToString() };
        public string CsvString => string.Join(",", CsvArray);
    }
}