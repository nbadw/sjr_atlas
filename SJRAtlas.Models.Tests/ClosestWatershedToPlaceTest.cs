using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class ClosestWatershedToPlaceTest : AbstractModelTestCase
    {
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            base.Setup();
            mocks = new MockRepository();
            // save one entry in the database that has a place and a watershed attached
            ClosestWatershedToPlace closestWatershedToPlace = new ClosestWatershedToPlace();
            Place testPlace = new Place();
            testPlace.CgndbKey = "ABCDE";
            testPlace.CreateAndFlush();
            Watershed testWatershed = new Watershed();
            testWatershed.DrainageCode = "01-00-00-00-00-00";
            testWatershed.Place = testPlace;
            testWatershed.CreateAndFlush();
            closestWatershedToPlace.Place = testPlace;
            closestWatershedToPlace.Watershed = testWatershed;
            closestWatershedToPlace.CreateAndFlush();
        }

        [Test]
        public void TestBelongsToPlace()
        {
            Place expectedPlace = Place.Find("ABCDE");
            ClosestWatershedToPlace closestWatershedToPlace = ClosestWatershedToPlace.FindFirst();
            Assert.AreEqual(expectedPlace, closestWatershedToPlace.Place);
        }

        [Test]
        public void TestBelongsToWatershed()
        {
            Watershed expectedWatershed  = Watershed.Find("01-00-00-00-00-00");
            ClosestWatershedToPlace closestWatershedToPlace = ClosestWatershedToPlace.FindFirst();
            Assert.AreEqual(expectedWatershed, closestWatershedToPlace.Watershed);
        }
        
        [Test]
        public void TestFindByCgndbKey()
        {
            Assert.IsNotNull(ClosestWatershedToPlace.FindByCgndbKey("ABCDE"));
        }

        [Test]
        public void TestIsWithinBasin()
        {
            Watershed watershed = mocks.CreateMock<Watershed>();
            Expect.Call(watershed.IsWithinBasin()).Return(true);
            mocks.ReplayAll();

            ClosestWatershedToPlace closestWatershedToPlace = new ClosestWatershedToPlace();
            closestWatershedToPlace.Watershed = watershed;
            Assert.IsTrue(closestWatershedToPlace.IsWithinBasin());

            mocks.VerifyAll();
        }			

        [Test]
        public void TestIsWithinBasinWhenWatershedIsNull()
        {
            ClosestWatershedToPlace closestWatershedToPlace = new ClosestWatershedToPlace();
            Assert.IsFalse(closestWatershedToPlace.IsWithinBasin());
        }			
    }
}
