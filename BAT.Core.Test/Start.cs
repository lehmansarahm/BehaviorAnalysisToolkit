using System;
using NUnit.Framework;

namespace BAT.Core.Test
{
    [TestFixture]
    public class Start
	{
		private int _someValue;

		[SetUp]
		public void Setup()
		{
			_someValue = 5;
		}

		[TearDown]
		public void TearDown()
		{
			_someValue = 0;
		}

		[Test]
		public void TestOne()
		{
            Assert.AreEqual(_someValue, 5);
		}
    }
}