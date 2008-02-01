using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Models.Tests.Atlas
{
    [TestFixture]
    public class LatLngCoordinateTest
    {
        [Test]
        public void TestLatitude()
        {
            Assert.IsEmpty(TestHelper.TestProperty(new LatLngCoord(0, 0), "Latitude", 37.3));
        }

        [Test]
        public void TestLongitude()
        {
            Assert.IsEmpty(TestHelper.TestProperty(new LatLngCoord(0, 0), "Longitude", 73.7));
        }	
    }
}
