using System;
using NUnit.Framework;
using Rhino.Mocks;
using SJRAtlas.Core;

namespace SJRAtlas.DataWarehouse.Tests
{
    [TestFixture]
    public class DWAtlasUtilsTest : AbstractModelTestCase
    {
        private MockRepository mocks;
        private DWAtlasUtils atlasUtils;

        [SetUp]
        public void Setup()
        {
            base.Init();
            mocks = new MockRepository();
            atlasUtils = new DWAtlasUtils();
        }

        [TearDown]
        public void Teardown()
        {
            base.Terminate();
            mocks.VerifyAll();
        }

        [Test]
        public void TestPlaceNameIsWithinBasin()
        {
            string placeNameId = "ABCDE";
            CgnsWatershedCrossReference xref = new CgnsWatershedCrossReference();
            xref.CgndbKey = placeNameId;
            xref.DrainageCode = "01-00-00-00-00-00";
            xref.CreateAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();

            Assert.IsTrue(atlasUtils.IsWithinBasin(placename));
        }

        [Test]
        public void TestPlaceNameIsNotWithinBasin()
        {
            string placeNameId = "ABCDE";
            CgnsWatershedCrossReference xref = new CgnsWatershedCrossReference();
            xref.CgndbKey = placeNameId;
            xref.DrainageCode = "02-00-00-00-00-00";
            xref.CreateAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();

            Assert.IsFalse(atlasUtils.IsWithinBasin(placename));
        }

        [Test]
        public void TestPlaceNameWithinBasinWhenNoXRefHits()
        {
            string placeNameId = "ABCDE";

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();

            Assert.IsFalse(atlasUtils.IsWithinBasin(placename));
        }

        [Test]
        public void TestPlaceNameWithinBasinWhenMultipleXRefHits()
        {
            string placeNameId = "ABCDE";
            CgnsWatershedCrossReference xref1 = new CgnsWatershedCrossReference();
            xref1.CgndbKey = placeNameId;
            xref1.DrainageCode = "01-00-00-00-00-00";
            xref1.CreateAndFlush();

            CgnsWatershedCrossReference xref2 = new CgnsWatershedCrossReference();
            xref2.CgndbKey = placeNameId;
            xref2.DrainageCode = "02-00-00-00-00-00";
            xref2.CreateAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();

            Assert.IsFalse(atlasUtils.IsWithinBasin(placename));
        }

        [Test]
        public void TestWatershedIsInBasin()
        {
            string drainageCode = "01-00-00-00-00-00";
            IWatershed watershed = mocks.CreateMock<IWatershed>();
            Expect.Call(watershed.DrainageCode).Return(drainageCode);
            mocks.ReplayAll();
            Assert.IsTrue(atlasUtils.IsWithinBasin(watershed));
        }

        [Test]
        public void TestWatershedIsNotWithinBasin()
        {
            string drainageCode = "00-00-00-00-00-00";
            IWatershed watershed = mocks.CreateMock<IWatershed>();
            Expect.Call(watershed.DrainageCode).Return(drainageCode);
            mocks.ReplayAll();
            Assert.IsFalse(atlasUtils.IsWithinBasin(watershed));
        }

        [Test]
        public void TestPlaceNameIsWatershed()
        {
            string placeNameId = "ABCDE";
            CgnsWatershedCrossReference xref = new CgnsWatershedCrossReference();
            xref.CgndbKey = placeNameId;
            xref.DrainageCode = "01-00-00-00-00-00";
            xref.CreateAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();

            Assert.IsTrue(atlasUtils.IsWatershed(placename));
        }

        [Test]
        public void TestPlaceNameIsNotWatershedWhenNoXRefHits()
        {
            string placeNameId = "ABCDE";
            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();
            Assert.IsFalse(atlasUtils.IsWatershed(placename));
        }

        [Test]
        public void TestPlaceNameIsNotWatershedWhenMutltipleXRefHits()
        {
            string placeNameId = "ABCDE";
            CgnsWatershedCrossReference xref1 = new CgnsWatershedCrossReference();
            xref1.CgndbKey = placeNameId;
            xref1.DrainageCode = "01-00-00-00-00-00";
            xref1.CreateAndFlush();

            CgnsWatershedCrossReference xref2 = new CgnsWatershedCrossReference();
            xref2.CgndbKey = placeNameId;
            xref2.DrainageCode = "02-00-00-00-00-00";
            xref2.CreateAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();

            Assert.IsFalse(atlasUtils.IsWatershed(placename));
        }

        [Test]
        public void TestPlaceNameIsWaterBodyWhenConciseTermIsLakeAndStatusTermNotRescinded()
        {
            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.ConciseTerm).Return("Lake");
            Expect.Call(placename.StatusTerm).Return("NOT RESCINDED");
            mocks.ReplayAll();
            Assert.IsTrue(atlasUtils.IsWaterBody(placename));
        }

