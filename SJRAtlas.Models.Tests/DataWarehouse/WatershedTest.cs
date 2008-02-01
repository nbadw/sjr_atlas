using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SJRAtlas.Models.DataWarehouse;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Models.Tests.DataWarehouse
{
    [TestFixture]
    public class WatershedTest : AbstractModelTestCase
    {
        private MockRepository mocks;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            mocks = new MockRepository();
        }

        [Test]
        public void TestFindWatershed()
        {
            string drainageCode = "01-02-03-04-05-06";

            Watershed watershed = new Watershed();
            watershed.DrainageCode = drainageCode;
            watershed.CreateAndFlush();

            Watershed dbWatershed = Watershed.Find(drainageCode);
            Assert.IsNotNull(dbWatershed);
            Assert.AreEqual(drainageCode, dbWatershed.DrainageCode);
        }

        [Test]
        public void TestProperties()
        {
            Watershed watershed = new Watershed();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("AreaHA", float.MaxValue);
            properties.Add("AreaPercent", float.MaxValue);
            properties.Add("BorderInd", "1");    
            properties.Add("DrainageCode", "01-02-03-04-05-06");
            properties.Add("DrainsInto", "Bay of Fundy");
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

            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.CreateAndFlush();

            Watershed watershed = new Watershed();
            watershed.DrainageCode = drainageCode;
            watershed.Place = place;
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
            watershed.DrainageCode = drainageCode;
            watershed.CreateAndFlush();

            WaterBody waterBody1 = new WaterBody();
            waterBody1.Id = id1;
            waterBody1.Watershed = watershed;
            waterBody1.CreateAndFlush();

            WaterBody waterBody2 = new WaterBody();
            waterBody2.Id = id2;
            waterBody2.Watershed = watershed;
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
            IList<Publication> publications = watershed.RelatedPublications;
            Assert.IsNotNull(publications);
            Assert.AreEqual(0, publications.Count);
        }

        [Test]
        public void TestRelatedInteractiveMapsWhenIsNotWithinBasin()
        {
            string query = "place name is the default query";
            Watershed watershed = new Watershed();
            watershed.Name = query;
            watershed.DrainageCode = "10-11-12-13-14-15";

            InteractiveMap[] interactiveMaps = new InteractiveMap[3];
            for (int i = 0; i < interactiveMaps.Length; i++)
            {
                interactiveMaps[i] = new InteractiveMap();
                interactiveMaps[i].Title = "Interactive Map where " + query + ": #" + i.ToString();
                interactiveMaps[i].IsBasinMap = (i % 2) == 0 ? true : false; // even to true, odd to false
                interactiveMaps[i].CreateAndFlush();
            }

            Assert.IsFalse(watershed.IsWithinBasin());
            Assert.AreEqual(interactiveMaps.Length, watershed.RelatedInteractiveMaps.Count);
        }

        [Test]
        public void TestRelatedInteractiveMapsWhenIsWithinBasin()
        {
            string query = "place name is the default query";
            Watershed watershed = new Watershed();
            watershed.Name = query;
            watershed.DrainageCode = "01-02-03-04-05-06";

            InteractiveMap[] interactiveMaps = new InteractiveMap[3];
            for (int i = 0; i < interactiveMaps.Length; i++)
            {
                interactiveMaps[i] = new InteractiveMap();
                interactiveMaps[i].Title = "Interactive Map where " + query + ": #" + i.ToString();
                interactiveMaps[i].IsBasinMap = (i % 2) == 0 ? true : false; // even to true, odd to false
                interactiveMaps[i].CreateAndFlush();
            }

            Assert.IsTrue(watershed.IsWithinBasin());
            Assert.AreEqual(2, watershed.RelatedInteractiveMaps.Count);
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
            WaterBody waterBody1 = new WaterBody();
            WaterBody waterBody2 = new WaterBody();
            waterBody1.DataSets = new List<DataSet>(new DataSet[] { new DataSet(), new DataSet() });
            waterBody2.DataSets = new List<DataSet>(new DataSet[] { new DataSet(), new DataSet(), new DataSet() });

            Watershed watershed = new Watershed();
            watershed.WaterBodies.Add(waterBody1);
            watershed.WaterBodies.Add(waterBody2);
            Assert.AreEqual(5, watershed.DataSets.Count);
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
        public void TestGetCoordinateReturnsNullWhenPlaceIsNull()
        {
            Watershed watershed = new Watershed();
            Assert.IsNull(watershed.Place);
            Assert.IsNull(watershed.GetCoordinate());
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
        public void TestFindByCgndbKey()
        {
            string cgndbKey = "ABCDE";
            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.Create();

            Watershed watershed = new Watershed();
            watershed.DrainageCode = "01-02-03-04-05-06";
            watershed.Place = place;
            watershed.Create();

            Flush();

            Watershed foundWatershed = Watershed.FindByCgndbKey(cgndbKey);
            Assert.AreEqual(watershed, foundWatershed);
            Assert.AreEqual(place, foundWatershed.Place);
        }

        [Test]
        public void TestFindByCgndbKeyWhenNoMatchingRecord()
        {
            Assert.IsNull(Watershed.FindByCgndbKey("ABCDE"));
        }

        [Test]
        public void TestExistsForCgndbKey()
        {
            string cgndbKey = "ABCDE";
            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.Create();

            Watershed watershed = new Watershed();
            watershed.DrainageCode = "01-02-03-04-05-06";
            watershed.Place = place;
            watershed.Create();

            Flush();

            Assert.IsTrue(Watershed.ExistsForCgndbKey(cgndbKey));
        }

        [Test]
        public void TestExistsForCgndbKeyWhenNoMatchingRecord()
        {
            Assert.IsFalse(Watershed.ExistsForCgndbKey("ABCDE"));
        }

        [Test]
        public void TestFindAllByQuery()
        {
            string[] unitNames = { "Saint", "Saint John", "Saint John River",
                "Hammond River", "Fredericton", "Moncton", "Miramichi River" };
            for (int i = 0; i < unitNames.Length; i++)
            {
                Watershed watershed = new Watershed();
                watershed.Name = unitNames[i];
                // XXX: this is technically not a valid Drainage Code but no validation is performed
                watershed.DrainageCode = i.ToString();
                watershed.Create();
            }

            Flush();

            Assert.AreEqual(3, Watershed.FindAllByQuery("Saint").Count, "Query for 'Saint'");
            Assert.AreEqual(3, Watershed.FindAllByQuery("saint").Count, "Query for 'saint'");
            Assert.AreEqual(1, Watershed.FindAllByQuery("Fredericton").Count, "Query for 'Fredericton'");
            Assert.AreEqual(2, Watershed.FindAllByQuery("M").Count, "Query for 'M' returned");
            Assert.AreEqual(0, Watershed.FindAllByQuery("John").Count, "Query for 'John'");
            Assert.AreEqual(0, Watershed.FindAllByQuery("River").Count, "Query for 'River'");
            Assert.AreEqual(3, Watershed.FindAllByQuery(" Saint").Count, "Query for ' Saint'");
            Assert.AreEqual(1, Watershed.FindAllByQuery("Hammond River ").Count, "Query for 'Hammond River '");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFindAllByQueryThrowsArgumentNullExceptionWhenQueryIsNull()
        {
            Watershed.FindAllByQuery(null);
        }	
    }
}
