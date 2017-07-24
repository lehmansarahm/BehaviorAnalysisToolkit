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
				Configuration.LoadFromFile(GetConfigFilePath("simpleConfig.json"));
			
            Assert.AreEqual(1, config.Inputs.Count);
			Assert.AreEqual(2, config.Transformers.Count);
			Assert.AreEqual(1, config.Filters.Count);

            var commandParams = config.Filters.FirstOrDefault().Parameters;
            Assert.AreEqual(2, commandParams.Count);

			config.LoadInputs();
			Assert.AreEqual(1, config.InputData.Keys.Count);

			string key = config.InputData.Keys.First();
            Assert.AreEqual(2729, config.InputData[key].Count());
		}

        /// <summary>
        /// Tests the basic transform and filter.
        /// </summary>
        [Test]
        public void TestBasicTransformAndFilter()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("simpleConfig.json"));
			config.LoadInputs();
			Assert.AreEqual(1, config.InputData.Keys.Count);

			config.RunTransformers(false);
			Assert.AreEqual(1, config.InputData.Keys.Count);

			config.RunFilters(false);
			Assert.AreEqual(33, config.InputData.Keys.Count);
        }
    }
}