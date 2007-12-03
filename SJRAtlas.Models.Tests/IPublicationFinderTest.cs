using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SJRAtlas.Models.Finders;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class IPublicationFinderTest : AbstractModelTestCase
    {
        private IPublicationFinder finder;

        [SetUp]
        public void Setup()
        {
            base.Init();
            finder = new IPublicationFinder();

            PublishedMap.DeleteAll();
            PublishedReport.DeleteAll();
            
            PublishedMap map = new PublishedMap();
            map.Id = 1;
            map.Title = "Test Map";
            map.Abstract = "This map object will be used for testing only.";
            map.Author = "Colin Casey";
            map.File = @"c:\path\to\some\map\file";
            map.CreateAndFlush();

            PublishedReport report = new PublishedReport();
            report.Id = 2;
            report.Title = "Test Report";
            report.Abstract = "Like the map, this report will be used for testing only.";
            report.Author = "Colin Casey";
            report.File = @"c:\path\to\a\report";
            report.CreateAndFlush();
        }

        [TearDown]
        public void Teardown()
        {
            PublishedMap.DeleteAll();
            PublishedReport.DeleteAll();
            base.Terminate();
        }

        [Test]
        public void TestFindById()
        {
            Assert.IsNotNull(finder.Find(1));
            Assert.IsNotNull(finder.Find(2));
        }

        [Test]
        public void TestFindByIdWhereIdDoesNotExist()
        {
            Assert.IsNull(finder.Find(1000));
        }

        [Test]
        public void TestFindByDefaultQuery()
        {            
            string query = "%map%";
            Assert.AreEqual(2, finder.FindByDefaultQuery(query).Length);
        }

        [Test]
        public void TestFindMapByQuery()
        {
            string expectedTitle = "Test Map";
            string query = "from Publication p where p.Title = ?";
            IPublication[] publications = finder.FindByQuery(query, expectedTitle);
            Assert.AreEqual(1, publications.Length);
            Assert.AreEqual(expectedTitle, publications[0].Title);
        }

        [Test]
        public void TestFindReportByQuery()
        {
            string expectedTitle = "Test Report";
            string query = "from Publication p where p.Title = ?";
            IPublication[] publications = finder.FindByQuery(query, expectedTitle);
            Assert.AreEqual(1, publications.Length);
            Assert.AreEqual(expectedTitle, publications[0].Title);
        }

        [Test]
        public void TestFindByQueryMatchesNothing()
        {
            string expectedTitle = "NOT A TITLE";
            string query = "from Publication p where p.Title = ?";
            IPublication[] publications = finder.FindByQuery(query, expectedTitle);
            Assert.AreEqual(0, publications.Length);
        }
    }
}
