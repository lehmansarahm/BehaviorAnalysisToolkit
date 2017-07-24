using System.Linq;
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
			Assert.AreEqual(2, labelCommand.Clauses.Count);

			var containsClause = labelCommand.Clauses.FirstOrDefault();
			Assert.AreEqual("Contains", containsClause.Key);
			Assert.AreEqual("Select", containsClause.Value);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyPhaseResultDataSetCount(config, 1);

			// TODO - FIX THIS TEST
			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyPhaseResultDataSetCount(config, 33);
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
			VerifyPhaseResultDataSetCount(config, 1);

			// user provided filter name that doesn't match anything we have
			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(false, result);
			VerifyPhaseResultDataSetCount(config, 0);
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
			VerifyPhaseResultDataSetCount(config, 1);

            // assuming everything else is well formed in config, operation should 
            // still complete successfully ... phase output will just be empty
			result = config.RunFilters(WRITE_TO_FILE);
			Assert.AreEqual(true, result);
			VerifyPhaseResultDataSetCount(config, 0);
		}

		[Test]
		public void TestOperationActivityFilter()
		{
            // run the gamut of this particular operation
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestOperationCompletionFilter()
		{
			// run the gamut of this particular operation
			Assert.AreEqual(true, false);
		}
    }
}