using System.Linq;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class ConfigFileTests : BATTest
	{
		[SetUp]
		public void Setup()
		{
			// do something
		}

		[TearDown]
		public void TearDown()
		{
			// do something
		}

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

        [Test]
        public void TestBasicTransformAndFilter()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("simpleConfig.json"));
			config.LoadInputs();

            config.RunTransformers(false);
            config.RunFilters(false);
        }
    }
}