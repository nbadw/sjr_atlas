using System;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections;

namespace SJRAtlas.Models.Tests
{
    [TestFixture]
    public class AtlasTest
    {
        private MockRepository mocks;
        private AtlasRepository atlas;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            atlas = new AtlasRepository();
        }

        [Test]
        public void TestFind()
        {
            int id = 0;
            MockEntity entity = new MockEntity();
            IEntityFinder<MockEntity> finder = mocks.CreateMock<IEntityFinder<MockEntity>>();
            IDictionary finders = mocks.CreateMock<IDictionary>();
            atlas.Finders = finders;

            Expect.Call(finder.Find(id)).Return(entity);
            Expect.Call(finders[typeof(MockEntity)]).Return(finder);

            mocks.ReplayAll();

            Assert.AreEqual(entity, atlas.Find<MockEntity>(id));

            mocks.VerifyAll();
        }

        [Test]
        public void TestFindersGetterAndSetter()
        {
            IDictionary finders = mocks.CreateMock<IDictionary>();
            TestHelper.ErrorSummary errors = TestHelper.TestProperty(atlas, "Finders", finders);
            Assert.IsEmpty(errors, errors.GetSummary());
        }

        [Test]
        public void TestFinderNotFoundReturnsNull()
        {
            atlas.Finders = mocks.CreateMock<IDictionary>();
            Assert.IsNull(atlas.Find<IEntity>(0));
        }

        //[Test]
        //public void TestFindByDefaultQuery()
        //{
        //    object param = "test parameter";
        //    MockEntity entity = new MockEntity();
        //    IEntityFinder<MockEntity> finder = mocks.CreateMock<IEntityFinder<MockEntity>>();
        //    IDictionary finders = mocks.CreateMock<IDictionary>();
        //    atlas.Finders = finders;

        //    Expect.Call(finders[typeof(MockEntity)]).Return(finder);
        //    Expect.Call(finder.FindByDefaultQuery(param)).Return(new MockEntity[] { entity });

        //    mocks.ReplayAll();
            
        //    MockEntity[] results = atlas.FindByDefaultQuery<MockEntity>(param);
        //    Assert.AreEqual(1, results.Length);
        //    Assert.AreEqual(entity, results[0]);

        //    mocks.VerifyAll();
        //}

        //[Test]
        //public void TestFindByDefaultQueryWhenNoFinderFound()
        //{
        //    atlas.Finders = mocks.CreateMock<IDictionary>();
        //    Assert.AreEqual(0, atlas.FindByDefaultQuery<IEntity>("test value").Length);
        //}

        //[Test]
        //public void TestFindByQuery()
        //{
        //    string query = "this could be sql or hql query";
        //    object paramOne = 1;
        //    object paramTwo = "another parameter";
        //    IEntity entity = mocks.CreateMock<IEntity>();
        //    IEntityFinder<IEntity> finder = mocks.CreateMock<IEntityFinder<IEntity>>();
        //    IDictionary finders = mocks.CreateMock<IDictionary>();
        //    atlas.Finders = finders;

        //    Expect.Call(finders[typeof(IEntity)]).Return(finder);
        //    Expect.Call(finder.FindByQuery(query, paramOne, paramTwo)).Return(new IEntity[] { entity });

        //    mocks.ReplayAll();

        //    IEntity[] results = atlas.FindByQuery<IEntity>(query, paramOne, paramTwo);
        //    Assert.AreEqual(1, results.Length);
        //    Assert.AreEqual(entity, results[0]);

        //    mocks.VerifyAll();
        //}

        //[Test]
        //public void TestFindByQueryWhenNoFinderFound()
        //{
        //    atlas.Finders = mocks.CreateMock<IDictionary>();
        //    Assert.AreEqual(0, atlas.FindByQuery<IEntity>("id = ?", 7).Length);
        //}
    }

    public class MockEntity : IEntity
    {
        #region IEntity Members

        public object GetId()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
