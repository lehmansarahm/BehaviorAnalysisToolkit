using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
	[TestFixture]
	public class AnalysisTests : BATTest
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
		public void TestOperationPauseCountAnalysis()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("pauseCountAnalysis.json"));
			VerifyConfigPhaseCounts(config, 1, 2, 1, 1, 0);

			var commandParams = config.Analyzers.FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("InstantSpeed", labelCommand.Field);
			Assert.AreEqual(2, labelCommand.Clauses.Count);

            var thresholdClause = labelCommand.Clauses.FirstOrDefault();
            Assert.AreEqual(Constants.COMMAND_PARAM_THRESHOLD, thresholdClause.Key);
			Assert.AreEqual("0.01", thresholdClause.Value);

			var success = config.LoadInputs();
			Assert.AreEqual(true, success);
			Assert.AreEqual(1, config.InputData.Keys.Count);
			Assert.AreEqual(null, config.AnalysisData);

			success = config.RunTransformers();
			Assert.AreEqual(true, success);
			Assert.AreEqual(1, config.InputData.Keys.Count);
			Assert.AreEqual(null, config.AnalysisData);

            success = config.RunFilters();
			Assert.AreEqual(true, success);
			Assert.AreEqual(null, config.AnalysisData);

			// 11 results (with activity split)
			Assert.AreEqual(11, config.InputData.Keys.Count);

            success = config.RunAnalyzers();
			Assert.AreEqual(true, success);

			// 11 results (with activity split) ... analysis data no longer null
			Assert.AreEqual(11, config.InputData.Keys.Count);
			Assert.AreEqual(11, config.AnalysisData.Keys.Count); 
		}
	}
}