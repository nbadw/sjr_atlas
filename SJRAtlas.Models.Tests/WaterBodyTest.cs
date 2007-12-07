using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class WaterBodyTest : AbstractModelTestCase
    {
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            base.Setup();
            mocks = new MockRepository();
        }

        [Test]
        public void TestConstructors()
        {
            WaterBody waterbody;
            Place place = mocks.CreateMock<Place>();
            Watershed watershed = mocks.CreateMock<Watershed>();

            waterbody = new WaterBody();
            Assert.IsNotNull(waterbody.Place);
            Assert.IsNotNull(waterbody.Watershed);

            waterbody = new WaterBody(place, watershed);
            Assert.AreEqual(place, waterbody.Place);
            Assert.AreEqual(watershed, waterbody.Watershed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWhenNullPlaceIsPassed()
        {
            Watershed watershed = mocks.CreateMock<Watershed>();
            new WaterBody(null, watershed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWhenNullWatershedIsPassed()
        {
            Place place = mocks.CreateMock<Place>();
            new WaterBody(place, null);
        }	

        [Test]
        public void FindWaterBody()
        {
            int id = 37;
            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
            waterbody.Place.CgndbKey = "ABCDE";
            waterbody.Place.CreateAndFlush();
            waterbody.Watershed.DrainageCode = "01-00-00-00-00-00";
            waterbody.Watershed.CreateAndFlush();
            waterbody.CreateAndFlush();
            WaterBody dbWaterbody = WaterBody.Find(id);
            Assert.AreEqual(id, dbWaterbody.Id);
        }
	
        [Test]
        public void TestProperties()
        {
            WaterBody waterbody = new WaterBody();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Abbreviation", "ABBREV");
            properties.Add("AltCgndbKey", "FGHIJ");
            properties.Add("AltName", "Another Name");
            properties.Add("CgndbKey", "ABCDE");
            properties.Add("ComplexId", 12345);
            properties.Add("County", "Northumberland");
            properties.Add("ConciseTerm", "TestValue");
            properties.Add("ConciseType", "TestValue");
            properties.Add("CoordAccM", "TestValue");
            properties.Add("Datum", "NAD83");
            properties.Add("FeatureId", "TestValue");
            properties.Add("FlowsIntoWaterBodyId", 12345);
            properties.Add("FlowsIntoWaterBodyName", "TestValue");
            properties.Add("GenericTerm", "TestValue");
            properties.Add("Id", 37);
            properties.Add("Name", "Saint John River");
            properties.Add("NameStatus", "Official");
            properties.Add("Latitude", 3.7);
            properties.Add("Longitude", 7.3);
            properties.Add("NtsMap", "TestValue");
            properties.Add("Region", "NB");
            properties.Add("SurveyedInd", "TestValue");
            properties.Add("Type", "TestValue");
            TestHelper.ErrorSummary errors = TestHelper.TestProperties(waterbody, properties);
            Assert.IsEmpty(errors, "The following errors occurred during property testing:\n" + errors.GetSummary());        
        }

        [Test]
        public void TestBelongsToPlace()
        {
            int id = 37;
            string cgndbKey = "ABCDE";

            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
            waterbody.Place.CgndbKey = cgndbKey;
            waterbody.Place.CreateAndFlush();
            waterbody.Watershed.DrainageCode = "00-00-00-00-00-00";
            waterbody.Watershed.CreateAndFlush();
            waterbody.CreateAndFlush();
            
            WaterBody dbWaterbody = WaterBody.Find(id);
            Assert.IsNotNull(dbWaterbody);
            Assert.IsNotNull(dbWaterbody.Place);
            Assert.AreEqual(cgndbKey, dbWaterbody.Place.CgndbKey);
        }

        [Test]
        public void TestBelongsToWatershed()
        {
            int id = 73;
            string drainageCode = "01-00-00-00-00-00";

            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
            waterbody.Place.CgndbKey = "ABCDE";
            waterbody.Place.CreateAndFlush();
            waterbody.Watershed.DrainageCode = drainageCode;
            waterbody.Watershed.CreateAndFlush();
            waterbody.CreateAndFlush();

            WaterBody dbWaterbody = WaterBody.Find(id);
            Assert.IsNotNull(dbWaterbody);
            Assert.IsNotNull(dbWaterbody.Watershed);
            Assert.AreEqual(drainageCode, dbWaterbody.Watershed.DrainageCode);
        }

        [Test]
        public void TestRelatedPublications()
        {
            string query = "watershed name is the default query";
            WaterBody waterbody = new WaterBody();
            waterbody.Name = query;
            waterbody.Place.Name = "this should not be searced on";

            Publication[] publications = new Publication[3];
            for (int i = 0; i < publications.Length; i++)
            {
                publications[i] = new Publication();
                publications[i].Title = "Publication where " + query + ": #" + i.ToString();
                publications[i].CreateAndFlush();
            }

            Assert.AreEqual(publications.Length, waterbody.RelatedPublications.Count);
        }

        [Test]
        public void TestRelatedPublicationsNeverReturnsNull()
        {
            WaterBody waterbody = new WaterBody();
            IList<IPublication> publications = waterbody.RelatedPublications;
            Assert.IsNotNull(publications);
            Assert.AreEqual(0, publications.Count);
        }

        [Test]
        public void TestRelatedInteractiveMaps()
        {
            string query = "place name is the default query";
            WaterBody waterbody = new WaterBody();
            waterbody.Name = query;
            waterbody.Place.Name = "this should not be searced on";

            InteractiveMap[] interactiveMaps = new InteractiveMap[3];
            for (int i = 0; i < interactiveMaps.Length; i++)
            {
                interactiveMaps[i] = new InteractiveMap();
                interactiveMaps[i].Title = "Interactive Map where " + query + ": #" + i.ToString();
                interactiveMaps[i].CreateAndFlush();
            }

            Assert.AreEqual(interactiveMaps.Length, waterbody.RelatedInteractiveMaps.Count);
        }

        [Test]
        public void TestRelatedInteractiveMapsNeverReturnsNull()
        {
            WaterBody waterbody = new WaterBody();
            IList<InteractiveMap> interativeMaps = waterbody.RelatedInteractiveMaps;
            Assert.IsNotNull(interativeMaps);
            Assert.AreEqual(0, interativeMaps.Count);
        }

        [Test]
        public void TestGetCoordinate()
        {
            double lat = 3.7;
            double lon = 7.3;
            WaterBody waterbody = new WaterBody();
            waterbody.Latitude = lat;
            waterbody.Longitude = lon;
            LatLngCoord coordinate = waterbody.GetCoordinate();
            Assert.IsNotNull(coordinate);
            Assert.AreEqual(lat, coordinate.Latitude);
            Assert.AreEqual(lon, coordinate.Longitude);
        }

        [Test]
        public void TestIsWithinBasin()
        {
            WaterBody waterbody = new WaterBody();
            Watershed watershed = mocks.CreateMock<Watershed>();
            waterbody.Watershed = watershed;
            Expect.Call(watershed.IsWithinBasin()).Return(true);
            mocks.ReplayAll();
            Assert.IsTrue(waterbody.IsWithinBasin());
            mocks.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPlaceCannotBeSetToNull()
        {
            WaterBody waterbody = new WaterBody();
            waterbody.Place = null;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWatershedCannotBeSetToNull()
        {
            WaterBody waterbody = new WaterBody();
            waterbody.Watershed = null;
        }

        [Test]
        public void TestRelatedDataSets()
        {
            int id = 50001;
            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
            waterbody.Place.CgndbKey = "ABCDE";
            waterbody.Place.CreateAndFlush();
            waterbody.Watershed.DrainageCode = "01-00-00-00-00-00";
            waterbody.Watershed.CreateAndFlush();
            waterbody.CreateAndFlush();

            DataSet dataset1 = new DataSet();
            dataset1.CreateAndFlush();
            DataSet dataset2 = new DataSet();
            dataset2.CreateAndFlush();

            waterbody.DataSetList.Add(dataset1);
            waterbody.DataSetList.Add(dataset2);
            waterbody.SaveAndFlush();

            waterbody = WaterBody.Find(id);
            Assert.IsNotNull(waterbody);
            Assert.AreEqual(2, waterbody.DataSets.Length);
        }
    }
}
