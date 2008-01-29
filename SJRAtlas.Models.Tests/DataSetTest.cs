using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class DataSetTest : AbstractModelTestCase
    {
        private MockRepository mocks = new MockRepository();

        [Test]
        public void TestFind()
        {
            DataSet dataset = new DataSet();
            dataset.CreateAndFlush();
            Assert.AreEqual(dataset, DataSet.Find(dataset.Id));
        }

        [Test]
        public void TestGetMetadata()
        {
            DataSet dataset = new DataSet();
            dataset.CreateAndFlush();
            Metadata metadata = new Metadata();
            metadata.Owner = dataset;
            metadata.CreateAndFlush();

            Assert.AreEqual(metadata, dataset.GetMetadata());
        }

        [Test]
        public void TestPresentations()
        {
            Assert.Ignore();
        }

        [Test]
        public void TestPresentationsNeverReturnsNull()
        {
            DataSet dataset = new DataSet();
            Assert.IsNotNull(dataset.Presentations);
            Assert.AreEqual(0, dataset.Presentations.Count);
        }

        [Test]
        public void TestProperties()
        {
            DataSet dataset = new DataSet();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Id", 7);
            properties.Add("Abstract", "Test Abstract");
            properties.Add("Author", "Colin Casey");
            properties.Add("Title", "DataSet Title");
            properties.Add("Origin", "New Origin");
            properties.Add("Presentations", mocks.CreateMock<IList<Presentation>>());
            TestHelper.ErrorSummary summary = TestHelper.TestProperties(dataset, properties);
            Assert.IsEmpty(summary, "The following errors occured while testing DataSet properties:\n" + summary.GetSummary());
        }	
    }
}
