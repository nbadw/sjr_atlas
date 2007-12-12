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
        public override void Setup()
        {
            base.Setup();
            mocks = new MockRepository();
        }

        [Test]
        public void FindWaterBody()
        {
            int id = 37;
            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
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
            properties.Add("AltName", "Another Name");
            properties.Add("ComplexId", 12345);
            properties.Add("FlowsIntoWaterBodyId", 12345);
            properties.Add("FlowsIntoWaterBodyName", "TestValue");
            properties.Add("Id", 37);
            properties.Add("Name", "Saint John River");
            properties.Add("SurveyedInd", "TestValue");
            properties.Add("Type", "TestValue");
            TestHelper.ErrorSummary errors = TestHelper.TestProperties(waterbody, properties);
            Assert.IsEmpty(errors, "The following errors occurred during property testing:\n" + errors.GetSummary());        
        }

        [Test]
        public void TestPlace()
        {
            int id = 37;
            string cgndbKey = "ABCDE";

            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.CreateAndFlush();

            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
            waterbody.Place = place;
            waterbody.CreateAndFlush();
            
            WaterBody dbWaterbody = WaterBody.Find(id);
            Assert.IsNotNull(dbWaterbody);
            Assert.IsNotNull(dbWaterbody.Place);
            Assert.AreEqual(cgndbKey, dbWaterbody.Place.CgndbKey);
        }

        [Test]
        public void TestAltPlace()
        {
            int id = 37;
            string cgndbKey = "ABCDE";

            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.CreateAndFlush();

            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
            waterbody.AltPlace = place;
            waterbody.CreateAndFlush();

            WaterBody dbWaterbody = WaterBody.Find(id);
            Assert.IsNotNull(dbWaterbody);
            Assert.IsNotNull(dbWaterbody.AltPlace);
            Assert.AreEqual(cgndbKey, dbWaterbody.AltPlace.CgndbKey);
        }

        [Test]
        public void TestBelongsToWatershed()
        {
            int id = 73;
            string drainageCode = "01-00-00-00-00-00";

            Watershed watershed = new Watershed();
            watershed.DrainageCode = drainageCode;
            watershed.CreateAndFlush();

            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
            waterbody.Watershed = watershed;
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
            IList<Publication> publications = waterbody.RelatedPublications;
            Assert.IsNotNull(publications);
            Assert.AreEqual(0, publications.Count);
        }

        [Test]
        public void TestRelatedInteractiveMaps()
        {
            string query = "place name is the default query";
            WaterBody waterbody = new WaterBody();
            waterbody.Name = query;

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
            waterbody.Place = new Place();
            waterbody.Place.Latitude = lat;
            waterbody.Place.Longitude = lon;
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
        public void TestRelatedDataSets()
        {
            int id = 50001;
            WaterBody waterbody = new WaterBody();
            waterbody.Id = id;
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

        [Test]
        public void TestExistsForCgndbKeyOrAltCgndbKeyWhenWaterbodyHasCgndbKey()
        {
            string cgndbKey = "ABCDE";
            
            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.Create();
            
            WaterBody waterbody = new WaterBody();
            waterbody.Place = place;
            waterbody.Create();

            Flush();

            Assert.IsTrue(WaterBody.ExistsForCgndbKeyOrAltCgndbKey(cgndbKey));
        }

        [Test]
        public void TestExistsForCgndbKeyOrAltCgndbKeyWhenWaterbodyHasAltCgndbKey()
        {
            string altCgndbKey = "ABCDE";

            Place place = new Place();
            place.CgndbKey = altCgndbKey;
            place.Create();

            WaterBody waterbody = new WaterBody();
            waterbody.AltPlace = place;
            waterbody.Create();

            Flush();

            Assert.IsTrue(WaterBody.ExistsForCgndbKeyOrAltCgndbKey(altCgndbKey));
        }

        [Test]
        public void TestExistsForCgndbKeyOrAltCgndbKeyWhenWaterbodyHasBothKeys()
        {
            string cgndbKey = "ABCDE";
            string altCgndbKey = "VWXYZ";

            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.Create();

            Place altPlace = new Place();
            altPlace.CgndbKey = altCgndbKey;
            altPlace.Create();

            WaterBody waterbody = new WaterBody();
            waterbody.Place = place;
            waterbody.AltPlace = altPlace;
            waterbody.Create();

            Flush();

            Assert.IsTrue(WaterBody.ExistsForCgndbKeyOrAltCgndbKey(cgndbKey));
            Assert.IsTrue(WaterBody.ExistsForCgndbKeyOrAltCgndbKey(altCgndbKey));
        }

        [Test]
        public void TestExistsForCgndbKeyOrAltCgndbKeyWhenNoMatchingWaterBody()
        {
            Assert.IsFalse(WaterBody.ExistsForCgndbKeyOrAltCgndbKey("ABCDE"));
        }

        [Test]
        public void TestFindByCgndbKeyOrAltCgndbKeyWhenWaterbodyHasCgndbKey()
        {
            string cgndbKey = "ABCDE";

            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.Create();

            WaterBody waterbody = new WaterBody();
            waterbody.Place = place;
            waterbody.Create();

            Flush();

            WaterBody foundWaterbody = WaterBody.FindByCgndbKeyOrAltCgndbKey(cgndbKey);
            Assert.AreEqual(waterbody, foundWaterbody);
            Assert.AreEqual(place, foundWaterbody.Place);
        }

        [Test]
        public void TestFindByCgndbKeyOrAltCgndbKeyWhenWaterbodyHasAltCgndbKey()
        {
            string altCgndbKey = "ABCDE";

            Place place = new Place();
            place.CgndbKey = altCgndbKey;
            place.Create();

            WaterBody waterbody = new WaterBody();
            waterbody.AltPlace = place;
            waterbody.Create();

            Flush();

            WaterBody foundWaterbody = WaterBody.FindByCgndbKeyOrAltCgndbKey(altCgndbKey);
            Assert.AreEqual(waterbody, foundWaterbody);
            Assert.AreEqual(place, foundWaterbody.AltPlace);
        }

        [Test]
        public void TestFindByCgndbKeyOrAltCgndbKeyWhenWaterbodyHasBothKeys()
        {
            string cgndbKey = "ABCDE";
            string altCgndbKey = "VWXYZ";

            Place place = new Place();
            place.CgndbKey = cgndbKey;
            place.Create();

            Place altPlace = new Place();
            altPlace.CgndbKey = altCgndbKey;
            altPlace.Create();

            WaterBody waterbody = new WaterBody();
            waterbody.Place = place;
            waterbody.AltPlace = altPlace;
            waterbody.Create();

            Flush();

            WaterBody foundWaterbody = WaterBody.FindByCgndbKeyOrAltCgndbKey(cgndbKey);
            Assert.AreEqual(waterbody, foundWaterbody);
            Assert.AreEqual(place, foundWaterbody.Place);
            Assert.AreEqual(altPlace, foundWaterbody.AltPlace);
        }

        [Test]
        public void TestFindByCgndbKeyOrAltCgndbKeyWhenNoMatchingWaterBody()
        {
            Assert.IsNull(WaterBody.FindByCgndbKeyOrAltCgndbKey("ABCDE"));
        }	
    }
}
