using System.Linq;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
	[TestFixture]
	public class SummaryTests : BATTest
	{
		const bool WRITE_TO_FILE = false;

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

        /// <summary>
        /// Tests the basic summary.
        /// </summary>
		[Test]
		public void TestBasicSummary()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("basicSummary.json"));
			VerifyConfigPhaseCounts(config, 1, 0, 1, 1, 1);

            Assert.AreEqual(1, config.Filters.FirstOrDefault().Parameters.Count);
            Assert.IsNull(config.Analyzers.FirstOrDefault().Parameters);

			var result = config.LoadInputs();
			Assert.IsTrue(result);
			VerifyInputDataSetCount(config, 1);
            Assert.IsNull(config.AnalysisData);

			result = config.RunFilters(WRITE_TO_FILE);
			Assert.IsTrue(result);
			VerifyInputDataSetCount(config, 27);
			Assert.IsNull(config.AnalysisData);

			result = config.RunAnalyzers(WRITE_TO_FILE);
			Assert.IsTrue(result);
			VerifyInputDataSetCount(config, 27);
            VerifyAnalysisDataSetCount(config, 27);
		}
	}
}