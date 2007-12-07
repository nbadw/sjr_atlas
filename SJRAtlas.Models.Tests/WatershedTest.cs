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
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            base.Setup();
            mocks = new MockRepository();
        }

        [Test]
        public void TestFindWatershed()
        {
            string drainageCode = "01-02-03-04-05-06";

            Watershed watershed = new Watershed();
            watershed.Place.CgndbKey = "ABCDE";
            watershed.Place.CreateAndFlush();
            watershed.DrainageCode = drainageCode;
            watershed.CreateAndFlush();

            Watershed dbWatershed = Watershed.Find(drainageCode);
            Assert.IsNotNull(dbWatershed);
            Assert.AreEqual(drainageCode, dbWatershed.DrainageCode);
        }

        [Test]
        public void TestConstructors()
        {
            Watershed watershed;

            watershed = new Watershed();
            Assert.IsNotNull(watershed.Place);

            Place place = mocks.CreateMock<Place>();
            watershed = new Watershed(place);
            Assert.AreEqual(place, watershed.Place);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWatershedConstructorWhenPlaceIsNull()
        {
            new Watershed(null);
        }	

        [Test]
        public void TestProperties()
        {
            Watershed watershed = new Watershed();
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
            properties.Add("StreamOrder", 1);
            properties.Add("UnitType", "TestValue");

            TestHelper.ErrorSummary errors = TestHelper.TestProperties(watershed, properties);
            Assert.IsEmpty(errors, "The following errors occurred during property testing:\n" + errors.GetSummary());
        }

        [Test]
        public void TestBelongsToPlace()
        {
            string drainageCode = "01-02-03-04-05-06";
            string cgndbKey = "ABCDE";

            Watershed watershed = new Watershed();
            watershed.Place.CgndbKey = cgndbKey;
            watershed.Place.CreateAndFlush();
            watershed.DrainageCode = drainageCode;
            watershed.CreateAndFlush();

            Watershed dbWatershed = Watershed.Find(drainageCode);
            Assert.IsNotNull(dbWatershed);
            Assert.IsNotNull(dbWatershed.Place);
            Assert.AreEqual(cgndbKey, dbWatershed.Place.CgndbKey);
        }

        [Test]
        public void TestHasManyWaterBodies()
        {
            string drainageCode = "01-02-03-04-05-06";
            int id1 = 1234;
            int id2 = 5678;

            Watershed watershed = new Watershed();
            watershed.Place.CgndbKey = "ABCDE";
            watershed.Place.CreateAndFlush();
            watershed.DrainageCode = drainageCode;
            watershed.CreateAndFlush();

            WaterBody waterBody1 = new WaterBody();
            waterBody1.Id = id1;
            waterBody1.Watershed = watershed;
            waterBody1.Place = watershed.Place;
            waterBody1.CreateAndFlush();

            WaterBody waterBody2 = new WaterBody();
            waterBody2.Id = id2;
            waterBody2.Watershed = watershed;
            waterBody2.Place = watershed.Place;
            waterBody2.CreateAndFlush();

            watershed.WaterBodies.Add(waterBody1);
            watershed.WaterBodies.Add(waterBody2);
            watershed.SaveAndFlush();

            Watershed dbWatershed = Watershed.Find(drainageCode);
            Assert.IsNotNull(dbWatershed);
            Assert.AreEqual(2, dbWatershed.WaterBodies.Count);
            Assert.AreEqual(id1, dbWatershed.WaterBodies[0].Id);
            Assert.AreEqual(id2, dbWatershed.WaterBodies[1].Id);
        }

        [Test]
        public void TestRelatedPublications()
        {
            string query = "watershed name is the default query";
            Watershed watershed = new Watershed();
            watershed.Name = query;
            watershed.Place.Name = "this should not be searced on";

            Publication[] publications = new Publication[3];
            for (int i = 0; i < publications.Length; i++)
            {
                publications[i] = new Publication();
                publications[i].Title = "Publication where " + query + ": #" + i.ToString();
                publications[i].CreateAndFlush();
            }

            Assert.AreEqual(publications.Length, watershed.RelatedPublications.Count);
        }

        [Test]
        public void TestRelatedPublicationsNeverReturnsNull()
        {
            Watershed watershed = new Watershed();
            IList<IPublication> publications = watershed.RelatedPublications;
            Assert.IsNotNull(publications);
            Assert.AreEqual(0, publications.Count);
        }

        [Test]
        public void TestRelatedInteractiveMaps()
        {
            string query = "place name is the default query";
            Watershed watershed = new Watershed();
            watershed.Name = query;
            watershed.Place.Name = "this should not be searced on";

            InteractiveMap[] interactiveMaps = new InteractiveMap[3];
            for (int i = 0; i < interactiveMaps.Length; i++)
            {
                interactiveMaps[i] = new InteractiveMap();
                interactiveMaps[i].Title = "Interactive Map where " + query + ": #" + i.ToString();
                interactiveMaps[i].CreateAndFlush();
            }

            Assert.AreEqual(interactiveMaps.Length, watershed.RelatedInteractiveMaps.Count);
        }

        [Test]
        public void TestRelatedInteractiveMapsNeverReturnsNull()
        {
            Watershed watershed = new Watershed();
            IList<InteractiveMap> interativeMaps = watershed.RelatedInteractiveMaps;
            Assert.IsNotNull(interativeMaps);
            Assert.AreEqual(0, interativeMaps.Count);
        }

        [Test]
        public void TestFindRelatedDataSets()
        {
            WaterBody waterBody1 = mocks.CreateMock<WaterBody>();
            WaterBody waterBody2 = mocks.CreateMock<WaterBody>();
            Expect.Call(waterBody1.DataSets).Return(new DataSet[2]);
            Expect.Call(waterBody2.DataSets).Return(new DataSet[3]);
            mocks.ReplayAll();

            Watershed watershed = new Watershed();
            watershed.WaterBodies.Add(waterBody1);
            watershed.WaterBodies.Add(waterBody2);
            Assert.AreEqual(5, watershed.DataSets.Length);
            mocks.VerifyAll();
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
        public void TestFlowsIntoWhenDrainsIntoIsNull()
        {
            string flowsInto = "LEVEL1";
            Watershed watershed = new Watershed();
            watershed.DrainsInto = null;
            watershed.Level1No = "01";
            watershed.Level1Name = flowsInto;
            watershed.Level2No = "02";
            watershed.Level2Name = "LEVEL2";
            Assert.AreEqual(flowsInto, watershed.FlowsInto);
        }

        [Test]
        public void TestFlowsIntoWhenDrainsIntoIsNotNull()
        {
            string flowsInto = "TEST VALUE";
            Watershed watershed = new Watershed();
            watershed.DrainsInto = flowsInto;
            Assert.AreEqual(flowsInto, watershed.DrainsInto);
        }

        [Test]
        public void TestGetCoordinate()
        {
            double lat = 37.3;
            double lon = 73.7;
            Place place = mocks.CreateMock<Place>();
            Expect.Call(place.GetCoordinate()).Return(new LatLngCoord(lat, lon));
            mocks.ReplayAll();

            Watershed watershed = new Watershed();
            watershed.Place = place;

            LatLngCoord coord = watershed.GetCoordinate();
            Assert.AreEqual(lat, coord.Latitude);
            Assert.AreEqual(lon, coord.Longitude);

            mocks.VerifyAll();
        }

        [Test]
        public void TestIsWithinBasin()
        {
            string drainageCode = "01-00-00-00-00-00";
            Watershed watershed = new Watershed();
            watershed.DrainageCode = drainageCode;
            Assert.IsTrue(watershed.IsWithinBasin());
        }

        [Test]
        public void TestIsWithinBasinWithBadlyFormattedDrainageCodes()
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
            Watershed watershed = new Watershed();

            foreach (string drainageCode in notInBasinCodes)
            {
                watershed.DrainageCode = drainageCode;
                Assert.IsFalse(watershed.IsWithinBasin(), "IsWithinBasin should return false for value " + drainageCode);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPlaceCannotBeSetToNull()
        {
            Watershed watershed = new Watershed();
            watershed.Place = null;
        }
    }
}
