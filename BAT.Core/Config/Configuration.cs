using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Constants;
using BAT.Core.Filters;
using BAT.Core.Summarizers;
using Newtonsoft.Json;

namespace BAT.Core.Config
{
    public class Configuration
    {
        [JsonProperty("inputs")]
		public List<UserInput> Inputs { get; set; }
		public Dictionary<string, IEnumerable<SensorReading>> InputData { get; set; }

        [JsonProperty("transformers")]
        public List<string> Transformers { get; set; }

        [JsonProperty("filters")]
		public List<Command> Filters { get; set; }
		public Dictionary<string, IEnumerable<KeyValuePair<string, decimal>>> CalibrationData { get; set; }

        [JsonProperty("analyzers")]
        public List<Command> Analyzers { get; set; }
        public Dictionary<string, IEnumerable<ICsvWritable>> AnalysisData { get; set; }

        [JsonProperty("summarizers")]
        public List<string> Summarizers { get; set; }

        public bool WriteOutputFile { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BAT.Core.Config.Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            Inputs = new List<UserInput>();
            Transformers = new List<string>();
            Filters = new List<Command>();
            Analyzers = new List<Command>();
            Summarizers = new List<string>();

            InputData = new Dictionary<string, IEnumerable<SensorReading>>();
            CalibrationData = new Dictionary<string, IEnumerable<KeyValuePair<string, decimal>>>();
            AnalysisData = new Dictionary<string, IEnumerable<ICsvWritable>>();
        }

        /// <summary>
        /// Loads the inputs.
        /// </summary>
        /// <returns><c>true</c>, if inputs was loaded, <c>false</c> otherwise.</returns>
        public bool LoadInputs()
        {
            bool success = false;

            try
            {
                InputData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var input in Inputs)
                {
                    foreach (var inputFile in input.InputFiles)
					{
						// read in the sensor data
                        var sensorReadings = SensorReading.ReadSensorFile(inputFile.Value);

                        // verify input - only take records between the "start" and "quit" flags
                        var startRecord = sensorReadings.FirstOrDefault(x => x.Start);
                        if (startRecord != null)
                            sensorReadings = sensorReadings.Where(x => x.RecordNum >= startRecord.RecordNum).ToList();
						var quitRecord = sensorReadings.FirstOrDefault(x => x.End);
						if (quitRecord != null)
							sensorReadings = sensorReadings.Where(x => x.RecordNum <= quitRecord.RecordNum).ToList();

						// add data to collection using file name, not file path
                        InputData.Add(inputFile.Key, sensorReadings);
                    }
                }

                // run any initialization methods for selected modules
                foreach (var summarizer in Summarizers)
                    SummarizerManager.GetSummarizer(summarizer).Initialize(InputData);

                // confirm that everything worked
                LogManager.Debug($"{InputData.Keys.Count} files processed.");
                foreach (var key in InputData.Keys)
                {
                    LogManager.Debug($"Input file: {key} ... contains {InputData[key].Count()} records.");
                }

                success = InputData.Any();
            }
            catch (Exception e)
            {
                LogManager.Error("Fatal error encountered while loading input data.  Exiting program.", e, this);
            }

            return success;
        }

        /// <summary>
        /// Runs the transformers.
        /// </summary>
        /// <returns><c>true</c>, if transformers was run, <c>false</c> otherwise.</returns>
        public bool RunTransformers()
		{
            if (!Transformers.Any())
            {
                LogManager.Info("No transformation operations configured.", this);
                return true;
            }

            bool success = false;
			var transformers = TransformerManager.GetTransformers(Transformers);

            if (transformers?.Any() ?? false && InputData?.Keys?.Count >= 1)
            {
                // iterate through the list of transformers and run on each input data set
                var transformedData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var transformer in transformers)
				{
					// Sanity checking ...
                    LogManager.Info($"Running transformation operation:\n\t{transformer}", this);

                    // Run the operation ...
                    foreach (var key in InputData.Keys)
                    {
                        bool dataAlreadyProcessedForKey = transformedData.ContainsKey(key);
                        var inputData = (dataAlreadyProcessedForKey ? transformedData[key] : InputData[key]);
                        var transformedValues = transformer.Transform(inputData);

                        if (dataAlreadyProcessedForKey) transformedData[key] = transformedValues;
                        else transformedData.Add(key, transformedValues);

                        if (WriteOutputFile)
                            CsvFileWriter.WriteResultsToFile
                                         (new[] { OutputDirs.Transformers, transformer.GetType().Name },
                                          key, transformer.GetHeaderCsv(), transformedValues);
                    }
                }

                InputData = transformedData;
                success = true;
            }
            else LogManager.Error("No input data to run transformations on.", this);

