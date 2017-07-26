using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
    public class BATTest
    {
        const string TEST_DATA_FOLDER = "SupportFiles";
        const string TEST_DATA_FOLDER_CONFIGS = "ConfigFiles";
        const string TEST_DATA_FOLDER_INPUTS = "InputFiles";

        const int PRECISION = 5;

        /// <summary>
        /// Gets the config file path.
        /// </summary>
        /// <returns>The config file path.</returns>
        /// <param name="filename">Filename.</param>
        protected static string GetConfigFilePath(string filename)
        {
            return Path.Combine(GetTestDataFolder(TEST_DATA_FOLDER_CONFIGS), filename);
        }

        /// <summary>
        /// Gets the input file path.
        /// </summary>
        /// <returns>The input file path.</returns>
        /// <param name="filename">Filename.</param>
        protected static string GetInputFilePath(string filename)
        {
            return Path.Combine(GetTestDataFolder(TEST_DATA_FOLDER_INPUTS), filename);
        }

        /// <summary>
        /// Verifies the config phase counts.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="inputs">Inputs.</param>
        /// <param name="transforms">Transforms.</param>
        /// <param name="filters">Filters.</param>
        /// <param name="analyses">Analyses.</param>
        /// <param name="summaries">Summaries.</param>
        protected static void VerifyConfigPhaseCounts(Configuration config, int inputs,
                                                      int transforms, int filters,
                                                      int analyses, int summaries)
        {
            Assert.AreEqual(inputs, config.Inputs.Count);
            Assert.AreEqual(transforms, config.Transformers.Count);
            Assert.AreEqual(filters, config.Filters.Count);
            Assert.AreEqual(analyses, config.Analyzers.Count);
            Assert.AreEqual(summaries, config.Summarizers.Count);
        }

        /// <summary>
        /// Verifies the input data set count.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="count">Count.</param>
		protected static void VerifyInputDataSetCount(Configuration config, int count)
        {
            Assert.IsNotNull(config.InputData);
            Assert.AreEqual(count, config.InputData.Keys.Count);
        }

        /// <summary>
        /// Verifies the input data set value count.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="dataSetIndex">Data set index.</param>
        /// <param name="count">Count.</param>
        protected static void VerifyInputDataSetValueCount(Configuration config, int dataSetIndex, int count)
        {
            Assert.IsNotNull(config.InputData);
            var dataSet = config.InputData.ElementAt(dataSetIndex).Value;
            Assert.AreEqual(count, dataSet.Count());
        }

        /// <summary>
        /// Verifies the analysis data set count.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="count">Count.</param>
		protected static void VerifyAnalysisDataSetCount(Configuration config, int count)
        {
            Assert.IsNotNull(config.AnalysisData);
            Assert.AreEqual(count, config.AnalysisData.Keys.Count);
        }

        /// <summary>
        /// Verifies the parameter count.
        /// </summary>
        /// <param name="commands">Commands.</param>
        /// <param name="commandIndex">Command index.</param>
        /// <param name="count">Count.</param>
        protected static void VerifyParameterCount(IEnumerable<Command> commands, int commandIndex, int count)
        {
            var command = commands.ElementAt(commandIndex);
            Assert.IsNotNull(command);
            Assert.IsNotNull(command.Parameters);
            Assert.AreEqual(count, command.Parameters.Count);
        }

        /// <summary>
        /// Verifies the clause count.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        /// <param name="paramIndex">Parameter index.</param>
        /// <param name="count">Count.</param>
        protected static void VerifyClauseCount(IEnumerable<Parameter> parameters, int paramIndex, int count)
        {
            var parameter = parameters.ElementAt(paramIndex);
            Assert.IsNotNull(parameter);
            Assert.IsNotNull(parameter.Clauses);
            Assert.AreEqual(count, parameter.Clauses.Count);
        }

        /// <summary>
        /// Verifies the bad config load.
        /// </summary>
        /// <param name="filename">Filename.</param>
        protected static void VerifyBadConfigLoad(string filename)
        {
            Configuration config =
                Configuration.LoadFromFile(GetConfigFilePath(filename));
            VerifyConfigPhaseCounts(config, 0, 0, 0, 0, 0);

            var success = config.LoadInputs();
            Assert.IsFalse(success);
            Assert.IsNull(config.AnalysisData);
            VerifyInputDataSetCount(config, 0);

            success = config.RunTransformers();
            Assert.IsFalse(success);
            Assert.IsNull(config.AnalysisData);
            VerifyInputDataSetCount(config, 0);

            success = config.RunFilters();
            Assert.IsFalse(success);
            Assert.IsNull(config.AnalysisData);
            VerifyInputDataSetCount(config, 0);

            success = config.RunAnalyzers();
            Assert.IsFalse(success);
            Assert.IsNull(config.AnalysisData);
            VerifyInputDataSetCount(config, 0);
        }

        /// <summary>
        /// Verifies the bad input load.
        /// </summary>
        /// <param name="filename">Filename.</param>
        protected static void VerifyBadInputLoad(string filename)
        {
            var inputRecords = SensorReading.ReadSensorFile(GetInputFilePath(filename));
            Assert.AreEqual(0, inputRecords.Count);
        }

        /// <summary>
        /// Verifies the sensor reading.
        /// </summary>
        /// <param name="expected">Expected.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="includeSupportingFields">If set to <c>true</c> include supporting fields.</param>
        protected static void VerifySensorReading(SensorReading expected, SensorReading actual, bool includeSupportingFields = false)
        {
            Assert.AreEqual(expected.Time, actual.Time);
            Assert.AreEqual(expected.RecordNum, actual.RecordNum);

            Assert.AreEqual(Math.Round(expected.Azimuth, PRECISION),
                            Math.Round(actual.Azimuth, PRECISION));
            Assert.AreEqual(Math.Round(expected.Pitch, PRECISION),
                            Math.Round(actual.Pitch, PRECISION));
            Assert.AreEqual(Math.Round(expected.Roll, PRECISION),
                            Math.Round(actual.Roll, PRECISION));

            Assert.AreEqual(Math.Round(expected.AccelX, PRECISION),
                            Math.Round(actual.AccelX, PRECISION));
            Assert.AreEqual(Math.Round(expected.AccelY, PRECISION),
                            Math.Round(actual.AccelY, PRECISION));
            Assert.AreEqual(Math.Round(expected.AccelZ, PRECISION),
                            Math.Round(actual.AccelZ, PRECISION));

            if (includeSupportingFields)
            {
                Assert.AreEqual(Math.Round(expected.AccelMag, PRECISION),
                                Math.Round(actual.AccelMag, PRECISION));
                Assert.AreEqual(Math.Round(expected.InstantSpeed, PRECISION),
                                Math.Round(actual.InstantSpeed, PRECISION));

                Assert.AreEqual(expected.Start, actual.Start);
                Assert.AreEqual(expected.End, actual.End);
                Assert.AreEqual(expected.Label, actual.Label);
            }
        }

        /// <summary>
        /// Gets the test data folder.
        /// </summary>
        /// <returns>The test data folder.</returns>
        /// <param name="testDataFolder">Test data folder.</param>
		static string GetTestDataFolder(string testDataFolder)
        {
            var startupPath = AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
			string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), 
                                             pathItems.Take(pathItems.Length - pos - 1));
            return Path.Combine(projectPath, TEST_DATA_FOLDER, testDataFolder);
        }
    }
}