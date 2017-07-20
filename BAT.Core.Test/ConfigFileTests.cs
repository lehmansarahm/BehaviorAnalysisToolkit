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
            Configuration config = Configuration.LoadFromFile(GetConfigFilePath("simpleConfig.json"));
			Assert.AreEqual(config.Inputs.Count, 1);
			Assert.AreEqual(config.Transformers.Count, 1);
			Assert.AreEqual(config.Filters.Count, 1);
		}
    }
}