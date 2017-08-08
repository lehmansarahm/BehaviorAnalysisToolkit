using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Filters;
using BAT.Core.Test.SupportFiles;
using BAT.Core.Transformers;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class ConfigFileTests : BATTest
    {
        const bool WRITE_TO_FILE = false;

        const int MULT_INPUT_DATA_SET_COUNT = 2;
        const int MULT_INPUT_FILTERED_DATA_SET_COUNT = 20;

		const int SECOND_INPUT_INDEX = 1;
		const int SECOND_INPUT_RAW_RECORD_COUNT = 6445;
		const int SECOND_INPUT_PROCESSED_RECORD_COUNT = 6445;

        /// <summary>
        /// Setup this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // do something
        }

        /// <summary>
        /// Tears down.
        /// </summary>
		[TearDown]
        public void TearDown()
        {
            // do something
        }

        /// <summary>
        /// Tests the basic config load.
        /// </summary>
		[Test]
        public void TestBasicConfigLoad()
        {
            var config = Configuration.LoadFromFile(GetConfigFilePath("basicConfigLoad.json"));
			VerifyConfigPhaseCounts(config, 1, 1, 1, 0, 0);
			VerifyParameterCount(config.Filters, 0, 0);

            var success = config.LoadInputs();
            Assert.IsTrue(success);
            Assert.IsEmpty(config.AnalysisData);

            VerifyInputDataSetCount(config, DefaultInput.RawInputCount);
            VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.RawInputRecordCount);
		}

        /// <summary>
        /// Tests the basic config load without user.
        /// </summary>
		[Test]
		public void TestBasicConfigLoadWithoutUser()
		{
			// should still work
			var config = Configuration.LoadFromFile(GetConfigFilePath("basicConfigWithoutUser.json"));
			VerifyConfigPhaseCounts(config, 1, 1, 1, 0, 0);
			VerifyParameterCount(config.Filters, 0, 0);

			var success = config.LoadInputs();
			Assert.IsTrue(success);
			Assert.IsEmpty(config.AnalysisData);

			VerifyInputDataSetCount(config, DefaultInput.RawInputCount);
			VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.RawInputRecordCount);
		}

		[Test]
		public void TestBasicConfigLoadFromDirectory()
		{
			var config = Configuration.LoadFromFile(GetConfigFilePath("basicConfigLoadFromDir.json"));
			VerifyConfigPhaseCounts(config, 1, 1, 1, 0, 0);
			VerifyParameterCount(config.Filters, 0, 0);

			var success = config.LoadInputs();
			Assert.IsTrue(success);
			Assert.IsEmpty(config.AnalysisData);

			Assert.AreEqual(1, config.Inputs.Count);
			string currentDir = AppDomain.CurrentDomain.BaseDirectory;
			string source = $"{currentDir}/{config.Inputs.FirstOrDefault().InputSource}";

            Assert.IsTrue(Directory.Exists(source));
            var files = Directory.GetFiles(source);
            var inputFileCount = files.Where(x => x.EndsWith(Constants.BAT.DEFAULT_INPUT_FILE_EXT)).Count();
            var defaultInputIndex = Array.FindIndex(files, x => x.Contains(DefaultInput.Filename));

            VerifyInputDataSetCount(config, inputFileCount);
			VerifyInputDataSetValueCount(config, defaultInputIndex, DefaultInput.RawInputRecordCount);
		}

		[Test]
		public void TestBasicConfigLoadFromDirectoryWithoutUser()
		{
			// should still work
			var config = Configuration.LoadFromFile(GetConfigFilePath("basicConfigLoadFromDirWithoutUser.json"));
			VerifyConfigPhaseCounts(config, 1, 1, 1, 0, 0);
			VerifyParameterCount(config.Filters, 0, 0);

			var success = config.LoadInputs();
			Assert.IsTrue(success);
			Assert.IsEmpty(config.AnalysisData);

			Assert.AreEqual(1, config.Inputs.Count);
			string currentDir = AppDomain.CurrentDomain.BaseDirectory;
			string source = $"{currentDir}/{config.Inputs.FirstOrDefault().InputSource}";

			Assert.IsTrue(Directory.Exists(source));
			var files = Directory.GetFiles(source);
			var inputFileCount = files.Where(x => x.EndsWith(Constants.BAT.DEFAULT_INPUT_FILE_EXT)).Count();
			var defaultInputIndex = Array.FindIndex(files, x => x.Contains(DefaultInput.Filename));

			VerifyInputDataSetCount(config, inputFileCount);
			VerifyInputDataSetValueCount(config, defaultInputIndex, DefaultInput.RawInputRecordCount);
		}

        /// <summary>
        /// Tests the config load with mult inputs.
        /// </summary>
        [Test]
        public void TestConfigLoadWithMultInputs()
        {
            var config =Configuration.LoadFromFile(GetConfigFilePath("multInputs.json"));
            VerifyConfigPhaseCounts(config, 2, 2, 1, 0, 0);

            Assert.IsEmpty(config.InputData);
			Assert.IsEmpty(config.AnalysisData);

			VerifyParameterCount(config.Filters, 0, 1);
			VerifyClauseCount(config.Filters.FirstOrDefault().Parameters, 0, 2);

            // -----------------------------------------------------------------

            var success = config.LoadInputs();
			Assert.IsTrue(success);
			Assert.IsEmpty(config.AnalysisData);

            VerifyInputDataSetCount(config, MULT_INPUT_DATA_SET_COUNT);
			VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.RawInputRecordCount);
            VerifyInputDataSetValueCount(config, SECOND_INPUT_INDEX, SECOND_INPUT_RAW_RECORD_COUNT);

            // -----------------------------------------------------------------

            success = config.RunTransformers();
			Assert.IsTrue(success);
			Assert.IsEmpty(config.AnalysisData);

			VerifyInputDataSetCount(config, MULT_INPUT_DATA_SET_COUNT);
            VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.ProcessedInputRecordCount);
            VerifyInputDataSetValueCount(config, SECOND_INPUT_INDEX, SECOND_INPUT_PROCESSED_RECORD_COUNT);

            // -----------------------------------------------------------------

            success = config.RunFilters();
			Assert.IsTrue(success);
			Assert.IsEmpty(config.AnalysisData);
            VerifyInputDataSetCount(config, MULT_INPUT_FILTERED_DATA_SET_COUNT);
        }

        /// <summary>
        /// Tests the incomplete config load.
        /// </summary>
		[Test]
        public void TestIncompleteConfigLoad()
        {
            // -----------------------------------------------------------------
            // file content improperly formatted...
            // -----------------------------------------------------------------
            VerifyBadConfigLoad("invalidConfigFormat.json");

            // -----------------------------------------------------------------
            // content stops halfway through...
            // -----------------------------------------------------------------
            VerifyBadConfigLoad("invalidConfigContent.json");

            // -----------------------------------------------------------------
            // file is empty...
            // -----------------------------------------------------------------
            VerifyBadConfigLoad("emptyConfig.json");

            // basically, anything that results in an inability to serialize into a Configuration object
        }

        /// <summary>
        /// Tests the invalid config load.
        /// </summary>
		[Test]
        public void TestInvalidConfigLoad()
        {
            // -----------------------------------------------------------------
            // wrong file type...
            // -----------------------------------------------------------------
            VerifyBadConfigLoad("invalidConfigFileType.txt");

            // -----------------------------------------------------------------
            // wrong file content (XML)...
            // -----------------------------------------------------------------
            VerifyBadConfigLoad("invalidConfigContentFormat.xml");

            // -----------------------------------------------------------------
            // file does not exist...
            // -----------------------------------------------------------------
            VerifyBadConfigLoad("asdfsdsdf.xml");
		}

        /// <summary>
        /// Cans the find transformers.
        /// </summary>
		[Test]
		public void CanFindTransformers()
		{
			var config = new Configuration();

			var transType = typeof(ITransformer);
			var types = Assembly.GetAssembly(transType).GetTypes()
								.Where(x => transType.IsAssignableFrom(x) && !x.IsInterface);

			config.Transformers = types.Select(x => x.Name).ToList();
			var transformers = TransformerManager.GetTransformers(config.Transformers);

			Assert.AreEqual(transformers.Count(), types.Count());
		}

		//todo: repeat with analyzers, summarizers, and filters
	}
}