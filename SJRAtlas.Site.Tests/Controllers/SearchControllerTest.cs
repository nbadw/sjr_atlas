namespace SJRAtlas.Site.Tests.Controllers
{
    using System;
    using Castle.MonoRail.TestSupport;
    using NUnit.Framework;
    using Rhino.Mocks;
    using System.Collections.Generic;
    using SJRAtlas.Site.Controllers;
    using SJRAtlas.Models;
    using SJRAtlas.Models.Atlas;
    using SJRAtlas.Models.DataWarehouse;

    [TestFixture]
    public class SearchControllerTest : BaseControllerTest
    {
        private MockRepository mocks = new MockRepository();

        [Test]
        public void TestQuickSearchWithNoTriggers()
        {
            SearchController controller = new SearchController();
            PrepareController(controller, "search", "quick");
            string query = "test";
            controller.Quick(query);
            Assert.AreEqual("/search/places.rails?q=" + query + "&", Response.RedirectedTo);
        }

        [Test]
        public void TestQuickSearchWithWatershedKeywordTriggers()
        {
            SearchController controller = new SearchController();
            PrepareController(controller, "search", "quick");
            string[] triggers = { "watershed", "basin", "catchment" };
            string queryWithoutTrigger = "query for";

            foreach (string trigger in triggers)
            {
                string queryWithTrigger = queryWithoutTrigger + " " + trigger;
                controller.Quick(queryWithTrigger);
                Assert.AreEqual("/search/watersheds.rails?q=" + queryWithoutTrigger + "&", Response.RedirectedTo, queryWithTrigger + " should have triggered watershed search");
            }
        }

        [Test]
        public void TestAdvanced()
        {
            Assert.Ignore();
        }

        [Test]
        public void TestTips()
        {
            SearchController controller = new SearchController();
            PrepareController(controller, "search", "tips");
            controller.Tips();
            Assert.AreEqual(@"search\tips", controller.SelectedViewName);
        }

        [Test]
        public void TestNoResults()
        {
            string query = "test";
            SearchController controller = new SearchController();
            PrepareController(controller, "search", "tips");
            controller.NoResults(query);
            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.AreEqual(@"search\no-results", controller.SelectedViewName);
        }	

        [Test]
        public void TestSubmitAdvanced()
        {
            Assert.Ignore();
        }	

        # region Place Search Tests

        [Test]
        public void TestPlaceSearchRedirectsToWatershedSearchWhenNoResultsFound()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            IList<Place> results = mocks.CreateMock<IList<Place>>();

            Expect.Call(mediator.FindAllPlacesByQuery(query)).Return(results);
            Expect.Call(results.Count).Return(0);
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "places");
            controller.Places(query);

            Assert.AreEqual("/search/watersheds.rails?q=" + query + "&", Response.RedirectedTo);
            mocks.VerifyAll();
        }

        [Test]
        public void TestPlaceSearchWhenMultipleResultsFound()
        {
            Assert.Ignore();
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            List<Place> results = new List<Place>();
            results.Add(new Place());
            results.Add(new Place());

            Expect.Call(mediator.FindAllPlacesByQuery(query)).Return(results);
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "places");
            controller.Places(query);

            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.IsNotNull(controller.PropertyBag["results"]);
            Assert.AreEqual(@"search\places", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestPlaceSearchDisplaysRescueWhenAnExceptionOccurs()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();

            Expect.Call(mediator.FindAllPlacesByQuery(query)).Throw(new Exception());
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "places");
            controller.Places(query);

            Assert.AreEqual(@"rescue\generalerror", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        [Test]
        public void TestPlaceSearchRedirectsToPlaceViewWhenOnlyOneResultFound()
        {
            string query = "test";
            string id = "ABCDE";

            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            IList<Place> results = mocks.CreateMock<IList<Place>>();
            Place place = new Place();
            place.CgndbKey = id;

            Expect.Call(mediator.FindAllPlacesByQuery(query)).Return(results);
            Expect.Call(results.Count).Repeat.Twice().Return(1);
            Expect.Call(results[0]).Return(place);
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "places");
            controller.Places(query);

            Assert.AreEqual("/place/view.rails?cgndbKey=" + id + "&", Response.RedirectedTo);
            mocks.VerifyAll();
        }

        # endregion

        # region Watershed Search Tests

        [Test]
        public void TestWatershedsSearchWhenMultipleResultsFound()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            IList<Watershed> results = mocks.CreateMock<IList<Watershed>>();

            Expect.Call(mediator.FindAllWatershedsByQuery(query)).Return(results);
            Expect.Call(results.Count).Repeat.Twice().Return(2);
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "watersheds");
            controller.Watersheds(query);

            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.AreEqual(results, controller.PropertyBag["results"]);
            Assert.AreEqual(@"search\watersheds", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        [Test]
        public void TestWatershedsSearchRedirectsToWatershedViewWhenOnlyOneResultFound()
        {
            string query = "test";
            string drainageCode = "01-02-03-04-05-06";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            IList<Watershed> results = mocks.CreateMock<IList<Watershed>>();
            Watershed watershed = new Watershed();
            watershed.DrainageCode = drainageCode;

            Expect.Call(mediator.FindAllWatershedsByQuery(query)).Return(results);
            Expect.Call(results.Count).Return(1);
            Expect.Call(results[0]).Return(watershed);
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "watersheds");
            controller.Watersheds(query);

            Assert.AreEqual("/watershed/view.rails?drainageCode=" + drainageCode + "&", Response.RedirectedTo);
            mocks.VerifyAll();
        }

        [Test]
        public void TestWatershedsSearchRedirectsToAllSearchWhenNoResultsFound()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            IList<Watershed> results = mocks.CreateMock<IList<Watershed>>();

            Expect.Call(mediator.FindAllWatershedsByQuery(query)).Return(results);
            Expect.Call(results.Count).Repeat.Twice().Return(0);
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "watersheds");
            controller.Watersheds(query);

            Assert.AreEqual("/search/all.rails?q=" + query + "&", Response.RedirectedTo);
            mocks.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestWatershedsSearchDisplaysRescueWhenAnExceptionOccurs()
        {
            string query = "test"; 
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();

            Expect.Call(mediator.FindAllWatershedsByQuery(query)).Throw(new Exception());
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "watersheds");
            controller.Watersheds(query);

            Assert.AreEqual(@"rescue\generalerror", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        # endregion

        #region All Search Test

        [Test]
        public void TestAllSearchWhenDataSetResultsFound()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            List<DataSet> results = new List<DataSet>();
            results.Add(new DataSet());

            Expect.Call(mediator.FindAllDataSetsByQuery(query)).Return(results);
            Expect.Call(mediator.FindAllInteractiveMapsByQuery(query)).Return(new List<InteractiveMap>());
            Expect.Call(mediator.FindAllPublicationsByQuery(query)).Return(new List<Publication>());
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "all");
            controller.All(query);

            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.IsNotNull(controller.PropertyBag["datasets"]);
            Assert.IsNotNull(controller.PropertyBag["interactive_maps"]);
            Assert.IsNotNull(controller.PropertyBag["published_maps"]);
            Assert.IsNotNull(controller.PropertyBag["published_reports"]);
            Assert.AreEqual(@"search\all", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        [Test]
        public void TestAllSearchWhenInteractiveMapResultsFound()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            List<InteractiveMap> results = new List<InteractiveMap>();
            results.Add(new InteractiveMap());

            Expect.Call(mediator.FindAllDataSetsByQuery(query)).Return(new List<DataSet>());
            Expect.Call(mediator.FindAllInteractiveMapsByQuery(query)).Return(results);
            Expect.Call(mediator.FindAllPublicationsByQuery(query)).Return(new List<Publication>());
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "all");
            controller.All(query);

            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.IsNotNull(controller.PropertyBag["datasets"]);
            Assert.IsNotNull(controller.PropertyBag["interactive_maps"]);
            Assert.IsNotNull(controller.PropertyBag["published_maps"]);
            Assert.IsNotNull(controller.PropertyBag["published_reports"]);
            Assert.AreEqual(@"search\all", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        [Test]
        public void TestAllSearchWhenPublicationResultsFound()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            List<Publication> results = new List<Publication>();
            results.Add(new Publication());

            Expect.Call(mediator.FindAllDataSetsByQuery(query)).Return(new List<DataSet>());
            Expect.Call(mediator.FindAllInteractiveMapsByQuery(query)).Return(new List<InteractiveMap>());
            Expect.Call(mediator.FindAllPublicationsByQuery(query)).Return(results);
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "all");
            controller.All(query);

            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.IsNotNull(controller.PropertyBag["datasets"]);
            Assert.IsNotNull(controller.PropertyBag["interactive_maps"]);
            Assert.IsNotNull(controller.PropertyBag["published_maps"]);
            Assert.IsNotNull(controller.PropertyBag["published_reports"]);
            Assert.AreEqual(@"search\all", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        [Test]
        public void TestAllSearchRendersNoResultsPageWhenNoResultsFound()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();

            Expect.Call(mediator.FindAllDataSetsByQuery(query)).Return(new List<DataSet>());
            Expect.Call(mediator.FindAllInteractiveMapsByQuery(query)).Return(new List<InteractiveMap>());
            Expect.Call(mediator.FindAllPublicationsByQuery(query)).Return(new List<Publication>());
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "all");
            controller.All(query);

            Assert.AreEqual("/search/noresults.rails?q=" + query + "&", Response.RedirectedTo);
            mocks.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestAllSearchDisplaysRescueWhenAnExceptionOccurs()
        {
            string query = "test";
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();

            Expect.Call(mediator.FindAllDataSetsByQuery(query)).Throw(new Exception());
            Expect.Call(mediator.FindAllInteractiveMapsByQuery(query)).Throw(new Exception());
            Expect.Call(mediator.FindAllPublicationsByQuery(query)).Throw(new Exception());
            mocks.ReplayAll();

            SearchController controller = new SearchController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "search", "all");
            controller.All(query);

            Assert.AreEqual(@"rescue\generalerror", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        #endregion
    }
}
