using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;
using BAT.Core.Test.SupportFiles;
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

			// -----------------------------------------------------------------

			var commandParams = config.Analyzers.Where(x => x.Name.Equals("PauseCount"))
									  .FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("InstantSpeed", labelCommand.Field);
			Assert.AreEqual(2, labelCommand.Clauses.Count);

			var thresholdClause = labelCommand.Clauses.FirstOrDefault();
			Assert.AreEqual(CommandParameters.Threshold, thresholdClause.Key);
			Assert.AreEqual("0.01", thresholdClause.Value);

            // -----------------------------------------------------------------

			var success = config.LoadInputs();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);

            VerifyInputDataSetCount(config, DefaultInput.RawInputCount);
			VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.RawInputRecordCount);

			// -----------------------------------------------------------------

			success = config.RunTransformers();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);

			VerifyInputDataSetCount(config, DefaultInput.RawInputCount);
            VerifyInputDataSetValueCount(config, DefaultInput.Index, DefaultInput.ProcessedInputRecordCount);

			// -----------------------------------------------------------------

			success = config.RunFilters();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);

			VerifyInputDataSetCount(config, DefaultInput.SelectTaskCount);
            // count of an individual data set?

			// -----------------------------------------------------------------

			success = config.RunAnalyzers();
			Assert.IsTrue(success);

			VerifyInputDataSetCount(config, DefaultInput.SelectTaskCount);
			// count of an individual data set?

			// analysis results aggregated by original input file ...
			// will only be one result set ...
			VerifyAnalysisDataSetCount(config, DefaultInput.RawInputCount);
			VerifyAnalysisDataSetValueCount(config, DefaultInput.Index, DefaultInput.IndivPauseCount);
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

            // -----------------------------------------------------------------

			var commandParams = config.Analyzers.Where(x => x.Name.Equals("PauseDuration"))
									  .FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("InstantSpeed", labelCommand.Field);
			Assert.AreEqual(2, labelCommand.Clauses.Count);

			var thresholdClause = labelCommand.Clauses.FirstOrDefault();
			Assert.AreEqual(CommandParameters.Threshold, thresholdClause.Key);
			Assert.AreEqual("0.01", thresholdClause.Value);

			// -----------------------------------------------------------------

			var success = config.LoadInputs();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);
			VerifyInputDataSetCount(config, 1);

			// -----------------------------------------------------------------

			success = config.RunTransformers();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);
			VerifyInputDataSetCount(config, 1);

			// -----------------------------------------------------------------

			success = config.RunFilters();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);
			VerifyInputDataSetCount(config, DefaultInput.SelectTaskCount);

			// -----------------------------------------------------------------

			success = config.RunAnalyzers();
			Assert.IsTrue(success);
			VerifyInputDataSetCount(config, DefaultInput.SelectTaskCount);

			// analysis results aggregated by original input file ...
			// will only be one result set ...
			VerifyAnalysisDataSetCount(config, DefaultInput.RawInputCount);
			VerifyAnalysisDataSetValueCount(config, DefaultInput.Index, DefaultInput.DistinctPauseCount);
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

			// -----------------------------------------------------------------

			var commandParams = config.Analyzers.Where(x => x.Name.Equals("TaskTime"))
									  .FirstOrDefault().Parameters;
			Assert.IsNull(commandParams);

			// -----------------------------------------------------------------

			var success = config.LoadInputs();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);
			VerifyInputDataSetCount(config, 1);

			// -----------------------------------------------------------------

			success = config.RunTransformers();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);
			VerifyInputDataSetCount(config, 1);

			// -----------------------------------------------------------------

			success = config.RunFilters();
			Assert.IsTrue(success);
			Assert.IsNull(config.AnalysisData);
			VerifyInputDataSetCount(config, DefaultInput.SelectTaskCount);

			// -----------------------------------------------------------------

			success = config.RunAnalyzers();
			Assert.IsTrue(success);
			VerifyInputDataSetCount(config, DefaultInput.SelectTaskCount);

			// analysis results aggregated by original input file ...
			// will only be one result set ...
			VerifyAnalysisDataSetCount(config, DefaultInput.RawInputCount);
            VerifyAnalysisDataSetValueCount(config, DefaultInput.Index, DefaultInput.SelectTaskCount);
		}
	}
}