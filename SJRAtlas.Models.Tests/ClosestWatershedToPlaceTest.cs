using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class ClosestWatershedToPlaceTest : AbstractModelTestCase
    {
        [SetUp]
        public void Setup()
        {
            base.Init();
            ClosestWatershedToPlace closestWatershedToPlace = new ClosestWatershedToPlace();
            Place testPlace = new Place();
            testPlace.CgndbKey = "ABCDE";
            testPlace.CreateAndFlush();
            Watershed testWatershed = new Watershed();
            testWatershed.DrainageCode = "01-00-00-00-00-00";
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
    }
}
