using System.Linq;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
	[TestFixture]
	public class SummaryTests : BATTest
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

        /// <summary>
        /// Tests the basic summary.
        /// </summary>
		[Test]
		public void TestBasicSummary()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("basicSummary.json"));
			VerifyConfigPhaseCounts(config, 1, 0, 1, 1, 1);

            VerifyParameterCount(config.Filters, 0, 1);
            Assert.IsNull(config.Analyzers.FirstOrDefault().Parameters);

			var result = config.LoadInputs();
			Assert.IsTrue(result);
			Assert.IsNull(config.AnalysisData);
			VerifyInputDataSetCount(config, 1);

			result = config.RunFilters();
			Assert.IsTrue(result);
			Assert.IsNull(config.AnalysisData);
			VerifyInputDataSetCount(config, 27);

			result = config.RunAnalyzers();
			Assert.IsTrue(result);
			VerifyInputDataSetCount(config, 27);
            VerifyAnalysisDataSetCount(config, 27);
		}
	}
}