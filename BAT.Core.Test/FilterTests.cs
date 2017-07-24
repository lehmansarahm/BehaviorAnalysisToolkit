using System.Linq;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
	[TestFixture]
	public class FilterTests : BATTest
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
		/// Tests the basic transform and filter.
		/// </summary>
		[Test]
		public void TestBasicFilter()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("basicFilter.json"));

			Assert.AreEqual(1, config.Inputs.Count);
			Assert.AreEqual(0, config.Transformers.Count);
			Assert.AreEqual(1, config.Filters.Count);
			Assert.AreEqual(0, config.Analyzers.Count);
			Assert.AreEqual(0, config.Summarizers.Count);

			var commandParams = config.Filters.FirstOrDefault().Parameters;
			Assert.AreEqual(1, commandParams.Count);

			var labelCommand = commandParams.FirstOrDefault();
			Assert.AreEqual("Label", labelCommand.Field);
			Assert.AreEqual(2, labelCommand.Clauses.Count);

			var containsClause = labelCommand.Clauses.FirstOrDefault();
			Assert.AreEqual("Contains", containsClause.Key);
			Assert.AreEqual("Select", containsClause.Value);

			config.LoadInputs();
			Assert.AreEqual(1, config.InputData.Keys.Count);

			config.RunFilters(false);
			Assert.AreEqual(33, config.InputData.Keys.Count);
		}

		[Test]
		public void TestInvalidFilterName()
		{
            // user provided filter name that doesn't match anything we have
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestInvalidFilterCommands()
		{
            // filter name is right, but commands are missing or malformed
            // alt: commands are properly formatted but don't match anything we have
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestActivityFilter()
		{
            // run the gamut of this particular operation
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestCompletionFilter()
		{
			// run the gamut of this particular operation
			Assert.AreEqual(true, false);
		}
    }
}