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

			var commandParams = config.Analyzers.Where(x => x.Name.Equals("PauseCount"))
									  .FirstOrDefault().Parameters;
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

			// returning only tasks with "select" in label
			// THERE ARE ONLY TEN!!  (no "select quit")
			VerifyInputDataSetCount(config, 10);

			success = config.RunAnalyzers();
			Assert.AreEqual(true, success);

			// returning only tasks with "select" in label ... analysis data no longer null
			// THERE ARE ONLY TEN!!  (no "select quit")
			VerifyInputDataSetCount(config, 10);
            VerifyAnalysisDataSetCount(config, 10);
		}

		/// <summary>
		/// Tests the basic config load.
		/// </summary>
		[Test]
		public void TestOperationPauseDurationAnalysis()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("pauseDurationAnalysis.json"));
			VerifyConfigPhaseCounts(config, 1, 2, 1, 2, 0);

			var commandParams = config.Analyzers.Where(x => x.Name.Equals("PauseDuration"))
									  .FirstOrDefault().Parameters;
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

			// returning only tasks with "select" in label
			// THERE ARE ONLY TEN!!  (no "select quit")
			VerifyInputDataSetCount(config, 10);

			success = config.RunAnalyzers();
			Assert.AreEqual(true, success);

			// returning only tasks with "select" in label ... analysis data no longer null
			// THERE ARE ONLY TEN!!  (no "select quit")
			VerifyInputDataSetCount(config, 10);
			VerifyAnalysisDataSetCount(config, 10);
		}

		/// <summary>
		/// Tests the basic config load.
		/// </summary>
		[Test]
		public void TestOperationTaskTimeAnalysis()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("taskTimeAnalysis.json"));
			VerifyConfigPhaseCounts(config, 1, 2, 1, 1, 0);

			var commandParams = config.Analyzers.Where(x => x.Name.Equals("TaskTime"))
									  .FirstOrDefault().Parameters;
			Assert.AreEqual(null, commandParams);

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

			// returning only tasks with "select" in label
			// THERE ARE ONLY TEN!!  (no "select quit")
			VerifyInputDataSetCount(config, 10);

			success = config.RunAnalyzers();
			Assert.AreEqual(true, success);

			// returning only tasks with "select" in label ... analysis data no longer null
			// THERE ARE ONLY TEN!!  (no "select quit")
			VerifyInputDataSetCount(config, 10);
			VerifyAnalysisDataSetCount(config, 10);
		}
	}
}