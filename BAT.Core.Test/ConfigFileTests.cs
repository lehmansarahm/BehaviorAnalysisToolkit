using System.Linq;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class ConfigFileTests : BATTest
    {
        const bool WRITE_TO_FILE = false;

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
            Assert.AreEqual(0, config.Filters.FirstOrDefault().Parameters.Count);

            var success = config.LoadInputs();
            Assert.IsTrue(success);
            VerifyInputDataSetCount(config, 1);

            string key = config.InputData.Keys.First();
            Assert.AreEqual(3060, config.InputData[key].Count());
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
            Assert.AreEqual(1, config.Filters.FirstOrDefault().Parameters.Count);
            Assert.AreEqual(2, config.Filters.FirstOrDefault().Parameters.FirstOrDefault().Clauses.Count);

            // -----------------------------------------------------------------

            var success = config.LoadInputs();
            Assert.IsTrue(success);

            VerifyInputDataSetCount(config, 2);
            Assert.IsNull(config.AnalysisData);

            string firstFile = config.InputData.Keys.FirstOrDefault();
            Assert.AreEqual(3060, config.InputData[firstFile].Count());

            string secondFile = config.InputData.Keys.LastOrDefault();
            Assert.AreEqual(4490, config.InputData[secondFile].Count());

            // -----------------------------------------------------------------

            success = config.RunTransformers();
            //Assert.IsTrue(success);

            VerifyInputDataSetCount(config, 2);
            Assert.IsNull(config.AnalysisData);

            firstFile = config.InputData.Keys.FirstOrDefault();
            Assert.AreEqual(3059, config.InputData[firstFile].Count());

            secondFile = config.InputData.Keys.LastOrDefault();
            Assert.AreEqual(4489, config.InputData[secondFile].Count());

            // -----------------------------------------------------------------

            success = config.RunFilters();
            Assert.IsTrue(success);

            VerifyInputDataSetCount(config, 18);
            Assert.IsNull(config.AnalysisData);
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