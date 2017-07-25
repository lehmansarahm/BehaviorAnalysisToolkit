using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
	[TestFixture]
	public class FilterTests : BATTest
	{
        const bool WRITE_TO_FILE = false;

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
		/// Tests the basic filter operation.
		/// </summary>
		[Test]
		public void TestBasicFilter()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("basicFilter.json"));
            VerifyConfigPhaseCounts(config, 1, 0, 1, 0, 0);

            var commandParams = config.Filters.FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("Label", labelCommand.Field);
			Assert.AreEqual(1, labelCommand.Clauses.Count);

			var containsClause = labelCommand.Clauses.FirstOrDefault();
			Assert.AreEqual("Split", containsClause.Key);
			Assert.AreEqual("true", containsClause.Value);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 35);
		}

        /// <summary>
        /// Tests the name of the invalid filter.
        /// </summary>
		[Test]
		public void TestInvalidFilterName()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("invalidFilterName.json"));
			VerifyConfigPhaseCounts(config, 1, 0, 1, 0, 0);

			var commandParams = config.Filters.FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("Label", labelCommand.Field);
			Assert.AreEqual(2, labelCommand.Clauses.Count);

			var containsClause = labelCommand.Clauses.FirstOrDefault();
			Assert.AreEqual("Contains", containsClause.Key);
			Assert.AreEqual("Select", containsClause.Value);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

			// user provided filter name that doesn't match anything we have
			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(false, result);
			VerifyInputDataSetCount(config, 0);
		}

        /// <summary>
        /// Tests the invalid filter commands.
        /// </summary>
		[Test]
		public void TestInvalidFilterCommands()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("invalidFilterCommands.json"));
			VerifyConfigPhaseCounts(config, 1, 0, 1, 0, 0);

			var commandParams = config.Filters.FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			// commands are properly formatted but don't match anything we have
			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("Invalid", labelCommand.Field);
			Assert.AreEqual(3, labelCommand.Clauses.Count);

            // filter name is right, but commands are missing or malformed
			var containsClause = labelCommand.Clauses.FirstOrDefault();
			Assert.AreEqual(null, containsClause.Key);
			Assert.AreEqual("Select", containsClause.Value);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

            // assuming everything else is well formed in config, operation should 
            // still complete successfully ... phase output will just be empty
			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 0);
		}

        /// <summary>
        /// Tests the operation activity filter.
        /// </summary>
		[Test]
		public void TestOperationActivityFilter()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("activityFilter.json"));
			VerifyConfigPhaseCounts(config, 1, 0, 1, 0, 0);

			var commandParams = config.Filters.FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("Label", labelCommand.Field);
			Assert.AreEqual(2, labelCommand.Clauses.Count);

			var containsClause = labelCommand.Clauses.FirstOrDefault();
            Assert.AreEqual(Constants.COMMAND_PARAM_CONTAINS, containsClause.Key);
			Assert.AreEqual("Select", containsClause.Value);

			var splitClause = labelCommand.Clauses.LastOrDefault();
			Assert.AreEqual(Constants.COMMAND_PARAM_SPLIT, splitClause.Key);
			Assert.AreEqual("true", splitClause.Value);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 11);  // returning only tasks with "select" in label
		}

		/// <summary>
		/// Tests the filter with transform.
		/// </summary>
		[Test]
		public void TestOperationActivityFilterWithTransform()
		{
			// need to make sure that the data being generated from the 
			// "transform" phase is being properly utilized for the "filter" phase
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("activityFilterWithTransform.json"));
			VerifyConfigPhaseCounts(config, 1, 2, 1, 0, 0);

			var commandParams = config.Filters.FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("Label", labelCommand.Field);
			Assert.AreEqual(2, labelCommand.Clauses.Count);

			var containsClause = labelCommand.Clauses.FirstOrDefault();
			Assert.AreEqual(Constants.COMMAND_PARAM_CONTAINS, containsClause.Key);
			Assert.AreEqual("Select", containsClause.Value);

            var splitClause = labelCommand.Clauses.LastOrDefault();
            Assert.AreEqual(Constants.COMMAND_PARAM_SPLIT, splitClause.Key);
			Assert.AreEqual("true", splitClause.Value);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

            result = config.RunTransformers(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(true, result);

			// returning only tasks with "select" in label
			// THERE ARE ONLY TEN!!  (no "select quit")
			VerifyInputDataSetCount(config, 10);

            var selectBreadDataSet = config.InputData.Where(x => x.Key.Contains("select-bread")).FirstOrDefault().Value;
			Assert.AreNotEqual(null, selectBreadDataSet);

            var firstReading = selectBreadDataSet.FirstOrDefault();
			Assert.AreNotEqual(null, firstReading);

			// make sure that the first record of "select bread" is what we expect
			VerifySensorReading(FIRST_SELECT_BREAD_READING, firstReading);
		}

        /// <summary>
        /// Tests the operation completion filter.
        /// </summary>
		[Test]
		public void TestOperationCompletionFilter()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("completionFilter.json"));
			VerifyConfigPhaseCounts(config, 1, 0, 1, 0, 0);

            var commandParams = config.Filters.FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

            var thresholdParam = commandParams.FirstOrDefault().Clauses.FirstOrDefault();
			Assert.AreEqual(Constants.COMMAND_PARAM_THRESHOLD, thresholdParam.Key);
			Assert.AreEqual("94", thresholdParam.Value);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

            // TODO - actually implement "completion" filter
			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 11);
		}
    }
}