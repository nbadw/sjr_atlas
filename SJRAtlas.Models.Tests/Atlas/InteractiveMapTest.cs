using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Models.Tests.Atlas
{
    [TestFixture]
    public class InteractiveMapTest : AbstractModelTestCase
    {
        [Test]
        public void TestFind()
        {
            string title = "Test Map";
            InteractiveMap map = new InteractiveMap();
            map.Title = title;
            map.CreateAndFlush();

            InteractiveMap dbMap = InteractiveMap.Find(map.Id);
            Assert.IsNotNull(dbMap);
            Assert.AreEqual(title, dbMap.Title);
        }

        [Test]
        [ExpectedException(typeof(Castle.ActiveRecord.NotFoundException))]
        public void TestFindWhenIdDoesNotExist()
        {
            InteractiveMap map = InteractiveMap.Find(12345);
        }

        [Test]
        public void TestFindAllByQuery()
        {
            Assert.Ignore();

            string query = "%Test Query%";
            InteractiveMap[] maps = new InteractiveMap[5];
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i] = new InteractiveMap();
                maps[i].Title = query + " will match this interactive map: #" + i.ToString();
                maps[i].CreateAndFlush();
            }

            IList<InteractiveMap> mapList = InteractiveMap.FindAllByQuery(query);
            Assert.IsNotNull(mapList);
            Assert.AreEqual(maps.Length, mapList.Count);
        }

        [Test]
        public void TestFindAllByQueryNeverReturnsNull()
        {
            Assert.Ignore();

            IList<InteractiveMap> mapList = InteractiveMap.FindAllByQuery(String.Empty);
            Assert.IsNotNull(mapList);
            Assert.AreEqual(0, mapList.Count);
        }	

        [Test]
        public void TestGetMetadata()
        {
            InteractiveMap map = new InteractiveMap();
            map.CreateAndFlush();

            Metadata metadata = new Metadata();
            metadata.Owner = map;
            metadata.CreateAndFlush();

            InteractiveMap dbMap = InteractiveMap.Find(map.Id);
            Assert.AreEqual(metadata, map.GetMetadata());
        }

        [Test]
        public void TestProperties()
        {
            InteractiveMap map = new InteractiveMap();
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("Title", "New Map Title");
            properties.Add("Id", 3);
            properties.Add("IsBasinMap", true);
            properties.Add("ServiceName", "new service");
            properties.Add("ThumbnailUrl", "http://location.to.thumbnail");
            properties.Add("LargeThumbnailUrl", "http://location.to.larger.thumbnail");
            properties.Add("CreatedAt", DateTime.Now);
            properties.Add("UpdatedAt", DateTime.Now);
            TestHelper.ErrorSummary summary = TestHelper.TestProperties(map, properties);
            Assert.IsEmpty(summary, "The following errors occured while testing InteractiveMap properties:\n" + summary.GetSummary());
        }

        [Test]
        public void TestFindAllBasinMaps()
        {
            InteractiveMap[] testMaps = new InteractiveMap[7];
            for (int i = 0; i < testMaps.Length; i++)
            {
                testMaps[i] = new InteractiveMap();
                testMaps[i].Create();
            }
            testMaps[1].IsBasinMap = true;
            testMaps[3].IsBasinMap = true;
            testMaps[5].IsBasinMap = true;
            Flush();

            Assert.AreEqual(3, InteractiveMap.FindAllBasinMaps().Count);
        }

        [Test]
        public void TestFindByTitle()
        {
            string testTitle = "title that matches exactly";

            InteractiveMap matchingMap = new InteractiveMap();
            matchingMap.Title = testTitle;
            matchingMap.Create();

            InteractiveMap almostExact = new InteractiveMap();
            almostExact.Title = testTitle + " but not quite";
            almostExact.Create();

            InteractiveMap notEvenClose = new InteractiveMap();
            notEvenClose.Title = "not even close to the title";
            notEvenClose.Create();

            Flush();

            Assert.AreEqual(matchingMap, InteractiveMap.FindByTitle(testTitle));
        }

        [Test]
        public void TestFindByTitleWhenNoMatchingTitle()
        {
            string testTitle = "title that does not exist";
            Assert.IsNull(InteractiveMap.FindByTitle(testTitle));
        }

        [Test]
        public void TestFindByTitleWhenTwoTitlesMatch()
        {
            string testTitle = "title that matches exactly";

            InteractiveMap firstMatchingMap = new InteractiveMap();
            firstMatchingMap.Title = testTitle;
            firstMatchingMap.Create();

            InteractiveMap secondMatchingMap = new InteractiveMap();
            secondMatchingMap.Title = testTitle;
            secondMatchingMap.Create();

            Flush();

            InteractiveMap returnedMap = InteractiveMap.FindByTitle(testTitle);
            Assert.AreEqual(firstMatchingMap, returnedMap);
            Assert.AreNotEqual(secondMatchingMap, returnedMap);
        }	
    }
}
