using NUnit.Framework;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using SJRAtlas.Models.Finders;
using Rhino.Mocks;

namespace SJRAtlas.Models.Tests
{
    /// <summary>
    /// This is a suggestion of base class that might 
    /// simplify the unit testing for the ActiveRecord
    /// classes.
    /// <para>
    /// Basically you have to create a separate database
    /// for your tests, which is always a good idea:
    /// - production
    /// - development
    /// - test
    /// </para>
    /// <para>
    /// You have to decide if you want to administrate the
    /// schema on the <c>test</c> database or let ActiveRecord
    /// generate them for you during test execution. Check 
    /// <see cref="AbstractModelTestCase.PrepareSchema"/>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Note that this class enables lazy classes and collections
    /// by using a <see cref="SessionScope"/>.
    /// This have side effects. Some of your test must 
    /// invoke <see cref="Flush"/> 
    /// to persist the changes.
    /// </remarks>
    public abstract class AbstractModelTestCase
    {
        protected SessionScope scope;
        private static bool initialized = false;

        [TestFixtureSetUp]
        public virtual void FixtureInit()
        {
            if (!initialized)
            {
                InitFramework();
                log4net.Config.XmlConfigurator.Configure();
                initialized = true;
            }
        }

        [SetUp]
        public virtual void Init()
        {
            PrepareSchema();
            CreateScope();
        }

        [TearDown]
        public virtual void Terminate()
        {
            DisposeScope();
            DropSchema();
        }

        [TestFixtureTearDown]
        public virtual void TerminateAll()
        {
        }

        protected void Flush()
        {
            SessionScope.Current.Flush();
        }

        protected void CreateScope()
        {
            scope = new SessionScope(FlushAction.Never);
        }

        protected void DisposeScope()
        {
            scope.Dispose();
        }

        /// <summary>
        /// If you want to delete everything from the model.
        /// Remember to do it in a descendent dependency order
        /// </summary>
        protected virtual void PrepareSchema()
        {
            // If you want to delete everything from the model.
            // Remember to do it in a descendent dependency order

            // Office.DeleteAll();
            // User.DeleteAll();

            // Another approach is to always recreate the schema 
            // (please use a separate test database if you want to do that)
            ActiveRecordStarter.CreateSchema();
        }

        protected virtual void DropSchema()
        {
            ActiveRecordStarter.DropSchema();
        }

        protected virtual void InitFramework()
        {
            IConfigurationSource source = ActiveRecordSectionHandler.Instance;
            // Remember to add the types, for example
            // ActiveRecordStarter.Initialize( source, typeof(Blog), typeof(Post) );
            // Or to use the assembly that holds the ActiveRecord types
            ActiveRecordStarter.Initialize(System.Reflection.Assembly.Load("SJRAtlas.Models"), source);
        }

        public void TestRelatedPublications(MockRepository mocks, IPlace place, IAtlasRepository repository)
        {
            IPublicationFinder finder = mocks.CreateMock<IPublicationFinder>();
            Expect.Call(repository.GetFinder<IPublicationFinder>()).Return(finder);
            Expect.Call(finder.FindAllByQuery(null)).IgnoreArguments().Return(new IPublication[3]);
            mocks.ReplayAll();
            IPublication[] publications = place.RelatedPublications;
            Assert.AreEqual(3, publications.Length);
            mocks.VerifyAll();
        }

        public void TestRelatedPublicationsNeverReturnsNull(MockRepository mocks, IPlace place, IAtlasRepository repository)
        {
            IPublicationFinder finder = mocks.CreateMock<IPublicationFinder>();
            Expect.Call(repository.GetFinder<IPublicationFinder>()).Return(finder);
            Expect.Call(finder.FindAllByQuery(null)).IgnoreArguments().Return(null);
            mocks.ReplayAll();
            IPublication[] publications = place.RelatedPublications;
            Assert.IsNotNull(publications);
            Assert.IsEmpty(publications);
            mocks.VerifyAll();
        }

        public void TestRelatedInteractiveMaps(MockRepository mocks, IPlace place, IAtlasRepository repository)
        {
            InteractiveMapFinder finder = mocks.CreateMock<InteractiveMapFinder>();
            Expect.Call(repository.GetFinder<InteractiveMapFinder>()).Return(finder);
            Expect.Call(finder.FindAllByQuery(null)).IgnoreArguments().Return(new InteractiveMap[3]);
            mocks.ReplayAll();
            InteractiveMap[] interactiveMaps = place.RelatedInteractiveMaps;
            Assert.AreEqual(3, interactiveMaps.Length);
            mocks.VerifyAll();
        }

        public void TestRelatedInteractiveMapsNeverReturnsNull(MockRepository mocks, IPlace place, IAtlasRepository repository)
        {
            InteractiveMapFinder finder = mocks.CreateMock<InteractiveMapFinder>();
            Expect.Call(repository.GetFinder<InteractiveMapFinder>()).Return(finder);
            Expect.Call(finder.FindAllByQuery(null)).IgnoreArguments().Return(null);
            mocks.ReplayAll();
            InteractiveMap[] interactiveMaps = place.RelatedInteractiveMaps;
            Assert.IsNotNull(interactiveMaps);
            Assert.IsEmpty(interactiveMaps);
            mocks.VerifyAll();
        }
    }
}