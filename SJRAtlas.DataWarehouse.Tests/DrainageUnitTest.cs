using System;
using NUnit.Framework;
using Castle.ActiveRecord.Framework;

namespace SJRAtlas.DataWarehouse.Tests
{
    [TestFixture]
    public class DrainageUnitTest : AbstractModelTestCase
    {
        [Test]
        public void TestCreateDrainageUnit()
        {
            DrainageUnit drainageUnit = new DrainageUnit();
            drainageUnit.CreateAndFlush();
            Assert.AreEqual(1, DrainageUnit.FindAll().Length);
        }

        [Test]
        public void TestReadDrainageUnit()
        {
            DrainageUnit drainageUnit = new DrainageUnit();
            drainageUnit.CreateAndFlush();
            Assert.AreEqual(drainageUnit.DrainageCode, DrainageUnit.FindFirst().DrainageCode);
        }

        [Test]
        [ExpectedException(typeof(ActiveRecordException))]
        public void TestUpdateDrainageUnit()
        {
            DrainageUnit drainageUnit = new DrainageUnit();
            drainageUnit.CreateAndFlush();

            drainageUnit.DrainageCode = "99-00-00-00-00-00";
            drainageUnit.UpdateAndFlush();
        }

        [Test]
        [ExpectedException(typeof(ActiveRecordException))]
        public void TestDeleteDrainageUnit()
        {
            DrainageUnit drainageUnit = new DrainageUnit();
            drainageUnit.CreateAndFlush();
            Assert.AreEqual(1, DrainageUnit.FindAll().Length);
            drainageUnit.DeleteAndFlush();
        }

        [Test]
        public void TestDrainageCodeIsAlsoId()
        {
            string id = "01-02-03-04-05-06";
            DrainageUnit drainageUnit = new DrainageUnit();
            drainageUnit.DrainageCode = id;
            Assert.AreEqual(id, drainageUnit.Id);
        }

        [Test]
        public void TestUnitNameIsAlsoName()
        {
            string name = "test";
            DrainageUnit drainageUnit = new DrainageUnit();
            drainageUnit.UnitName = name;
            Assert.AreEqual(name, drainageUnit.Name);
        }

        [Test]
        public void TestTributaryOf()
        {
            DrainageUnit drainageUnit = new DrainageUnit();

            drainageUnit.Level1No = "01";
            drainageUnit.Level1Name = "LEVEL1";
            Assert.AreEqual("", drainageUnit.TributaryOf);

            drainageUnit.Level2No = "02";
            drainageUnit.Level2Name = "LEVEL2";
            Assert.AreEqual("LEVEL1", drainageUnit.TributaryOf);

            drainageUnit.Level3No = "03";
            drainageUnit.Level3Name = "LEVEL3";
            Assert.AreEqual("LEVEL1 - LEVEL2", drainageUnit.TributaryOf);

            drainageUnit.Level4No = "04";
            drainageUnit.Level4Name = "LEVEL4";
            Assert.AreEqual("LEVEL1 - LEVEL2 - LEVEL3", drainageUnit.TributaryOf);

            drainageUnit.Level5No = "05";
            drainageUnit.Level5Name = "LEVEL5";
            Assert.AreEqual("LEVEL1 - LEVEL2 - LEVEL3 - LEVEL4", drainageUnit.TributaryOf);

            drainageUnit.Level6No = "06";
            drainageUnit.Level6Name = "LEVEL6";
            Assert.AreEqual("LEVEL1 - LEVEL2 - LEVEL3 - LEVEL4 - LEVEL5", drainageUnit.TributaryOf);
        }
	
        [Test]
        public void TestFindAllByUnitName()
        {
            string query = "match";
            DrainageUnit drainageUnit;

            drainageUnit = new DrainageUnit();
            drainageUnit.DrainageCode = "01-00-00-00-00-00";
            drainageUnit.UnitName = "will not be found";
            drainageUnit.CreateAndFlush();

            drainageUnit = new DrainageUnit();
            drainageUnit.DrainageCode = "02-00-00-00-00-00";
            drainageUnit.UnitName = "match keyword at beginning";
            drainageUnit.CreateAndFlush();

            drainageUnit = new DrainageUnit();
            drainageUnit.DrainageCode = "03-00-00-00-00-00";
            drainageUnit.UnitName = "keyword at end match";
            drainageUnit.CreateAndFlush();

            drainageUnit = new DrainageUnit();
            drainageUnit.DrainageCode = "04-00-00-00-00-00";
            drainageUnit.UnitName = "keyword match in middle";
            drainageUnit.CreateAndFlush();

            Assert.AreEqual(4, DrainageUnit.FindAll().Length);

            Assert.AreEqual(3, DrainageUnit.FindAllByUnitNameSearch(query).Length);
        }

        [Test]
        public void TestFindAllByUnitNameIsNotCaseSensitive()
        {
            string query = "match";
            DrainageUnit drainageUnit;

            drainageUnit = new DrainageUnit();
            drainageUnit.DrainageCode = "01-00-00-00-00-00";
            drainageUnit.UnitName = "Match";
            drainageUnit.CreateAndFlush();

            drainageUnit = new DrainageUnit();
            drainageUnit.DrainageCode = "02-00-00-00-00-00";
            drainageUnit.UnitName = "match";
            drainageUnit.CreateAndFlush();

            Assert.AreEqual(2, DrainageUnit.FindAll().Length);

            Assert.AreEqual(2, DrainageUnit.FindAllByUnitNameSearch(query).Length);
        }

        [Test]
        public void TestFindByDrainageCode()
        {
            DrainageUnit excludedDrainageUnit = new DrainageUnit();
            excludedDrainageUnit.DrainageCode = "00-00-00-00-00-00";
            excludedDrainageUnit.CreateAndFlush();

            int[] n = { 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 6; i++)
            {
                n[i] = 1;
                DrainageUnit drainageUnit = new DrainageUnit();
                drainageUnit.DrainageCode = String.Format("0{0}-0{1}-0{2}-0{3}-0{4}-0{5}",
                                                          n[0], n[1], n[2], n[3], n[4], n[5]); 
                drainageUnit.CreateAndFlush();
            }
            
            Assert.AreEqual(7, DrainageUnit.FindAll().Length);
            Assert.AreEqual(6, DrainageUnit.FindAllByDrainageCode("01*").Length);
            Assert.AreEqual(5, DrainageUnit.FindAllByDrainageCode("01-01*").Length);
            Assert.AreEqual(4, DrainageUnit.FindAllByDrainageCode("01-01-01*").Length);
            Assert.AreEqual(3, DrainageUnit.FindAllByDrainageCode("01-01-01-01*").Length);
            Assert.AreEqual(2, DrainageUnit.FindAllByDrainageCode("01-01-01-01-01*").Length);
            Assert.AreEqual(1, DrainageUnit.FindAllByDrainageCode("01-01-01-01-01-01").Length);
        }	
    }
}
