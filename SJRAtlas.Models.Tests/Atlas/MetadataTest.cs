using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Models.Tests.Atlas
{
    [TestFixture]
    public class MetadataTest : AbstractModelTestCase
    {
        private MockRepository mocks = new MockRepository();

        [Test]
        public void TestFind()
        {
            Metadata metadata = new Metadata();            
            metadata.CreateAndFlush();

            Metadata dbMetadata = Metadata.Find(metadata.Id);
            Assert.AreEqual(metadata, dbMetadata);
        }

        [Test]
        [ExpectedException(typeof(Castle.ActiveRecord.NotFoundException))]
        public void TestFindWhenMetadataDoesNotExist()
        {
            Metadata dbMetadata = Metadata.Find(37);
        }

        [Test]
        public void TestFindByOwner()
        {
            Publication publication = new Publication();
            publication.CreateAndFlush();

            Metadata metadata = new Metadata();
            metadata.Owner = publication;
            metadata.CreateAndFlush();

            Metadata dbMetadata = Metadata.FindByOwner(publication);
            Assert.IsNotNull(dbMetadata);
            Assert.AreEqual(metadata, dbMetadata);
        }

        [Test]
        public void TestFindByOwnerWhenOwnerDoesNotExist()
        {
            Publication publication = new Publication();
            publication.CreateAndFlush();

            Metadata metadata = Metadata.FindByOwner(publication);
            Assert.IsNull(metadata);
        }	

        [Test]
        public void TestBelongsToOwner()
        {
            Publication publication = new Publication();
            publication.CreateAndFlush();

            Metadata metadata = new Metadata();
            metadata.Owner = publication;
            metadata.CreateAndFlush();

            Metadata dbMetadata = Metadata.Find(metadata.Id);
            Assert.IsNotNull(dbMetadata);
            Assert.AreEqual(publication, dbMetadata.Owner);
        }	

        [Test]
        public void TestProperties()
        {
            Metadata metadata = new Metadata();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Id", 12345);
            properties.Add("Content", "<metadata>some xml content</metadata>");
            properties.Add("Filename", "c:/path/to/metadata/file.xml");
            properties.Add("Owner", mocks.CreateMock<IMetadataAware>());
            TestHelper.ErrorSummary summary = TestHelper.TestProperties(metadata, properties);
            Assert.IsEmpty(summary, "The following errors occured while testing Metadata properties:\n" + summary.GetSummary());
        }	
    }
}
