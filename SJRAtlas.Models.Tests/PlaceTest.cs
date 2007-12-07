using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class PlaceTest : AbstractModelTestCase
    {
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            base.Setup();
            mocks = new MockRepository();
        }

        [Test]
        public void TestGetCoordinate()
        {
            double lat = 3.7;
            double lon = 7.3;
            Place place = new Place();
            place.Latitude = lat;
            place.Longitude = lon;
            LatLngCoord coordinate = place.GetCoordinate();
            Assert.IsNotNull(coordinate);
            Assert.AreEqual(lat, coordinate.Latitude);
            Assert.AreEqual(lon, coordinate.Longitude);
        }

        [Test]
        public void TestIsWithinBasin()
        {
            Place place = new Place();
            ClosestWatershedToPlace closestWatershedToPlace = mocks.CreateMock<ClosestWatershedToPlace>();
            Expect.Call(closestWatershedToPlace.IsWithinBasin()).Return(true);
            mocks.ReplayAll();
            place.ClosestWatershedToPlace = closestWatershedToPlace;
            Assert.IsTrue(place.IsWithinBasin());
            mocks.VerifyAll();
        }

        [Test]
        public void TestIsWithinBasinWhenClosestWatershedToPlaceIsNull()
        {
            Place place = new Place();
            place.ClosestWatershedToPlace = null;
            Assert.IsFalse(place.IsWithinBasin());
        }	

        [Test]
        public void TestClosestWatershedToPlace()
        {
            string cgndbKey = "ABCDE";
            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.CreateAndFlush();

            Watershed watershed = new Watershed();
            watershed.DrainageCode = "01-02-03-04-05-06";
            watershed.Place = place;
            watershed.CreateAndFlush();

            ClosestWatershedToPlace closestWatershedToPlace = new ClosestWatershedToPlace();
            closestWatershedToPlace.Place = place;
            closestWatershedToPlace.Watershed = watershed;
            closestWatershedToPlace.CreateAndFlush();

            Place dbPlace = Place.Find(cgndbKey);
            Assert.IsNotNull(dbPlace);
            Assert.AreEqual(closestWatershedToPlace, place.ClosestWatershedToPlace);
        }	

        [Test]
        public void TestRelatedPublications()
        {
            string query = "place name is the default query";
            Place place = new Place();
            place.Name = query;

            Publication[] publications = new Publication[3];
            for (int i = 0; i < publications.Length; i++)
            {
                publications[i] = new Publication();
                publications[i].Title = "Publication where " + query + ": #" + i.ToString();
                publications[i].CreateAndFlush();
            }

            Assert.AreEqual(publications.Length, place.RelatedPublications.Count);
        }

        [Test]
        public void TestRelatedPublicationsNeverReturnsNull()
        {
            Place place = new Place();
            IList<IPublication> publications = place.RelatedPublications;
            Assert.IsNotNull(publications);
            Assert.AreEqual(0, publications.Count);
        }

        [Test]
        public void TestRelatedInteractiveMaps()
        {
            string query = "place name is the default query";
            Place place = new Place();
            place.Name = query;

            InteractiveMap[] interactiveMaps = new InteractiveMap[3];
            for (int i = 0; i < interactiveMaps.Length; i++)
            {
                interactiveMaps[i] = new InteractiveMap();
                interactiveMaps[i].Title = "Interactive Map where " + query + ": #" + i.ToString();
                interactiveMaps[i].CreateAndFlush();
            }

            Assert.AreEqual(interactiveMaps.Length, place.RelatedInteractiveMaps.Count);
        }

        [Test]
        public void TestRelatedInteractiveMapsNeverReturnsNull()
        {
            Place place = new Place();
            IList<InteractiveMap> interativeMaps = place.RelatedInteractiveMaps;
            Assert.IsNotNull(interativeMaps);
            Assert.AreEqual(0, interativeMaps.Count);
        }

        [Test]
        public void TestProperties()
        {
            Place place = new Place();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("CgndbKey", "ABCDE");
            properties.Add("County", "Northumberland");
            properties.Add("ConciseTerm", "TestValue");
            properties.Add("ConciseType", "TestValue");
            properties.Add("CoordAccM", "TestValue");
            properties.Add("Datum", "NAD83");
            properties.Add("FeatureId", "TestValue");
            properties.Add("GenericTerm", "TestValue");
            properties.Add("Name", "Saint John River");
            properties.Add("NameStatus", "Official");
            properties.Add("Latitude", 3.7);
            properties.Add("Longitude", 7.3);
            properties.Add("NtsMap", "TestValue");
            properties.Add("Region", "NB");
            TestHelper.ErrorSummary errors = TestHelper.TestProperties(place, properties);
            Assert.IsEmpty(errors, "The following errors occurred during property testing:\n" + errors.GetSummary());
        }
    }
}
