namespace SJRAtlas.Site.Tests.Controllers
{
    using System;
    using Castle.MonoRail.TestSupport;
    using NUnit.Framework;
    using Rhino.Mocks;
    using System.Collections.Generic;
    using SJRAtlas.Site.Controllers;
    using SJRAtlas.Core;

    [TestFixture]
    public class SearchControllerTest : BaseControllerTest
    {
        private MockRepository mocks;
        private SearchController controller;
        private IPlaceNameLookup placeNameLookup;
        private IMetadataLookup metadataLookup;
        private IWatershedLookup watershedLookup;
        private IEasyMapLookup easymapLookup;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            placeNameLookup = mocks.CreateMock<IPlaceNameLookup>();
            metadataLookup = mocks.CreateMock<IMetadataLookup>();
            watershedLookup = mocks.CreateMock<IWatershedLookup>();
            easymapLookup = mocks.CreateMock<IEasyMapLookup>();
            controller = new SearchController();
            controller.PlaceNameLookup = placeNameLookup;
            controller.WatershedLookup = watershedLookup;
            controller.MetadataLookup = metadataLookup;
            controller.EasyMapLookup = easymapLookup;
            PrepareController(controller, "search", "");
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void TestQuickSearchWithNoTriggers()
        {
            string query = "test";
            controller.Quick(query);
            Assert.AreEqual("/search/gazetteer.rails?q=" + query + "&", Response.RedirectedTo);
        }	

        [Test]
        public void TestQuickSearchWithWatershedKeywordTriggers()
        {
            string[] triggers = { "watershed", "basin", "catchment" };
            string queryWithoutTrigger = "query for";

            foreach (string trigger in triggers)
            {
                string queryWithTrigger = queryWithoutTrigger + " " + trigger;
                controller.Quick(queryWithTrigger);
                Assert.AreEqual("/search/watershed.rails?q=" + queryWithoutTrigger + "&", Response.RedirectedTo, queryWithTrigger + " should have triggered watershed search");
            }
        }

        [Test]
        public void TestQuickSearchWithMetadataKeywordTrigger()
        {
            string queryWithoutTrigger = "query for";
            string queryWithTrigger = queryWithoutTrigger + " metadata";
            controller.Quick(queryWithTrigger);
            Assert.AreEqual("/search/metadata.rails?q=" + queryWithoutTrigger + "&", Response.RedirectedTo);
        }

        # region Gazetteer Search Tests

        [Test]
        public void TestGazetteerQuickSearch()
        {
            string placename = "saint john";
            controller.Quick(placename);
            Assert.AreEqual("/search/gazetteer.rails?q=" + placename + "&", Response.RedirectedTo);
        }

        [Test]
        public void TestGazetteerSearchReturnsNoResultsAndRedirectsToMetadataSearch()
        {
            string query = "test"; 
            IPlaceName[] results = new IPlaceName[0];

            Expect.Call(placeNameLookup.FindByQuery(query)).Return(results);
            mocks.ReplayAll();

            controller.Gazetteer(query);

            mocks.VerifyAll();
            Assert.AreEqual("/search/metadata.rails?q=" + query + "&", Response.RedirectedTo); 
        }

        [Test]
        public void TestGazetteerSearchReturnsMultipleResults()
        {
            string query = "test";
            IPlaceName[] results = new IPlaceName[2];

            Expect.Call(placeNameLookup.FindByQuery(query)).Return(results);
            mocks.ReplayAll();

            controller.Gazetteer(query);

            mocks.VerifyAll();
            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.IsNotNull(controller.PropertyBag["results"]);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestGazetteerSearchServiceIsDown()
        {
            string query = "test";
            Exception exception = new Exception("Service Unavailable");

            Expect.Call(placeNameLookup.FindByQuery(query)).Throw(exception);
            mocks.ReplayAll();

            controller.Gazetteer(query);          
        }	

        [Test]
        public void TestGazetteerSearchReturnsOneResultAndRedirectsToViewPlaceName()
        {
            string query = "test";
            string id = "ABCDE";

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            IPlaceName[] results = new IPlaceName[] { placename };

            Expect.Call(placeNameLookup.FindByQuery(query)).Return(results);
            Expect.Call(placename.Id).Return(id);
            mocks.ReplayAll();

            controller.Gazetteer(query);

            mocks.VerifyAll();
            Assert.AreEqual("/placename/view.rails?id=" + id + "&", Response.RedirectedTo);
        }
        # endregion

        # region Watershed Search Tests

        [Test]
        public void TestWatershedSearchReturnsMultipleResults()
        {
            string query = "test";
            IWatershed[] results = new IWatershed[2];

            Expect.Call(watershedLookup.FindAllByProperty("UnitName", query)).Return(results);
            mocks.ReplayAll();

            controller.Watershed(query);

            mocks.VerifyAll();
            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.IsNotNull(controller.PropertyBag["watersheds"]);
        }

        [Test]
        public void TestWatershedSearchReturnsOneResultAndRedirectsToViewWatershed()
        {
            string query = "test";
            string id = "00-00-00-00-00-00";
            IWatershed watershed = mocks.CreateMock<IWatershed>();
            IWatershed[] results = new IWatershed[] { watershed };

            Expect.Call(watershedLookup.FindAllByProperty("UnitName", query)).Return(results);
            Expect.Call(watershed.Id).Return(id);
            mocks.ReplayAll();

            controller.Watershed(query);

            mocks.VerifyAll();
            Assert.AreEqual("/watershed/view.rails?id=" + id + "&", Response.RedirectedTo);
        }

        [Test]
        public void TestWatershedSearchReturnsNoResults()   
        {
            string query = "watershed";
            IWatershed[] results = new IWatershed[0];

            Expect.Call(watershedLookup.FindAllByProperty("UnitName", query)).Return(results);
            mocks.ReplayAll();

            controller.Watershed(query);

            mocks.VerifyAll();
            Assert.AreEqual(results, controller.PropertyBag["watersheds"]);
            Assert.AreEqual(query, controller.PropertyBag["query"]);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestWatershedSearchServiceIsDown()
        {
            string query = "test";
            Exception exception = new Exception("Service Unavailable");
                        
            Expect.Call(watershedLookup.FindAllByProperty("UnitName", query)).Throw(exception);
            mocks.ReplayAll();

            controller.Watershed(query);   
        }
	
        # endregion

        # region Metadata Search Tests

        [Test]
        public void TestMetadataSearchReturnsOneOrMoreResults()
        {
            string query = "test";
            IMetadata[] results = new IMetadata[1];

            Expect.Call(metadataLookup.FindByQuery(query)).Return(results);
            mocks.ReplayAll();

            controller.Metadata(query);

            mocks.VerifyAll();
            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.IsNotNull(controller.PropertyBag["results"]);
        }

        [Test]
        public void TestMetadataSearchReturnsNoResults()
        {
            string query = "test";
            IMetadata[] results = new IMetadata[0];

            Expect.Call(metadataLookup.FindByQuery(query)).Return(results);
            mocks.ReplayAll();

            controller.Metadata(query);

            mocks.VerifyAll();
            Assert.AreEqual(query, controller.PropertyBag["query"]);
            Assert.IsNotNull(controller.PropertyBag["results"]);
            Assert.AreEqual(0, ((IMetadata[])controller.PropertyBag["results"]).Length);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestMetadataSearchServiceIsDown()
        {
            string query = "test";
            Exception exception = new Exception("Service Unavailable");

            Expect.Call(metadataLookup.FindByQuery(query)).Throw(exception);
            mocks.ReplayAll();

            controller.Metadata(query); 
        }	

        # endregion 
    }
}
