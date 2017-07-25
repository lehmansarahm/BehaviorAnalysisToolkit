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

			config.LoadInputs();
			Assert.AreEqual(1, config.InputData.Keys.Count);

			string key = config.InputData.Keys.First();
            Assert.AreEqual(2729, config.InputData[key].Count());
		}

        /// <summary>
        /// Tests the incomplete config load.
        /// </summary>
		[Test]
		public void TestIncompleteConfigLoad()
		{
			// file content improperly formatted...
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("invalidConfigFormat.json"));
			VerifyConfigPhaseCounts(config, 0, 0, 0, 0, 0);

			// content stops halfway through...
			config = Configuration.LoadFromFile(GetConfigFilePath("invalidConfigContent.json"));
			VerifyConfigPhaseCounts(config, 0, 0, 0, 0, 0);
            
            // basically, anything that results in an inability to serialize into a Configuration object
		}

        /// <summary>
        /// Tests the invalid config load.
        /// </summary>
		[Test]
		public void TestInvalidConfigLoad()
		{
			// wrong file type
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("invalidConfigFileType.txt"));
			VerifyConfigPhaseCounts(config, 0, 0, 0, 0, 0);

			// wrong file content (XML), etc.
			config = Configuration.LoadFromFile(GetConfigFilePath("invalidConfigContentFormat.xml"));
			VerifyConfigPhaseCounts(config, 0, 0, 0, 0, 0);
		}
    }
}