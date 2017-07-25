using System.Linq;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class ConfigFileTests : BATTest
	{
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
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("basicConfigLoad.json"));
            
			VerifyConfigPhaseCounts(config, 1, 1, 1, 0, 0);
            Assert.AreEqual(0, config.Filters.FirstOrDefault().Parameters.Count);

			var success = config.LoadInputs();
            Assert.IsTrue(success);
            VerifyInputDataSetCount(config, 1);

			string key = config.InputData.Keys.First();
            Assert.AreEqual(2729, config.InputData[key].Count());
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