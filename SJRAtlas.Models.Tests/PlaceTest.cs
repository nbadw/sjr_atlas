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
        public override void Setup()
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
            IList<Publication> publications = place.RelatedPublications;
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

        [Test]
        public void TestFindAllByQuery()
        {
            string[] placeNames = { "Saint", "Saint John", "Saint John River",
                "Hammond River", "Fredericton", "Moncton", "Miramichi River" };
            for(int i=0; i < placeNames.Length; i++)
            {
                Place place = new Place();
                place.Name = placeNames[i];
                // XXX: this is technically not a valid CGNDB Key but no validation is performed
                place.CgndbKey = i.ToString();
                place.Create();
            }

            Flush();

            Assert.AreEqual(3, Place.FindAllByQuery("Saint").Count, "Query for 'Saint'");
            Assert.AreEqual(3, Place.FindAllByQuery("saint").Count, "Query for 'saint'");
            Assert.AreEqual(1, Place.FindAllByQuery("Fredericton").Count, "Query for 'Fredericton'");
            Assert.AreEqual(2, Place.FindAllByQuery("M").Count, "Query for 'M' returned");
            Assert.AreEqual(0, Place.FindAllByQuery("John").Count, "Query for 'John'");
            Assert.AreEqual(0, Place.FindAllByQuery("River").Count, "Query for 'River'");
            Assert.AreEqual(3, Place.FindAllByQuery(" Saint").Count, "Query for ' Saint'");
            Assert.AreEqual(1, Place.FindAllByQuery("Hammond River ").Count, "Query for 'Hammond River '");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFindAllByQueryThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            Place.FindAllByQuery(null);
        }	
    }
}
