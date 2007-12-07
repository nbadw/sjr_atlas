using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SJRAtlas.Models.Tests
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
            TestHelper.ErrorSummary summary = TestHelper.TestProperties(map, properties);
            Assert.IsEmpty(summary, "The following errors occured while testing InteractiveMap properties:\n" + summary.GetSummary());
        }	
    }
}
