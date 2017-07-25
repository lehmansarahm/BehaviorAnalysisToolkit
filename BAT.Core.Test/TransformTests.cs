using System.Linq;
using BAT.Core.Common;
using BAT.Core.Config;
using NUnit.Framework;

namespace BAT.Core.Test
{
	[TestFixture]
	public class TransformTests : BATTest
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

			config.RunTransformers();
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
			var result = config.RunTransformers();
			Assert.AreEqual(false, result);
			VerifyInputDataSetCount(config, 0);
		}

        /// <summary>
        /// Tests the operation linear acceleration transform.
        /// </summary>
		[Test]
		public void TestOperationLinearAccelerationTransform()
		{
			// need to make sure that the data being generated from the
			// "transform" phase is being properly utilized for the "filter" phase
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("linearAccelerationTransform.json"));
			VerifyConfigPhaseCounts(config, 1, 1, 0, 0, 0);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

			result = config.RunTransformers();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

			var firstDataSet = config.InputData.FirstOrDefault().Value;
			Assert.AreNotEqual(null, firstDataSet);

			var firstSelectReading = firstDataSet.Where(x => x.Label.Contains("select")).FirstOrDefault();
			Assert.AreNotEqual(null, firstSelectReading);

            // make sure that the first record of "select bread" is what we expect
            VerifySensorReading(FIRST_SELECT_BREAD_READING, firstSelectReading);
		}

        /// <summary>
        /// Tests the operation label cleanup transform.
        /// </summary>
		[Test]
		public void TestOperationLabelCleanupTransform()
		{
			// need to make sure that the data being generated from the
			// "transform" phase is being properly utilized for the "filter" phase
			Configuration config =
				Configuration.LoadFromFile(GetConfigFilePath("labelCleanupTransform.json"));
			VerifyConfigPhaseCounts(config, 1, 1, 0, 0, 0);

			var result = config.LoadInputs();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

			result = config.RunTransformers();
			Assert.AreEqual(true, result);
			VerifyInputDataSetCount(config, 1);

			var firstDataSet = config.InputData.FirstOrDefault().Value;
			Assert.AreNotEqual(null, firstDataSet);

			var firstSelectReading = firstDataSet.Where(x => x.Label.Contains("select")).FirstOrDefault();
			Assert.AreNotEqual(null, firstSelectReading);

            // make sure that the first record of "select bread" is what we expect
            Assert.AreEqual("select-bread", firstSelectReading.Label);
		}
    }
}