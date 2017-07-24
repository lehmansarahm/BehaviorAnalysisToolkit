using System.Collections.Generic;
using BAT.Core.Common;
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

		[Test]
		public void TestBasicDataLoad()
		{
            List<SensorReading> inputRecords = SensorReading.ReadSensorFile(GetInputFilePath("OA5-Breakfast.csv"));
            Assert.AreEqual(inputRecords.Count, 2729);
		}

		[Test]
		public void TestInvalidDataLoad()
		{
            // can't find file for whatever reason (bad path, typo in filename, etc.)
			Assert.AreEqual(true, false);
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

		[Test]
		public void TestDataLoadWithEmptyFile()
		{
            // correct file type but no contents
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestDataLoadWithIncompleteFile()
		{
            // correct file type but data cuts out somewhere in the middle
            // think:  watch dies in middle of testing trial
			Assert.AreEqual(true, false);
		}

		[Test]
		public void TestDataLoadWithBadFileData()
		{
            // individual fields aren't what we expect
            // wrong file type
			Assert.AreEqual(true, false);
		}
    }
}