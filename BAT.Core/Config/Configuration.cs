using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAT.Core.Analyzers;
using BAT.Core.Common;
using BAT.Core.Filters;
using BAT.Core.Summarizers;
using BAT.Core.Transformers;
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
					LogManager.Debug($"Input file: {key} \n\t... contains "
                                     + $"{InputData[key].Count()} records.");
				}

				return true;
            } 
            catch (Exception e) 
            {
                LogManager.Error("Fatal error encountered while loading input data.  Exiting program.", e, this);
				return false;
            }
        }

        /// <summary>
        /// Runs the transformers.
        /// </summary>
        /// <returns><c>true</c>, if transformers was run, <c>false</c> otherwise.</returns>
        /// <param name="writeOutputToFile">If set to <c>true</c> write output to file.</param>
        public bool RunTransformers(bool writeOutputToFile = false) 
        {
            bool success = false;
            if (Transformers?.Count > 0 && InputData?.Keys?.Count >= 1) 
            {
                // iterate through the list of transformers and run on each input data set
                var transformedData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var transformerName in Transformers)
				{
					Type transformerType;
					ITransformer transformer;

					try
					{
						transformerType = Type.GetType(Constants.NAMESPACE_TRANSFORMER_IMPL + transformerName);
						transformer = (ITransformer)Activator.CreateInstance(transformerType);
                        success = true;
					}
					catch (ArgumentNullException ex)
					{
						LogManager.Error("Could not create instance of provided transformer.  "
										+ "Please double-check configuration file.", ex, this);
                        continue; // proceed to next operation
					}

                    foreach (var key in InputData.Keys) 
                    {
                        var transformedValues = transformer.Transform(InputData[key]);
                        if (transformedData.ContainsKey(key))
                            transformedData[key] = transformedValues;
                        else transformedData.Add(key, transformedValues);

						if (writeOutputToFile)
                            CsvFileWriter.WriteToFile(new string[] { Constants.OUTPUT_DIR_TRANSFORMERS, transformerName },
                                                      key, transformer.GetHeaderCsv(),
                                                      transformedValues);
                    }
                }

                InputData = transformedData;
            } 
            else LogManager.Error("No input data to run transformations on.", this);

            return success;
        }

        /// <summary>
        /// Runs the filters.
        /// </summary>
        /// <returns><c>true</c>, if filters was run, <c>false</c> otherwise.</returns>
        /// <param name="writeOutputToFile">If set to <c>true</c> write output to file.</param>
		public bool RunFilters(bool writeOutputToFile = false) 
        {
            bool success = false;
			if (Filters?.Count > 0 && InputData?.Keys?.Count >= 1) 
            {
                var filteredData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var filterCommand in Filters) 
                {
                    string filterName;
                    Type filterType;
                    IFilter filter;

                    try
					{
						filterName = filterCommand.Name;
						filterType = Type.GetType(Constants.NAMESPACE_FILTER_IMPL + filterName);
						filter = (IFilter)Activator.CreateInstance(filterType);
						success = true;
                    }
                    catch (ArgumentNullException ex)
                    {
                        LogManager.Error("Could not create instance of provided filter.  "
										+ "Please double-check configuration file.", ex, this);
						continue; // proceed to next operation
					}
                    
					foreach (var key in InputData.Keys)
					{
                        IEnumerable<PhaseResult<SensorReading>> filteredResultSets = null;
                        if (filterCommand.Parameters != null)
                            filteredResultSets = filter.Filter(InputData[key], filterCommand.Parameters);

                        if (filteredResultSets != null)
						{
							foreach (var filterResult in filteredResultSets)
							{
								var filenameComponents = key.Split('.');
								var fileName = filenameComponents[filenameComponents.Length - 2];
								var fileExtension = filenameComponents[filenameComponents.Length - 1];
								var newFilename = $"{fileName}_{filterResult.Name}.{fileExtension}";

								var filteredValues = filterResult.Data;
								if (filteredData.ContainsKey(newFilename))
									filteredData[newFilename] = filteredValues;
								else filteredData.Add(newFilename, filteredValues);

								if (writeOutputToFile)
									CsvFileWriter.WriteToFile(new string[] { Constants.OUTPUT_DIR_FILTERS, filterName }, 
                                                              newFilename, filter.GetHeaderCsv(),
															  filteredValues);
							}
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
		/// <param name="writeOutputToFile">If set to <c>true</c> write output to file.</param>
		public bool RunAnalyzers(bool writeOutputToFile = false)
		{
			bool success = false;
			if (Analyzers?.Count > 0 && InputData?.Keys?.Count >= 1)
			{
                var analyzedData = new Dictionary<string, IEnumerable<ICsvWritable>>();
				foreach (var analyzerCommand in Analyzers)
				{
					string analyzerName;
					Type analyzerType;
					IAnalyzer analyzer;

					try
					{
						analyzerName = analyzerCommand.Name;
						analyzerType = Type.GetType(Constants.NAMESPACE_ANALYZER_IMPL + analyzerName);
						analyzer = (IAnalyzer)Activator.CreateInstance(analyzerType);
						success = true;
					}
					catch (ArgumentNullException ex)
					{
						LogManager.Error("Could not create instance of provided analyzer.  "
										+ "Please double-check configuration file.", ex, this);
						continue; // proceed to next operation
					}

					foreach (var key in InputData.Keys)
					{
						var analysisResult = analyzer.Analyze(InputData[key], analyzerCommand.Parameters);
						if (analyzedData.ContainsKey(key)) analyzedData[key] = analysisResult;
						else analyzedData.Add(key, analysisResult);

						if (writeOutputToFile)
							CsvFileWriter.WriteToFile(new string[] { Constants.OUTPUT_DIR_ANALYZERS, analyzerName }, 
                                                      key, analyzer.GetHeaderCsv(),
													  analysisResult);
					}
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
        /// <param name="writeOutputToFile">If set to <c>true</c> write output to file.</param>
		public bool RunSummarizers(bool writeOutputToFile = false)
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
						summarizerType = Type.GetType(Constants.NAMESPACE_SUMMARIZER_IMPL + summarizerName);
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
                        CsvFileWriter.WriteToFile(new string[] { Constants.OUTPUT_DIR_SUMMARIZERS },
                                                  $"{summarizerName}.csv",
												  summarizer.GetHeaderCsv(),
												  summarizedValues);
				}
			}
			else LogManager.Error("No input data to run summaries on.", this);

			return success;
        }

		/// <summary>
		/// Loads from file.
		/// </summary>
		/// <returns>The from file.</returns>
		/// <param name="filepath">Filepath.</param>
		public static Configuration LoadFromFile(string filepath)
		{
			var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(filepath));
            return config;
		}
    }
}