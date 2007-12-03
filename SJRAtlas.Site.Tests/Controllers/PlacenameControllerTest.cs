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
    public class PlacenameControllerTest : BaseControllerTest
    {
        private MockRepository mocks;
        private PlaceNameController controller;
        private IPlaceNameLookup pnss;
        private IMetadataLookup mss;
        private IWatershedLookup wss;
        private IEasyMapLookup emf;
        private IAtlasUtils utils;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            pnss = mocks.CreateMock<IPlaceNameLookup>();
            mss = mocks.CreateMock<IMetadataLookup>();
            emf = mocks.CreateMock<IEasyMapLookup>();
            utils = mocks.CreateMock<IAtlasUtils>();
            controller = new PlaceNameController();
            controller.PlaceNameLookup = pnss;
            controller.WatershedLookup = wss;
            controller.MetadataLookup = mss;
            controller.EasyMapLookup = emf;
            controller.AtlasUtils = utils;
            PrepareController(controller, "placename", "");
        }

        [Test]
        public void TestViewPlaceName()
        {
            string id = "ABCDE";
            string name = "test";
            IPlaceName placename = mocks.CreateMock<IPlaceName>();

            Expect.Call(pnss.Find(id)).Return(placename);
            Expect.Call(placename.Name).Return(name);
            Expect.Call(mss.FindByQuery(name)).Return(new IMetadata[0]);
            Expect.Call(emf.FindAll()).Return(new IEasyMap[0]);
            Expect.Call(utils.IsWithinBasin(placename)).Return(false);
            mocks.ReplayAll();

            controller.View(id);

            mocks.VerifyAll();
            Assert.IsNotNull(controller.PropertyBag["placename"]);
            Assert.IsNotNull(controller.PropertyBag["reports"]);
            Assert.IsNotNull(controller.PropertyBag["easymaps"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewPlaceNameWithBadId()
        {
            string id = "ABCDE";

            Expect.Call(pnss.Find(id)).Return(null);
            mocks.ReplayAll();

            controller.View(id);
        }

        [Test]
        public void TestViewPlaceNameWhenWithinBasin()
        {
            string id = "ABCDE";
            string name = "test";
            IPlaceName placename = mocks.CreateMock<IPlaceName>();

            Expect.Call(pnss.Find(id)).Return(placename);
            Expect.Call(placename.Name).Return(name);
            Expect.Call(mss.FindByQuery(name)).Return(new IMetadata[0]);
            Expect.Call(emf.FindAll()).Return(new IEasyMap[0]);
            mocks.ReplayAll();

            controller.View(id);

            mocks.VerifyAll();
            Assert.IsNotNull(controller.PropertyBag["placename"]);
            Assert.IsNotNull(controller.PropertyBag["reports"]);
            Assert.IsNotNull(controller.PropertyBag["data"]);
            Assert.IsNotNull(controller.PropertyBag["easymaps"]);
        }

        [Test]
        public void TestPlaceNameLookupFails()
        {
            // test code goes here
            Assert.Fail();
        }	

        [Test]
        public void TestRedirectsToWaterBodyViewWhenPlaceNameIsWaterBody()
        {
            string id = "ABCDE";
            
            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(pnss.Find(id)).Return(placename);
            Expect.Call(placename.Id).Return(id);
            Expect.Call(utils.IsWithinBasin(placename)).Return(true);
            Expect.Call(utils.IsWaterBody(placename)).Return(true);
            
            mocks.ReplayAll();
            controller.View(id);
            mocks.VerifyAll();
            
            Assert.AreEqual("/waterbody/view.rails?placename=" + id + "&", Response.RedirectedTo);
        }

        [Test]
        public void TestRedirectsToWatershedViewWhenPlaceNameIsWatershed()
        {
            string id = "ABCDE";

            IPlaceName placename = mocks.CreateMock<IPlaceName>();
            Expect.Call(pnss.Find(id)).Return(placename);
            Expect.Call(placename.Id).Return(id);
            Expect.Call(utils.IsWithinBasin(placename)).Return(true);
            Expect.Call(utils.IsWaterBody(placename)).Return(false);
            Expect.Call(utils.IsWatershed(placename)).Return(true);

            mocks.ReplayAll();
            controller.View(id);
            mocks.VerifyAll();

            Assert.AreEqual("/watershed/view.rails?placename=" + id + "&", Response.RedirectedTo);        
        }
	
	
    }
}
