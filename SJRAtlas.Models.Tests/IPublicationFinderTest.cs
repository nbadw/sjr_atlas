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
            map.Abstract = "Map Abstract";
            map.Author = "Colin Casey";
            map.File = @"c:\path\to\some\map\file";
            map.CreateAndFlush();

            PublishedReport report = new PublishedReport();
            report.Id = 2;
            report.Title = "Test Report";
            report.Abstract = "Report Abstract";
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
        public void TestFindAllByQuery()
        {            
            string query = "Test%";
            Assert.AreEqual(2, finder.FindAllByQuery(query).Length);
        }

        [Test]
        public void TestFindAllMapsByQuery()
        {
            string query = "Test Map";
            PublishedMap[] maps = finder.FindAllByQuery<PublishedMap>(query);
            Assert.AreEqual(1, maps.Length);
            Assert.AreEqual(query, maps[0].Title);
        }

        [Test]
        public void TestFindAllReportByQuery()
        {
            string query = "Test Report";
            PublishedReport[] reports = finder.FindAllByQuery<PublishedReport>(query);
            Assert.AreEqual(1, reports.Length);
            Assert.AreEqual(query, reports[0].Title);
        }
	

        [Test]
        public void TestFindByQueryMatchesNothing()
        {
            string query = "NOT A TITLE";
            IPublication[] publications = finder.FindAllByQuery(query);
            Assert.AreEqual(0, publications.Length);
        }
    }
}
