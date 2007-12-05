using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class WatershedTest : AbstractModelTestCase
    {
        private Watershed watershed;
        private Place place; 
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            base.Init();

            place = new Place();
            place.CgndbKey = "ABCDE";
            place.CreateAndFlush();

            watershed = new Watershed();
            watershed.DrainageCode = "01-00-00-00-00-00";
            watershed.Name = "Saint John River";
            watershed.Place = place;
            watershed.CreateAndFlush();

            mocks = new MockRepository();
        }

        [Test]
        public void FindWatershed()
        {
            Watershed found = Watershed.Find("01-00-00-00-00-00");
            Assert.AreEqual(watershed, found);
        }	

        [Test]
        public void TestProperties()
        {
            Watershed watershed = new Watershed();
            watershed.Place = new Place();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("AreaHA", float.MaxValue);
            properties.Add("AreaPercent", float.MaxValue);
            properties.Add("BorderInd", "1");
            properties.Add("CgndbKey", "ABCDE");                
            properties.Add("County", "Northumberland");
            properties.Add("ConciseTerm", "TestValue");
            properties.Add("ConciseType", "TestValue");
            properties.Add("CoordAccM", "TestValue");
            properties.Add("Datum", "NAD83");
            properties.Add("DrainageCode", "01-02-03-04-05-06");
            properties.Add("DrainsInto", "Bay of Fundy");
            properties.Add("FeatureId", "TestValue");
            properties.Add("GenericTerm", "TestValue");
            properties.Add("Level1No", "01");
            properties.Add("Level1Name", "Level1");
            properties.Add("Level2No", "02");
            properties.Add("Level2Name", "Level2");
            properties.Add("Level3No", "03");
            properties.Add("Level3Name", "Level3");
            properties.Add("Level4No", "04");
            properties.Add("Level4Name", "Level4");
            properties.Add("Level5No", "05");
            properties.Add("Level5Name", "Level5");
            properties.Add("Level6No", "06");
            properties.Add("Level6Name", "Level6");
            properties.Add("Name", "Saint John River");
            properties.Add("NameStatus", "Official");
            properties.Add("Latitude", 3.7);
            properties.Add("Longitude", 7.3);
            properties.Add("NtsMap", "TestValue");
            properties.Add("Region", "NB");
            properties.Add("Repository", mocks.CreateMock<IAtlasRepository>());
            properties.Add("StreamOrder", 1);
            properties.Add("UnitType", "TestValue");

            TestHelper.ErrorSummary errors = TestHelper.TestProperties(watershed, properties);
            Assert.IsEmpty(errors, "The following errors occurred during property testing:\n" + errors.GetSummary());
        }

        [Test]
        public void TestSetActiveRecordPlaceAlsoSetsPlace()
        {
            Watershed watershed = new Watershed();
            Place place = new Place();
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWatershedConstructorWhenAtlasArgumentIsNull()
        {
            new Watershed(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWatershedConstructorWhenPlaceArgumentIsNull()
        {
            new Watershed(mocks.CreateMock<IAtlasRepository>(), null);
        }	

        [Test]
        public void TestWatershedConstructorSetsDefaultPlaceAndRepository()
        {
            Watershed watershed = new Watershed();
            Assert.IsNotNull(watershed.Place);
            Assert.IsNotNull(watershed.Repository);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlaceCannotBeSetToNull()
        {
            Watershed watershed = new Watershed();
            watershed.Place = null;
        }

        [Test]
        public void TestBelongsToPlace()
        {
            Watershed found = Watershed.Find("01-00-00-00-00-00");
            Assert.AreEqual(place, found.Place);
        }

        [Test]
        public void TestBelongsToPlaceIsSetToDefaultIfPlaceDoesNotExist()
        {
            // test code goes here
            Assert.Fail();
        }	

        [Test]
        public void FindRelatedInteractiveMaps()
        {
            // test code goes here
            Assert.Fail();
        }	

        [Test]
        public void TestFindRelatedPublications()
        {
            // test code goes here
            Assert.Fail();
        }

        [Test]
        public void TestFindRelatedDataSets()
        {
            // test code goes here
            Assert.Fail();
        }

        [Test]
        public void FindAllByDrainageCodeOrUnitName()
        {
            // test code goes here
            Assert.Fail();
        }

        [Test]
        public void TestTributaryOf()
        {
            Watershed watershed = new Watershed();

            watershed.Level1No = "01";
            watershed.Level1Name = "LEVEL1";
            Assert.AreEqual(String.Empty, watershed.TributaryOf);

            watershed.Level2No = "02";
            watershed.Level2Name = "LEVEL2";
            Assert.AreEqual("LEVEL1", watershed.TributaryOf);

            watershed.Level3No = "03";
            watershed.Level3Name = "LEVEL3";
            Assert.AreEqual("LEVEL1 - LEVEL2", watershed.TributaryOf);

            watershed.Level4No = "04";
            watershed.Level4Name = "LEVEL4";
            Assert.AreEqual("LEVEL1 - LEVEL2 - LEVEL3", watershed.TributaryOf);

            watershed.Level5No = "05";
            watershed.Level5Name = "LEVEL5";
            Assert.AreEqual("LEVEL1 - LEVEL2 - LEVEL3 - LEVEL4", watershed.TributaryOf);

            watershed.Level6No = "06";
            watershed.Level6Name = "LEVEL6";
            Assert.AreEqual("LEVEL1 - LEVEL2 - LEVEL3 - LEVEL4 - LEVEL5", watershed.TributaryOf);
        }

        [Test]
        public void TestFlowsInto()
        {
            // test code goes here
            Assert.Fail();
        }

        [Test]
        public void TestGetCoordinate()
        {
            double lat = 37.3;
            double lon = 73.7;
            IPlace place = mocks.CreateMock<IPlace>();

            Expect.Call(place.Latitude).Repeat.Twice().Return(lat);
            Expect.Call(place.Longitude).Repeat.Twice().Return(lon);

            mocks.ReplayAll();

            Watershed watershed = new Watershed();
            watershed.Place = place;

            LatLngCoord coord = watershed.GetCoordinate();
            Assert.AreEqual(lat, place.Latitude);
            Assert.AreEqual(lon, place.Longitude);

            mocks.VerifyAll();
        }

        [Test]
        public void IsWithinBasin()
        {
            string drainageCode = "01-00-00-00-00-00";
            watershed.DrainageCode = drainageCode;
            Assert.IsTrue(watershed.IsWithinBasin());
        }

        [Test]
        public void IsWithinBasinWithBadlyFormattedDrainageCodes()
        {
            string[] notInBasinCodes = new string[]
            {
                "00-00-00-00-00-00",
                "02-00-00-00-00-00",
                "",
                null,
                "ab-cd-ef-gh-ij-kl",
                "01-.."
            };

            foreach (string drainageCode in notInBasinCodes)
            {
                watershed.DrainageCode = drainageCode;
                Assert.IsFalse(watershed.IsWithinBasin(), "IsWithinBasin should return false for value " + drainageCode);
            }
        }
		
    }
}
