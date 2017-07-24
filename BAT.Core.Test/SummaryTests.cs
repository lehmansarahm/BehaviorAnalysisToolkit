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
            Assert.AreEqual(null, config.Analyzers.FirstOrDefault().Parameters);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);
            Assert.AreEqual(null, config.AnalysisData);

			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 35);
			Assert.AreEqual(null, config.AnalysisData);

			result = config.RunAnalyzers(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 35);
            VerifyAnalysisDataSetCount(config, 35);

            result = config.RunSummarizers(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 35);
			VerifyAnalysisDataSetCount(config, 35);
		}
	}
}