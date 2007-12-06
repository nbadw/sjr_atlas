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
            base.Init();
            mocks = new MockRepository();
        }

        [Test]
        public void TestConstructors()
        {
            WaterBody waterbody;
            IAtlasRepository repository = mocks.CreateMock<IAtlasRepository>();
            Place place = mocks.CreateMock<Place>();
            Watershed watershed = mocks.CreateMock<Watershed>();

            waterbody = new WaterBody();
            Assert.IsNotNull(waterbody.Repository);
            Assert.IsNotNull(waterbody.Place);
            Assert.IsNotNull(waterbody.Watershed);

            waterbody = new WaterBody(repository);
            Assert.AreEqual(repository, waterbody.Repository);
            Assert.IsNotNull(waterbody.Place);
            Assert.IsNotNull(waterbody.Watershed);

            waterbody = new WaterBody(repository, place, watershed);
            Assert.AreEqual(repository, waterbody.Repository);
            Assert.AreEqual(place, waterbody.Place);
            Assert.AreEqual(watershed, waterbody.Watershed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWhenNullRepositoryIsPassed()
        {
            Place place = mocks.CreateMock<Place>();
            Watershed watershed = mocks.CreateMock<Watershed>();
            new WaterBody(null, place, watershed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWhenNullPlaceIsPassed()
        {
            IAtlasRepository repository = mocks.CreateMock<IAtlasRepository>();
            Watershed watershed = mocks.CreateMock<Watershed>();
            new WaterBody(repository, null, watershed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWhenNullWatershedIsPassed()
        {
            IAtlasRepository repository = mocks.CreateMock<IAtlasRepository>();
            Place place = mocks.CreateMock<Place>();
            new WaterBody(repository, place, null);
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
            properties.Add("Repository", mocks.CreateMock<IAtlasRepository>());
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
            WaterBody waterbody = new WaterBody();
            waterbody.Repository = mocks.CreateMock<IAtlasRepository>();
            base.TestRelatedPublications(mocks, waterbody, waterbody.Repository);
        }

        [Test]
        public void TestRelatedPublicationsNeverReturnsNull()
        {
            WaterBody waterbody = new WaterBody();
            waterbody.Repository = mocks.CreateMock<IAtlasRepository>();
            base.TestRelatedPublicationsNeverReturnsNull(mocks, waterbody, waterbody.Repository);
        }

        [Test]
        public void TestRelatedInteractiveMaps()
        {
            WaterBody waterbody = new WaterBody();
            waterbody.Repository = mocks.CreateMock<IAtlasRepository>();
            base.TestRelatedInteractiveMaps(mocks, waterbody, waterbody.Repository);
        }

        [Test]
        public void TestRelatedInteractiveMapsNeverReturnsNull()
        {
            WaterBody waterbody = new WaterBody();
            waterbody.Repository = mocks.CreateMock<IAtlasRepository>();
            base.TestRelatedInteractiveMapsNeverReturnsNull(mocks, waterbody, waterbody.Repository);
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
        public void TestRepositoryCannotBeSetToNull()
        {
            WaterBody waterbody = new WaterBody();
            waterbody.Repository = null;
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
