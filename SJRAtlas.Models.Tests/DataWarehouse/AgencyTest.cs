using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SJRAtlas.Models.DataWarehouse;

namespace SJRAtlas.Models.Tests.DataWarehouse
{
    [TestFixture]
    public class AgencyTest : AbstractModelTestCase
    {
        [Test]
        public void TestProperties()
        {
            Agency agency = new Agency();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("AgencyCode", "Agency01");
            properties.Add("Name", "Test Agency");
            properties.Add("Type", "Test");
            properties.Add("DataRulesInd", "Some Value");
            TestHelper.ErrorSummary errors = TestHelper.TestProperties(agency, properties);
            Assert.IsEmpty(errors, errors.GetSummary());
        }

        [Test]
        public void TestFindAllByDrainageCode()
        {
            Assert.Ignore();
        }

        [Test]
        public void TestFindAllByWaterBodyId()
        {
            Assert.Ignore();
        }

        [Test]
        public void TestFindAllByAquaticSiteId()
        {
            Assert.Ignore();
        }	
    }
}
