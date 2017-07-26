using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Summarizers;
using Newtonsoft.Json;

namespace BAT.Core.Config
{
    public class Configuration
    {
        [JsonProperty("inputs")]
        public List<string> Inputs { get; set; }
        public Dictionary<string, IEnumerable<SensorReading>> InputData { get; set; }

        [JsonProperty("transformers")]
        public List<string> Transformers { get; set; }

        [JsonProperty("filters")]
        public List<Command> Filters { get; set; }

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
            Inputs = new List<string>();
            Transformers = new List<string>();
            Filters = new List<Command>();
            Analyzers = new List<Command>();
            Summarizers = new List<string>();
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
                string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                InputData = new Dictionary<string, IEnumerable<SensorReading>>();

                foreach (var inputFile in Inputs)
                {
                    // split out the actual file name from the config input
                    var inputFileComponents = inputFile.Split('/');
                    var inputFileName = inputFileComponents[inputFileComponents.Length - 1];

                    // read in the sensor data
                    var sensorReadings = SensorReading.ReadSensorFile(currentDir + "/" + inputFile);

                    // add data to collection using file name, not file path
                    InputData.Add(inputFileName, sensorReadings);
                }

                // confirm that everything worked
                LogManager.Debug($"{InputData.Keys.Count} files processed.");
                foreach (var key in InputData.Keys)
                {
                    LogManager.Debug($"Input file: {key} \n\t... contains {InputData[key].Count()} records.");
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
			bool success = false;
            var transformers = TransformerManager.GetTransformers(Transformers);

            if (transformers?.Any() ?? false && InputData?.Keys?.Count >= 1)
            {
                // iterate through the list of transformers and run on each input data set
                var transformedData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var transformer in transformers)
                {
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
            bool success = false;
            if (Filters?.Count > 0 && InputData?.Keys?.Count >= 1)
            {
                var filteredData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var filterCommand in Filters)
                {
                    var filter = FilterManager.GetFilter(filterCommand.Name);
                    if (filter == null) continue;

                    foreach (var key in InputData.Keys)
                    {
                        IEnumerable<PhaseResult<SensorReading>> filteredResultSets = null;
                        if (filterCommand.Parameters != null)
                            filteredResultSets = filter.Filter(InputData[key], filterCommand.Parameters);

                        if (filteredResultSets != null && filteredResultSets.Any())
                        {
                            foreach (var filterResult in filteredResultSets)
                            {
								var filteredValues = filterResult.Data;
								var newFilename = FilterManager.GetFilename(key, filterResult.Name);

                                if (filteredData.ContainsKey(newFilename))
                                    filteredData[newFilename] = filteredValues;
                                else filteredData.Add(newFilename, filteredValues);

                                if (WriteOutputFile)
                                    CsvFileWriter.WriteResultsToFile
                                                 (new string[] { OutputDirs.Filters, filterCommand.Name },
                                                  newFilename, filter.GetHeaderCsv(), filteredValues);
							}

							success = true;
                        }
                    }
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
            bool success = false;
            if (Analyzers?.Count > 0 && InputData?.Keys?.Count >= 1)
            {
                var analyzedData = new Dictionary<string, IEnumerable<ICsvWritable>>();
                foreach (var analyzerCommand in Analyzers)
                {
                    var analyzerName = analyzerCommand.Name;
                    var analyzer = AnalyzerManager.GetAnalyzer(analyzerName);
                    if (analyzer == null) continue;

                    foreach (var key in InputData.Keys)
                    {
                        var analysisResult = analyzer.Analyze(InputData[key], analyzerCommand.Parameters);
                        if (analyzedData.ContainsKey(key))
                            analyzedData[key] = analysisResult;
                        else analyzedData.Add(key, analysisResult);

                        if (WriteOutputFile)
                            CsvFileWriter.WriteResultsToFile
                                         (new string[] { OutputDirs.Analyzers, analyzerName },
                                          key, analyzer.GetHeaderCsv(), analysisResult);

	                    // now, check to see if there is an associated summary
	                    if (Summarizers.Contains(analyzerName))
						{
                            ISummarizer summarizer = SummarizerManager.GetSummarizer(analyzerName);
	                        var summarizedValues = summarizer.Summarize(analyzedData);
	                        if (WriteOutputFile)
	                            CsvFileWriter.WriteSummaryToFile
                                             (new string[] { OutputDirs.Summarizers },
                                              $"{analyzerName}.csv", summarizer.GetHeaderCsv(),
                                              summarizedValues, summarizer.GetFooterCsv(),
                                              summarizer.GetFooterValues());
	                    }
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
        /// Runs the summarizers.
        /// </summary>
        /// <returns><c>true</c>, if summarizers was run, <c>false</c> otherwise.</returns>
        /*public bool RunSummarizers(bool writeOutputToFile = false)
        {
            bool success = false;
            if (Summarizers?.Count > 0 && AnalysisData?.Keys?.Count >= 1)
            {
                foreach (var summarizerName in Summarizers)
                {
                    Type summarizerType;
                    ISummarizer summarizer;

                    try
                    {
                        summarizerType = Type.GetType(Constants.NAMESPACE_SUMMARIZER_IMPL +
                                                      summarizerName + Constants.PHASE_IMPL_SUMMARIZER);
                        summarizer = (ISummarizer)Activator.CreateInstance(summarizerType);
                        success = true;
                    }
                    catch (ArgumentNullException ex)
                    {
                        LogManager.Error("Could not create instance of provided summarizer.  "
                                        + "Please double-check configuration file.", ex, this);
                        continue; // proceed to next operation
                    }

                    var summarizedValues = summarizer.Summarize(AnalysisData);
                    if (writeOutputToFile)
                        CsvFileWriter.WriteSummaryToFile(new string[] { Constants.OUTPUT_DIR_SUMMARIZERS },
                                                         $"{summarizerName}.csv",
                                                         summarizer.GetHeaderCsv(),
                                                         summarizedValues,
                                                         summarizer.GetFooterCsv(),
                                                         summarizer.GetFooterValues());
                }
            }
            else LogManager.Error("No input data to run summaries on.", this);

            return success;
        }*/

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
                return config;
            }
            catch (FileNotFoundException ex)
            {
                LogManager.Error($"Could not locate input file: {filepath}.  "
                                 + "Please double check file name / path.",
                                 ex, typeof(Configuration));
                return new Configuration();
            }
            catch (JsonReaderException ex)
            {
                LogManager.Error($"Could not parse configuration object from "
                                 + "input file: {filepath}.  Is input properly formatted?",
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