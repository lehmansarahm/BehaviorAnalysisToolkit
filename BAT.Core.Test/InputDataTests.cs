using System.Collections.Generic;
using BAT.Core.Common;
using BAT.Core.Test.SupportFiles;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class InputDataTests : BATTest
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
        /// Tests the basic data load.
        /// </summary>
		[Test]
		public void TestBasicDataLoad()
		{
            List<SensorReading> inputRecords =
                SensorReading.ReadSensorFile(GetInputFilePath(DefaultInput.Filename));
            Assert.AreEqual(DefaultInput.RawInputRecordCount, inputRecords.Count);
		}

        /// <summary>
        /// Tests the invalid data load.
        /// </summary>
		[Test]
		public void TestInvalidDataLoad()
		{
			//------------------------------------------------------------------
			// can't find file for whatever reason (bad path, typo in filename, etc.)
			//------------------------------------------------------------------
			VerifyBadInputLoad("OA5-Invalid.csv");
		}

		[Test]
		public void TestDataLoadWithUser()
		{
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestDataLoadFromDirectory()
		{
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestDataLoadFromDirectoryWithUsers()
		{
			Assert.AreEqual(true, false);
		}

        /// <summary>
        /// Tests the data load with empty file.
        /// </summary>
		[Test]
		public void TestDataLoadWithEmptyFile()
		{
			//------------------------------------------------------------------
			// correct file type but no contents...
			//------------------------------------------------------------------
			VerifyBadInputLoad("OA5-Empty.csv");
		}

		[Test]
		public void TestDataLoadWithIncompleteFile()
		{
			//------------------------------------------------------------------
			// correct file type but data cuts out somewhere in the middle...
			// think:  watch dies in middle of testing trial...
            //
            // Should still import just fine ... will dump incomplete records
			//------------------------------------------------------------------
			var inputRecords = 
                SensorReading.ReadSensorFile(GetInputFilePath("OA5-MissingSecondHalf.csv"));
			Assert.AreEqual(2504, inputRecords.Count);
		}

        /// <summary>
        /// Tests the data load with bad file data.
        /// </summary>
		[Test]
		public void TestDataLoadWithBadFileData()
		{
            //------------------------------------------------------------------
            // individual fields aren't what we expect...
            //------------------------------------------------------------------
            VerifyBadInputLoad("OA5-BadData.csv");

			//------------------------------------------------------------------
			// wrong file type...
			//------------------------------------------------------------------
			VerifyBadInputLoad("OA5-Breakfast.xlsx");
		}
    }
}