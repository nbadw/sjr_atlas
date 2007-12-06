using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SJRAtlas.Models.Finders;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class PlaceTest : AbstractModelTestCase
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
            Place place;
            IAtlasRepository repository = mocks.CreateMock<IAtlasRepository>();

            place = new Place();
            Assert.IsNotNull(place.Repository);

            place = new Place(repository);
            Assert.AreEqual(repository, place.Repository);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWhenRepositoryIsNull()
        {
            new Place(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRepositoryCannotBeSetToNull()
        {
            Place place = new Place();
            place.Repository = null;
        }

        [Test]
        public void TestGetId()
        {
            Place place = new Place();
            Assert.IsNull(place.CgndbKey);
            Assert.IsNull(place.GetId());
            place.CgndbKey = "ABCDE";
            Assert.AreEqual(place.CgndbKey, place.GetId());
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
            Place place = new Place();
            IAtlasRepository repository = mocks.CreateMock<IAtlasRepository>();
            place.Repository = repository;
            ClosestWatershedToPlaceFinder finder = mocks.CreateMock<ClosestWatershedToPlaceFinder>();
            ClosestWatershedToPlace closestWatershedToPlace = mocks.CreateMock<ClosestWatershedToPlace>();
            Expect.Call(repository.GetFinder<ClosestWatershedToPlaceFinder>()).Return(finder);
            Expect.Call(finder.FindByCgndbKey(place.CgndbKey)).Return(closestWatershedToPlace);
            mocks.ReplayAll();
            Assert.AreEqual(closestWatershedToPlace, place.ClosestWatershedToPlace);
            mocks.VerifyAll();
        }

        [Test]
        public void TestClosestWatershedToPlaceOnlyCalledOnce()
        {
            Place place = new Place();
            IAtlasRepository repository = mocks.CreateMock<IAtlasRepository>();
            place.Repository = repository;
            ClosestWatershedToPlaceFinder finder = mocks.CreateMock<ClosestWatershedToPlaceFinder>();
            ClosestWatershedToPlace closestWatershedToPlace = mocks.CreateMock<ClosestWatershedToPlace>();
            Expect.Call(repository.GetFinder<ClosestWatershedToPlaceFinder>())
                .Repeat.Once().Return(finder);
            Expect.Call(finder.FindByCgndbKey(place.CgndbKey))
                .Repeat.Once().Return(closestWatershedToPlace);
            mocks.ReplayAll();
            ClosestWatershedToPlace callOne = place.ClosestWatershedToPlace;
            ClosestWatershedToPlace callTwo = place.ClosestWatershedToPlace;
            mocks.VerifyAll();
        }	

        [Test]
        public void TestRelatedPublications()
        {
            Place place = new Place();
            place.Repository = mocks.CreateMock<IAtlasRepository>();
            base.TestRelatedPublications(mocks, place, place.Repository);
        }

        [Test]
        public void TestRelatedPublicationsNeverReturnsNull()
        {
            Place place = new Place();
            place.Repository = mocks.CreateMock<IAtlasRepository>();
            base.TestRelatedPublicationsNeverReturnsNull(mocks, place, place.Repository);
        }

        [Test]
        public void TestRelatedInteractiveMaps()
        {
            Place place = new Place();
            place.Repository = mocks.CreateMock<IAtlasRepository>();
            base.TestRelatedInteractiveMaps(mocks, place, place.Repository);
        }

        [Test]
        public void TestRelatedInteractiveMapsNeverReturnsNull()
        {
            Place place = new Place();
            place.Repository = mocks.CreateMock<IAtlasRepository>();
            base.TestRelatedInteractiveMapsNeverReturnsNull(mocks, place, place.Repository);
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
            properties.Add("Repository", mocks.CreateMock<IAtlasRepository>());
            TestHelper.ErrorSummary errors = TestHelper.TestProperties(place, properties);
            Assert.IsEmpty(errors, "The following errors occurred during property testing:\n" + errors.GetSummary());
        }
    }
}
