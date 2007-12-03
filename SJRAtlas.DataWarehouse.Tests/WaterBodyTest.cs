using System;
using NUnit.Framework;
using SJRAtlas.Core;
using Rhino.Mocks;

namespace SJRAtlas.DataWarehouse.Tests
{
    [TestFixture]
    public class WaterBodyTest : AbstractModelTestCase
    {
        private MockRepository mocks; 

        [SetUp]
        public void Setup()
        {
            base.Init();
            mocks = new MockRepository();
        }

        [TearDown]
        public void Teardown()
        {
            base.Terminate();
            mocks.VerifyAll();
        }

        [Test]
        public void TestDefaultProperties()
        {
            string testString = "EXPECTED_RETURN_VALUE_OF_STRING_PROPERTIES";
            int testInt = int.MaxValue;
            float testFloat = float.MaxValue;
            DateTime testDate = DateTime.Now;
            IPlaceName testPlaceName = mocks.CreateMock<IPlaceName>();
            IWatershed testWatershed = mocks.CreateMock<IWatershed>();

            WaterBody waterbody = new WaterBody();

            waterbody.Abbreviation = testString;
            Assert.AreEqual(testString, waterbody.Abbreviation);

            waterbody.AltCgndbKey = testString;
            Assert.AreEqual(testString, waterbody.AltCgndbKey);

            waterbody.AltName = testString;
            Assert.AreEqual(testString, waterbody.AltName);

            waterbody.CgndbKey = testString;
            Assert.AreEqual(testString, waterbody.CgndbKey);

            waterbody.ComplexId = testInt;
            Assert.AreEqual(testInt, waterbody.ComplexId);

            waterbody.Created = testDate;
            Assert.AreEqual(testDate, waterbody.Created);

            waterbody.DrainageCode = testString;
            Assert.AreEqual(testString, waterbody.DrainageCode);

            waterbody.FlowsIntoWaterBodyId = testFloat;
            Assert.AreEqual(testFloat, waterbody.FlowsIntoWaterBodyId);

            waterbody.FlowsIntoWaterBodyName = testString;
            Assert.AreEqual(testString, waterbody.FlowsIntoWaterBodyName);

            waterbody.Id = testInt;
            Assert.AreEqual(testInt, waterbody.Id);

            waterbody.Modified = testDate;
            Assert.AreEqual(testDate, waterbody.Modified);

            waterbody.Name = testString;
            Assert.AreEqual(testString, waterbody.Name);

            waterbody.Placename = testPlaceName;
            Assert.AreEqual(testPlaceName, waterbody.Placename);

            waterbody.SurveryInd = testString;
            Assert.AreEqual(testString, waterbody.SurveryInd);

            waterbody.Type = testString;
            Assert.AreEqual(testString, waterbody.Type);

            waterbody.Watershed = testWatershed;
            Assert.AreEqual(testWatershed, waterbody.Watershed);

            mocks.ReplayAll();
        }

        [Test]
        public void TestPlaceNameProperties()
        {
            WaterBody waterbody = new WaterBody();

            Assert.AreEqual("NB", waterbody.Region);
            Assert.IsNull(waterbody.County);
            Assert.IsNull(waterbody.Latitude);
            Assert.IsNull(waterbody.Longitude);
            Assert.IsNaN(waterbody.LatDec);
            Assert.IsNaN(waterbody.LongDec);
            Assert.IsNull(waterbody.StatusTerm);
            Assert.IsNull(waterbody.ConciseTerm);

            string testString = "THIS_IS_A_TEST_STRING";
            double testDouble = double.MaxValue;
            IPlaceName placename = mocks.CreateMock<IPlaceName>();

            Expect.Call(placename.Region).Return(testString);
            Expect.Call(placename.County).Return(testString);
            Expect.Call(placename.Latitude).Return(testString);
            Expect.Call(placename.Longitude).Return(testString);
            Expect.Call(placename.LatDec).Return(testDouble);
            Expect.Call(placename.LongDec).Return(testDouble);
            Expect.Call(placename.StatusTerm).Return(testString);
            Expect.Call(placename.ConciseTerm).Return(testString);
            mocks.ReplayAll();

            waterbody.Placename = placename;
            Assert.AreEqual(testString, waterbody.Region);
            Assert.AreEqual(testString, waterbody.County);
            Assert.AreEqual(testString, waterbody.Latitude);
            Assert.AreEqual(testString, waterbody.Longitude);
            Assert.AreEqual(testDouble, waterbody.LatDec);
            Assert.AreEqual(testDouble, waterbody.LongDec);
            Assert.AreEqual(testString, waterbody.StatusTerm);
            Assert.AreEqual(testString, waterbody.ConciseTerm);
        }

        [Test]
        public void TestWatershedProperties()
        {
            WaterBody waterbody = new WaterBody();
            waterbody.FlowsIntoWaterBodyName = "00-00-00-00-00-00";
            
            Assert.IsNull(waterbody.TributaryOf);
            Assert.AreEqual(waterbody.FlowsIntoWaterBodyName, waterbody.DrainsInto);

            string testString = "THIS_IS_A_TEST_STRING";
            IWatershed watershed = mocks.CreateMock<IWatershed>();

            Expect.Call(watershed.DrainsInto).Return(testString);
            Expect.Call(watershed.TributaryOf).Return(testString);
            mocks.ReplayAll();

            waterbody.Watershed = watershed;
            Assert.AreEqual(testString, waterbody.DrainsInto);
            Assert.AreEqual(testString, waterbody.TributaryOf);
        }
	
    }
}
