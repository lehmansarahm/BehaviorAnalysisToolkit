using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class ConfigFileTests
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
			Configuration config = Configuration.LoadFromFile("./Helpers/Configs/simpleConfig.json");
			Assert.AreEqual(config.inputs.Count, 1);
			Assert.AreEqual(config.transformers.Count, 1);
			Assert.AreEqual(config.filters.Count, 1);
		}
    }
}