        [Test]
        public void TestPlaceNameIsWaterBodyWhenConciseTermIsRiverAndStatusTermNotRescinded()
        {
            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.ConciseTerm).Repeat.Twice().Return("River");
            Expect.Call(placename.StatusTerm).Return("NOT RESCINDED");
            mocks.ReplayAll();
            Assert.IsTrue(atlasUtils.IsWaterBody(placename));
        }

        [Test]
        public void TestPlaceNameIsWaterBodyWhenStatusTermRescindedAndOneWaterBodyHit()
        {
            string id = "ABCDE";

            WaterBody waterbody = new WaterBody();
            waterbody.AltCgndbKey = id;
            waterbody.CreateAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.ConciseTerm).Return("Lake");
            Expect.Call(placename.StatusTerm).Return("Rescinded");
            Expect.Call(placename.Id).Return(id);
            mocks.ReplayAll();

            Assert.IsTrue(atlasUtils.IsWaterBody(placename));
        }

        [Test]
        public void TestPlaceNameIsWaterBodyWhenStatusTermRescindedAndNoWaterBodyHits()
        {
            string id = "ABCDE";

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.ConciseTerm).Return("Lake");
            Expect.Call(placename.StatusTerm).Return("Rescinded");
            Expect.Call(placename.Id).Return(id);
            mocks.ReplayAll();

            Assert.IsFalse(atlasUtils.IsWaterBody(placename));
        }

        [Test]
        public void TestPlaceNameIsWaterBodyWhenStatusTermRescindedAndMultipleWaterBodyHits()
        {
            string id = "ABCDE";

            WaterBody waterbody1 = new WaterBody();
            waterbody1.AltCgndbKey = id;
            waterbody1.CreateAndFlush();

            WaterBody waterbody2 = new WaterBody();
            waterbody2.AltCgndbKey = id;
            waterbody2.CreateAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.ConciseTerm).Return("Lake");
            Expect.Call(placename.StatusTerm).Return("Rescinded");
            Expect.Call(placename.Id).Return(id);
            mocks.ReplayAll();

            Assert.IsFalse(atlasUtils.IsWaterBody(placename));
        }

        [Test]
        public void TestPlaceNameIsNotWaterBody()
        {
            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.ConciseTerm).Repeat.Twice().Return("NOT A WATERBODY");
            mocks.ReplayAll();
            Assert.IsFalse(atlasUtils.IsWaterBody(placename));
        }

        [Test]
        public void TestCreateWaterBodyFromPlaceName()
        {
            string placenameId = "ABCDE";
            WaterBody waterbody = new WaterBody();
            waterbody.CgndbKey = placenameId;
            waterbody.SaveAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placenameId);
            mocks.ReplayAll();

            IWaterBody createdWaterbody = atlasUtils.CreateWaterBodyFromPlaceName(placename);
            Assert.IsNotNull(createdWaterbody);
        }

        [Test]
        public void TestCreateWaterbodyFromPlaceNameReturnsNull()
        {
            string placenameId = "ABCDE";
            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placenameId);
            mocks.ReplayAll();
            Assert.IsNull(atlasUtils.CreateWaterBodyFromPlaceName(placename));
        }

        [Test]
        public void TestCreateWaterbodyFromPlaceNameWhenMoreThanOneMatch()
        {
            string placenameId = "ABCDE";
            WaterBody waterbody1 = new WaterBody();
            waterbody1.CgndbKey = placenameId;
            waterbody1.SaveAndFlush();

            WaterBody waterbody2 = new WaterBody();
            waterbody2.CgndbKey = placenameId;
            waterbody2.SaveAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placenameId);
            mocks.ReplayAll();

            Assert.IsNull(atlasUtils.CreateWaterBodyFromPlaceName(placename));
        }	

        [Test]
        public void TestCreateWaterBodyFromNullPlaceName()
        {
            Assert.IsNull(atlasUtils.CreateWaterBodyFromPlaceName(null));
        }	

        [Test]
        public void TestFindDrainageCodeReturnsNull()
        {
            string placeNameId = "ABCDE";
            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();
            Assert.IsNull(atlasUtils.FindDrainageCode(placename));
        }

        [Test]
        public void FindDrainageCode()
        {
            string placeNameId = "ABCDE";
            string drainageCode = "01-00-00-00-00-00";
            CgnsWatershedCrossReference xref = new CgnsWatershedCrossReference();
            xref.CgndbKey = placeNameId;
            xref.DrainageCode = drainageCode;
            xref.CreateAndFlush();

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(placename.Id).Return(placeNameId);
            mocks.ReplayAll();

            Assert.AreEqual(drainageCode, atlasUtils.FindDrainageCode(placename));
        }
	
	
	
    }
}
