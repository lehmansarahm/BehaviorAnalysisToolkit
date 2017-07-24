using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
	[TestFixture]
	public class TransformerTestsBATTest : BATTest
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
		public void TestBasicTransform()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("basicTransform.json"));

			Assert.AreEqual(1, config.Inputs.Count);
			Assert.AreEqual(2, config.Transformers.Count);
			Assert.AreEqual(0, config.Filters.Count);
			Assert.AreEqual(0, config.Analyzers.Count);
			Assert.AreEqual(0, config.Summarizers.Count);

			config.LoadInputs();
			Assert.AreEqual(1, config.InputData.Keys.Count);

			config.RunTransformers(false);
			Assert.AreEqual(1, config.InputData.Keys.Count);
		}

		[Test]
		public void TestInvalidTransform()
		{
            // user provided a transformer name that doesn't match anything we have
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestLinearAccelerationTransform()
		{
			// run the gamut of this particular operation
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestLabelCleanupTransform()
		{
			// run the gamut of this particular operation
			Assert.AreEqual(true, false);
		}
    }
}