using System.Linq;
using BAT.Core.Config;
using BAT.Core.Test.SupportFiles;
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
            Assert.IsNull(config.AnalysisData);

            VerifyInputDataSetCount(config, DefaultInput.RawInputCount);
            VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.RawInputRecordCount);
        }

        /// <summary>
        /// Tests the config load with mult inputs.
        /// </summary>
        [Test]
        public void TestConfigLoadWithMultInputs()
        {
            var config =Configuration.LoadFromFile(GetConfigFilePath("multInputs.json"));
            VerifyConfigPhaseCounts(config, 2, 2, 1, 0, 0);

            Assert.IsNull(config.InputData);
			Assert.IsNull(config.AnalysisData);

			VerifyParameterCount(config.Filters, 0, 1);
			VerifyClauseCount(config.Filters.FirstOrDefault().Parameters, 0, 2);

            // -----------------------------------------------------------------

            var success = config.LoadInputs();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);

            VerifyInputDataSetCount(config, MULT_INPUT_DATA_SET_COUNT);
			VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.RawInputRecordCount);
            VerifyInputDataSetValueCount(config, SECOND_INPUT_INDEX, SECOND_INPUT_RAW_RECORD_COUNT);

            // -----------------------------------------------------------------

            success = config.RunTransformers();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);

			VerifyInputDataSetCount(config, MULT_INPUT_DATA_SET_COUNT);
            VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.ProcessedInputRecordCount);
            VerifyInputDataSetValueCount(config, SECOND_INPUT_INDEX, SECOND_INPUT_PROCESSED_RECORD_COUNT);

            // -----------------------------------------------------------------

            success = config.RunFilters();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);
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
    }
}