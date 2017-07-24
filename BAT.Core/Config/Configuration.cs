using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAT.Core.Analyzers;
using BAT.Core.Common;
using BAT.Core.Filters;
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
                    var sensorReadings = SensorReading.ReadSensorFile(currentDir + "/" + inputFile);
                    InputData.Add(inputFile, sensorReadings);
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
            if (Transformers?.Count > 0 && InputData?.Keys?.Count >= 1) 
            {
                // iterate through the list of transformers and run on each input data set
                var transformedData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var transformerName in Transformers) 
                {
                    Type transformerType = 
                        Type.GetType(Constants.NAMESPACE_TRANSFORMER_IMPL + transformerName);
                    ITransformer transformer = 
                        (ITransformer)Activator.CreateInstance(transformerType);

                    foreach (var key in InputData.Keys) 
                    {
                        var transformedValues = transformer.Transform(InputData[key]);
                        if (transformedData.ContainsKey(key))
                            transformedData[key] = transformedValues;
                        else transformedData.Add(key, transformedValues);

						if (writeOutputToFile)
                            CsvFileWriter.WriteToFile(Constants.OUTPUT_DIR_TRANSFORMERS, 
                                                      transformerName, key, 
                                                      SensorReading.HeaderCsv,
                                                      transformedValues);
                    }
                }

                InputData = transformedData;
                return true;
            } 
            else 
            {
                LogManager.Error("No input data to run transformations on.", this);
                return false;
            }
        }

        /// <summary>
        /// Runs the filters.
        /// </summary>
        /// <returns><c>true</c>, if filters was run, <c>false</c> otherwise.</returns>
        /// <param name="writeOutputToFile">If set to <c>true</c> write output to file.</param>
		public bool RunFilters(bool writeOutputToFile = false) 
        {
			if (Filters?.Count > 0 && InputData?.Keys?.Count >= 1) 
            {
                var filteredData = new Dictionary<string, IEnumerable<SensorReading>>();
                foreach (var filterCommand in Filters) 
                {
                    string filterName = filterCommand.Name;
                    Type filterType =
						Type.GetType(Constants.NAMESPACE_FILTER_IMPL + filterName);
                    IFilter filter =
						(IFilter)Activator.CreateInstance(filterType);
                    
					foreach (var key in InputData.Keys) 
                    {
                        var filteredResultSets = filter.Filter(InputData[key], filterCommand.Parameters);
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
								CsvFileWriter.WriteToFile(Constants.OUTPUT_DIR_FILTERS,
														  filterName, newFilename,
														  SensorReading.HeaderCsv,
														  filteredValues);
                        }
					}
				}

				InputData = filteredData;
				return true;
			} 
            else 
            {
				LogManager.Error("No input data to run filters on.", this);
				return false;
			}
        }

        /// <summary>
        /// Runs the analyzers.
        /// </summary>
        /// <returns><c>true</c>, if analyzers was run, <c>false</c> otherwise.</returns>
        /// <param name="writeOutputToFile">If set to <c>true</c> write output to file.</param>
		public bool RunAnalyzers(bool writeOutputToFile = false) {
			if (Analyzers?.Count > 0 && InputData?.Keys?.Count >= 1) {
				foreach (var analyzerCommand in Analyzers)
				{
					string analyzerName = analyzerCommand.Name;
                    Type analyzerType =
						Type.GetType(Constants.NAMESPACE_ANALYZER_IMPL + analyzerName);
                    IAnalyzer analyzer =
                        (IAnalyzer)Activator.CreateInstance(analyzerType);

					foreach (var key in InputData.Keys)
					{
                        var analyzedResultSets = analyzer.Analyze(InputData[key], analyzerCommand.Parameters);
                        foreach (var analysisResult in analyzedResultSets)
						{
							if (writeOutputToFile)
								CsvFileWriter.WriteToFile(Constants.OUTPUT_DIR_FILTERS,
                                                          analyzerName, key,
														  SensorReading.HeaderCsv,
														  analysisResult.Data);
						}
					}
				}
				return true;
			} else {
				LogManager.Error("No input data to run analyzers on.", this);
				return false;
			}
        }

        /// <summary>
        /// Runs the summarizers.
        /// </summary>
        /// <returns><c>true</c>, if summarizers was run, <c>false</c> otherwise.</returns>
        /// <param name="writeOutputToFile">If set to <c>true</c> write output to file.</param>
		public bool RunSummarizers(bool writeOutputToFile = false)
		{
			// TODO
			return false;
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