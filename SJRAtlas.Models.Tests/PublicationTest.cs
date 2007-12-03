using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class PublicationTest
    {
        private Publication publication;

        [SetUp]
        public void Setup()
        {
            publication = new Publication();
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void TestBasicProperties()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties["Id"] = 1000;
            properties["Title"] = "test title";
            properties["Abstract"] = "just a simple abstract";
            properties["Author"] = "Colin";
            properties["File"] = "c:/path/to/file";
            properties["CreatedAt"] = new DateTime(2007, 1, 1);
            properties["UpdatedAt"] = new DateTime(2007, 1, 2);
            TestHelper.ErrorSummary summary = TestHelper.TestProperties(publication, properties);
            Assert.AreEqual(0, summary.Count, summary.GetSummary());
        }

        [Test]
        public void TestGetId()
        {
            Assert.AreEqual(0, publication.Id);
            Assert.AreEqual(0, publication.GetId());

            int id = 12345;
            publication.Id = id;
            Assert.AreEqual(id, publication.GetId());
        }
    }
}
