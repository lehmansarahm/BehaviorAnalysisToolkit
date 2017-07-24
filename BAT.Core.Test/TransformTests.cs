using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
	[TestFixture]
	public class TransformTests : BATTest
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
		/// Tests the basic transform operation.
		/// </summary>
		[Test]
		public void TestBasicTransform()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("basicTransform.json"));
			VerifyConfigPhaseCounts(config, 1, 2, 0, 0, 0);

			config.LoadInputs();
			VerifyInputDataSetCount(config, 1);

			config.RunTransformers(WRITE_TO_FILE);
			VerifyInputDataSetCount(config, 1);
		}

        /// <summary>
        /// Tests the invalid transform.
        /// </summary>
		[Test]
		public void TestInvalidTransform()
		{
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("invalidTransform.json"));
			VerifyConfigPhaseCounts(config, 1, 1, 0, 0, 0);

			config.LoadInputs();
            VerifyInputDataSetCount(config, 1);

			// user provided a transformer name that doesn't match anything we have
			var result = config.RunTransformers(WRITE_TO_FILE);
			Assert.AreEqual(false, result);
			VerifyInputDataSetCount(config, 0);
		}

		[Test]
		public void TestOperationLinearAccelerationTransform()
		{
			// run the gamut of this particular operation
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestOperationLabelCleanupTransform()
		{
			// run the gamut of this particular operation
			Assert.AreEqual(true, false);
		}
    }
}