            return success;
        }

        /// <summary>
        /// Runs the filters.
        /// </summary>
        /// <returns><c>true</c>, if filters was run, <c>false</c> otherwise.</returns>
        public bool RunFilters()
		{
			bool success = true;
			if (!Filters.Any())
			{
				LogManager.Info("No filter operations configured.", this);
                return success;
			}

            if (Filters?.Count > 0 && InputData?.Keys?.Count >= 1)
			{
				var filteredData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var filterCommand in Filters)
                {
                    var filter = FilterManager.GetFilter(filterCommand.Name);
                    if (filter == null)
                    {
                        success = false;
                        continue;
                    }

                    // Sanity checking ...
                    LogManager.Info($"Running filter operation:\n\t{filter}", this);

                    // Processing the data for the phase...
                    if (filterCommand.Parameters != null)
                    {
                        // retrieving the input for the phase
                        var phaseData = new List<PhaseData<SensorReading>>();
                        foreach (var key in InputData.Keys)
                        {
                            phaseData.Add(new PhaseData<SensorReading>
                            {
                                Name = key,
                                Data = InputData[key].ToList()
                            });
                        }

	                    // INPUT = COMMANDS, SET OF FILE NAMES AND DATA VALUES
	                    // OUTPUT = SET OF FILE NAMES AND NEW DATA VALUES

                        var filteredResultSets = filter.Filter(new PhaseInput<SensorReading>
                        {
                            Input = phaseData,
                            Parameters = filterCommand.Parameters
						});

						// list of calibrated thresholds for various fields
						// ONLY APPLICABLE TO THIS INDIVIDUAL INPUT DATA SET!!
						/*if (filter is ThresholdCalibrationFilter calibFilter)
                        {
                            var thresholdValues = calibFilter.CalibratedThresholds;
                            CalibrationData.Add(phaseInput.Name, thresholdValues);
                        }*/

						if (filteredResultSets != null && filteredResultSets.Any())
                        {
                            foreach (var filterResult in filteredResultSets)
                            {
                                filteredData[filterResult.Name] = filterResult.Data;
                                if (WriteOutputFile)
                                    CsvFileWriter.WriteResultsToFile
                                         (new string[] { OutputDirs.Filters, filterCommand.Name },
                                          filterResult.Name, filter.HeaderCsv, filterResult.Data);
                            }
                        }
                        else success = false;
                    }
                    else success = false;
                }

                InputData = filteredData;
            }
            else LogManager.Error("No input data to run filters on.", this);

            return success;
        }

        /// <summary>
        /// Runs the analyzers.
        /// </summary>
        /// <returns><c>true</c>, if analyzers was run, <c>false</c> otherwise.</returns>
        public bool RunAnalyzers()
		{
            if (!Analyzers.Any())
			{
				LogManager.Info("No analysis operations configured.", this);
				return true;
			}

			bool success = false;
			if (Analyzers?.Count > 0 && InputData?.Keys?.Count >= 1)
            {
                var analyzedData = new Dictionary<string, IEnumerable<ICsvWritable>>();
                foreach (var analyzerCommand in Analyzers)
				{
					// ---------------------------------------------------------
					//          Retrieve analysis operation details
					// ---------------------------------------------------------
					var analyzerName = analyzerCommand.Name;
                    var analyzer = AnalyzerManager.GetAnalyzer(analyzerName);
                    if (analyzer == null) continue;

					// Sanity checking ...
					LogManager.Info($"Running analysis operation:\n\t{analyzer}", this);

					// ---------------------------------------------------------
					//      Check to see if user requested a summary
					// ---------------------------------------------------------
					bool summaryRequested = Summarizers.Contains(analyzerName);

                    // ---------------------------------------------------------
                    //          Group input data by original file name
                    // ---------------------------------------------------------
                    var consolidatedInputFiles = 
                        Inputs.SelectMany(x => x.InputFiles.Select(y => y.Key)).ToList();
                    foreach (var origInputFile in consolidatedInputFiles)
					{
						var analysisDataByInputFile = new Dictionary<string, IEnumerable<ICsvWritable>>();
                        var analysisKeysByInputFile = InputData.Keys.Where(x => x.Contains(origInputFile));
                        foreach (var key in analysisKeysByInputFile)
						{
                            // -------------------------------------------------
                            //      Perform the actual analysis operation
                            // -------------------------------------------------
                            if (analyzerCommand.HasParameters &&
                                FilterManager.ContainsFilter(Filters, typeof(ThresholdCalibrationFilter)))
                            {
                                var calibData = CalibrationData[origInputFile];
                                analyzerCommand.Parameters =
                                    ThresholdCalibrationFilter.CalibrateParameters
                                        (analyzerCommand.Parameters, calibData);
                            }

							// -------------------------------------------------
							//      Any required analyzer-specific prep
							// -------------------------------------------------
							if (analyzer is Analyzers.SciKitPrepAnalysis skpAnalyzer)
								skpAnalyzer.CurrentInput = key;
							else if (analyzer is Analyzers.SciKitEvalAnalysis skeAnalyzer)
								skeAnalyzer.CurrentInput = key;
							// -------------------------------------------------

							var analysisResult =
								analyzer.Analyze(InputData[key], analyzerCommand.Parameters);
							analysisDataByInputFile[key] = analysisResult;

                            // -------------------------------------------------
                            //          Dump output to file if necessary
                            // -------------------------------------------------
							if (WriteOutputFile)
								CsvFileWriter.WriteResultsToFile
											 (new string[] { OutputDirs.Analyzers, analyzerName },
                                              key, analyzer.HeaderCsv, analysisResult);

							// -------------------------------------------------
							//  If requested, summarize file-specific results
							// -------------------------------------------------
							if (summaryRequested)
							{
								ISummarizer summarizer = 
                                    SummarizerManager.GetSummarizer(analyzerName);
								var summarizedValues = 
                                    summarizer.Summarize(analysisDataByInputFile);
                                
								if (WriteOutputFile)
									CsvFileWriter.WriteSummaryToFile
                                                 (new string[] { OutputDirs.Summarizers, analyzerName },
                                                  $"{origInputFile}{Constants.BAT.DEFAULT_INPUT_FILE_EXT}",
												  summarizer.HeaderCsv, summarizedValues,
												  summarizer.FooterCsv, summarizer.FooterValues);
							}
						}

                        // consolidate the results by input file
                        var inputFileResults = analyzer.ConsolidateData(analysisDataByInputFile);
						if (analyzedData.ContainsKey(origInputFile))
							analyzedData[origInputFile] = inputFileResults;
						else analyzedData.Add(origInputFile, inputFileResults);
					}

					// -----------------------------------------------------
					// If requested, aggregate all file-specific summaries 
					// into one high level file in root summarizer directory
					// -----------------------------------------------------
					if (summaryRequested)
					{
						ISummarizer summarizer = SummarizerManager.GetSummarizer(analyzerName);
						var summarizedValues = summarizer.Summarize(analyzedData);
						if (WriteOutputFile)
							CsvFileWriter.WriteSummaryToFile
										 (new string[] { OutputDirs.Summarizers },
										  $"{analyzerName}Aggregate{Constants.BAT.DEFAULT_INPUT_FILE_EXT}",
										  summarizer.HeaderCsv, summarizedValues,
										  summarizer.FooterCsv, summarizer.FooterValues);
					}

                    success = true;
                }

                // use different collection to maintain integrity of original input data
                AnalysisData = analyzedData;
            }
            else LogManager.Error("No input data to run analyzers on.", this);

            return success;
        }

        /// <summary>
        /// Loads from file.
        /// </summary>
        /// <returns>The from file.</returns>
        /// <param name="filepath">Filepath.</param>
        public static Configuration LoadFromFile(string filepath)
        {
            try
            {
                var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(filepath));
                CsvFileWriter.CopyFileToDir(filepath, OutputDirs.ExecTime);
                return config;
            }
            catch (FileNotFoundException ex)
            {
                LogManager.Error($"Could not locate input file: {filepath}.  "
                                 + "Please double check file name and path.",
                                 ex, typeof(Configuration));
                return new Configuration();
            }
            catch (JsonReaderException ex)
            {
                LogManager.Error("Could not parse configuration object from "
                                 + $"input file: {filepath}.  Is input properly formatted?",
                                 ex, typeof(Configuration));
                return new Configuration();
            }
            catch (JsonSerializationException ex)
            {
                LogManager.Error("Error encountered while attempting to serialize "
                                 + $"configuration object from input file: {filepath}.  "
                                 + "Is input complete?",
                                 ex, typeof(Configuration));
                return new Configuration();
            }
        }
    }
}