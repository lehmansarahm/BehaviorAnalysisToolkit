using System.Collections.Generic;
using BAT.Core.Common;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class InputDataTests
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
            List<SensorReading> inputRecords = SensorReading.ReadSensorFile("./Helpers/InputFiles/OA5-Breakfast.csv");
            Assert.AreEqual(inputRecords.Count, 2729);
		}
    }
}