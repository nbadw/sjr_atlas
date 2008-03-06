using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Models.Tests.Atlas
{
    [TestFixture]
    public class PublicationTest : AbstractModelTestCase
    {
        protected Publication publication;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            publication = CreatePublication();
        }

        protected virtual Publication CreatePublication()
        {
            return new Publication();
        }

        [Test]
        public void TestProperties()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties["Id"] = 1000;
            properties["Title"] = "test title";
            properties["Abstract"] = "just a simple abstract";
            properties["Origin"] = "Colin";
            properties["Uri"] = "c:/path/to/file";
            properties["MimeType"] = "application/pdf";
            properties["CreatedAt"] = new DateTime(2007, 1, 1);
            properties["UpdatedAt"] = new DateTime(2007, 1, 2);
            TestHelper.ErrorSummary summary = TestHelper.TestProperties(publication, properties);
            Assert.AreEqual(0, summary.Count, summary.GetSummary());
        }

        [Test]
        public void TestGetMetadata()
        {
            Publication publication = CreatePublication();
            publication.CreateAndFlush();
            Metadata metadata = new Metadata();
            metadata.Owner = publication;
            metadata.CreateAndFlush();

            Publication dbPublication = Publication.Find(publication.Id);
            Assert.AreEqual(metadata, dbPublication.GetMetadata());
        }	
    }
}
