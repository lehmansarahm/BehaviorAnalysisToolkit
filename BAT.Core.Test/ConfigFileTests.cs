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
			
            Assert.AreEqual(config.Inputs.Count, 1);
			Assert.AreEqual(config.Transformers.Count, 1);
			Assert.AreEqual(config.Filters.Count, 1);

			config.LoadInputs();
			Assert.AreEqual(config.InputData.Keys.Count, 1);

			string key = config.InputData.Keys.First();
            Assert.AreEqual(config.InputData[key].Count, 2729);
		}
    }
}