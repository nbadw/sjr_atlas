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
        private Place place;
        private MockRepository mocks;
        private IAtlasRepository repository;

        [SetUp]
        public void Setup()
        {
            base.Init();
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
            place.ClosestWatershedToPlace = null;
            Assert.IsFalse(place.IsWithinBasin());
        }	

        [Test]
        public void TestClosestWatershedToPlace()
        {
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
            IPublicationFinder finder = mocks.CreateMock<IPublicationFinder>();
            Expect.Call(repository.GetFinder<IPublicationFinder>()).Return(finder);
            Expect.Call(finder.FindAllByQuery(null)).IgnoreArguments().Return(new IPublication[3]);
            mocks.ReplayAll();
            IPublication[] publications = place.RelatedPublications;
            Assert.AreEqual(3, publications.Length);
            mocks.VerifyAll();
        }

        [Test]
        public void TestRelatedPublicationsNeverReturnsNull()
        {
            IPublicationFinder finder = mocks.CreateMock<IPublicationFinder>();
            Expect.Call(repository.GetFinder<IPublicationFinder>()).Return(finder);
            Expect.Call(finder.FindAllByQuery(null)).IgnoreArguments().Return(null);
            mocks.ReplayAll();
            IPublication[] publications = place.RelatedPublications;
            Assert.IsNotNull(publications);
            Assert.IsEmpty(publications);
            mocks.VerifyAll();
        }

        [Test]
        public void TestRelatedInteractiveMaps()
        {
            InteractiveMapFinder finder = mocks.CreateMock<InteractiveMapFinder>();
            Expect.Call(repository.GetFinder<InteractiveMapFinder>()).Return(finder);
            Expect.Call(finder.FindAllByQuery(null)).IgnoreArguments().Return(new InteractiveMap[3]);
            mocks.ReplayAll();
            InteractiveMap[] interactiveMaps = place.RelatedInteractiveMaps;
            Assert.AreEqual(3, interactiveMaps.Length);
            mocks.VerifyAll();
        }

        [Test]
        public void TestRelatedInteractiveMapsNeverReturnsNull()
        {
            InteractiveMapFinder finder = mocks.CreateMock<InteractiveMapFinder>();
            Expect.Call(repository.GetFinder<InteractiveMapFinder>()).Return(finder);
            Expect.Call(finder.FindAllByQuery(null)).IgnoreArguments().Return(null);
            mocks.ReplayAll();
            InteractiveMap[] interactiveMaps = place.RelatedInteractiveMaps;
            Assert.IsNotNull(interactiveMaps);
            Assert.IsEmpty(interactiveMaps);
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
            Assert.IsEmpty(errors, "The following errors occurred during property testing:\n" + errors.GetSummary());
        }
    }
}
