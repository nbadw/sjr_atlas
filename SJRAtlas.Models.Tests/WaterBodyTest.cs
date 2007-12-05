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
        private WaterBody waterbody;
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            base.Init();
            mocks = new MockRepository();
        }

        [Test]
        public void FindWaterBody()
        {
            // test code goes here
            Assert.Fail();
        }
	
        [Test]
        public void TestProperties()
        {
            WaterBody waterbody = new WaterBody();
            waterbody.Place = new Place();
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
            properties.Add("Created", DateTime.Now);
            properties.Add("Datum", "NAD83");
            properties.Add("FeatureId", "TestValue");
            properties.Add("FlowsIntoWaterBodyId", 12345);
            properties.Add("FlowsIntoWaterBodyName", "TestValue");
            properties.Add("GenericTerm", "TestValue");
            properties.Add("Id", 37);
            properties.Add("Name", "Saint John River");
            properties.Add("NameStatus", "Official");
            properties.Add("Modified", DateTime.Now);
            properties.Add("Latitude", 3.7);
            properties.Add("Longitude", 7.3);
            properties.Add("NtsMap", "TestValue");
            properties.Add("Region", "NB");
            properties.Add("Repository", mocks.CreateMock<IAtlasRepository>());
            properties.Add("SurveryInd", "TestValue");
            properties.Add("Type", "TestValue");
            TestHelper.ErrorSummary errors = TestHelper.TestProperties(waterbody, properties);
            Assert.IsEmpty(errors, "The following errors occurred during property testing:\n" + errors.GetSummary());        
        }

        [Test]
        public void TestBelongsToPlace()
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
        public void FindAllByIdOrNameOrAbbreviation()
        {
            // test code goes here
            Assert.Fail();
        }	

    }
}
