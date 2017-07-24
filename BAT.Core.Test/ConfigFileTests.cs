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
			
            Assert.AreEqual(1, config.Inputs.Count);
			Assert.AreEqual(1, config.Transformers.Count);

			Assert.AreEqual(1, config.Filters.Count);
            Assert.AreEqual(0, config.Filters.FirstOrDefault().Parameters.Count);

            Assert.AreEqual(0, config.Analyzers.Count);
            Assert.AreEqual(0, config.Summarizers.Count);

			config.LoadInputs();
			Assert.AreEqual(1, config.InputData.Keys.Count);

			string key = config.InputData.Keys.First();
            Assert.AreEqual(2729, config.InputData[key].Count());
		}

		[Test]
		public void TestIncompleteConfigLoad()
		{
            // file content improperly formatted...
            // content stops halfway through...
            // basically, anything that results in an inability to serialize into a Configuration object
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestInvalidConfigLoad()
		{
            // wrong file type, wrong file content (XML), etc.
			Assert.AreEqual(true, false);
		}
    }
}