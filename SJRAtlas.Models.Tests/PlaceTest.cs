using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class PlaceTest
    {
        private Place place;
        private MockRepository mocks;
        private IAtlasRepository repository;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            repository = mocks.CreateMock<IAtlasRepository>();
            place = new Place(repository);
        }

        [Test]
        public void TestConstructors()
        {
            Place place1 = new Place();
            Assert.IsNotNull(place1.Repository);
            Place place2 = new Place(repository);
            Assert.AreEqual(repository, place2.Repository);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorWhenRepositoryParameterIsNull()
        {
            new Place(null);
        }

        [Test]
        public void TestGetId()
        {
            Assert.IsNull(place.CgndbKey);
            Assert.IsNull(place.GetId());
            place.CgndbKey = "ABCDE";
            Assert.AreEqual(place.CgndbKey, place.GetId());
        }

        [Test]
        public void TestGetCoordinate()
        {
            place.Latitude = 3.7;
            place.Longitude = 7.3;
            LatLngCoord coordinate = place.GetCoordinate();
            Assert.IsNotNull(coordinate);
            Assert.AreEqual(place.Latitude, coordinate.Latitude);
            Assert.AreEqual(place.Longitude, coordinate.Longitude);
        }

        [Test]
        public void TestIsWithinBasin()
        {
            ClosestWatershedToPlace closest = mocks.CreateMock<ClosestWatershedToPlace>();            
            Expect.Call(closest.IsWithinBasin()).Return(true);
            mocks.ReplayAll();

            Assert.IsTrue(place.IsWithinBasin());

            mocks.VerifyAll();
        }

        [Test]
        public void TestPlaceIsNotWithinBasin()
        {
            ClosestWatershedToPlace closest = mocks.CreateMock<ClosestWatershedToPlace>();
            
            Expect.Call(closest.IsWithinBasin()).Return(false);
            mocks.ReplayAll();

            Assert.IsFalse(place.IsWithinBasin());

            mocks.VerifyAll();
        }

        [Test]
        public void TestPlaceIsNotWithinBasinWhenClosestWatershedDoesNotExist()
        {
            
            mocks.ReplayAll();

            Assert.IsFalse(place.IsWithinBasin());

            mocks.VerifyAll();
        }

        [Test]
        public void TestRelatedPublications()
        {
            //Expect.Call(repository.FindByDefaultQuery<IPublication>("%" + place.Name + "%"))
            //    .Return(new IPublication[0]);
            mocks.ReplayAll();

            IPublication[] publications = place.RelatedPublications;
            Assert.IsNotNull(publications);

            mocks.VerifyAll();
        }

        [Test]
        public void TestRelatedPublicationsNeverReturnsNull()
        {
            //Expect.Call(repository.FindByDefaultQuery<IPublication>("%" + place.Name + "%"))
            //    .Return(null);
            mocks.ReplayAll();

            IPublication[] publications = place.RelatedPublications;
            Assert.IsNotNull(publications);

            mocks.VerifyAll();
        }

        [Test]
        public void TestInteractiveMaps()
        {
            //Expect.Call(repository.FindByDefaultQuery<InteractiveMap>("%" + place.Name + "%"))
            //    .Return(new InteractiveMap[0]);
            mocks.ReplayAll();

            InteractiveMap[] maps = place.RelatedInteractiveMaps;
            Assert.IsNotNull(maps);

            mocks.VerifyAll();
        }

        [Test]
        public void TestInteractiveMapsNeverReturnsNull()
        {
            //Expect.Call(repository.FindByDefaultQuery<InteractiveMap>("%" + place.Name + "%"))
            //    .Return(null);
            mocks.ReplayAll();

            InteractiveMap[] maps = place.RelatedInteractiveMaps;
            Assert.IsNotNull(maps);

            mocks.VerifyAll();
        }

        [Test]
        public void TestProperties()
        {
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
            Assert.AreEqual(0, errors.Count, "The following errors occurred during property testing:\n" + errors.GetSummary());
        }
    }
}
