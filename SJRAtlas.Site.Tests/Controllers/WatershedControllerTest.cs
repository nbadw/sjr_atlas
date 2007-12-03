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
    public class WatershedControllerTest : BaseControllerTest
    {
        private MockRepository mocks;
        private WatershedController controller;
        private IPlaceNameLookup pnss;
        private IMetadataLookup mss;
        private IWatershedLookup wss;
        private IEasyMapLookup emf;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            pnss = mocks.CreateMock<IPlaceNameLookup>();
            mss = mocks.CreateMock<IMetadataLookup>();
            wss = mocks.CreateMock<IWatershedLookup>();
            emf = mocks.CreateMock<IEasyMapLookup>();
            controller = new WatershedController();
            controller.WatershedLookup = wss;
            controller.MetadataLookup = mss;
            controller.EasyMapLookup = emf;
            PrepareController(controller, "watershed", "");
        }

        [Test]
        public void TestViewWatershed()
        {
            string id = "00-00-00-00-00-00";
            string name = "test";
            IWatershed watershed = mocks.CreateMock<IWatershed>();

            Expect.Call(wss.Find(id)).Return(watershed);
            Expect.Call(watershed.Name).Return(name);
            Expect.Call(mss.FindByQuery(name)).Return(new IMetadata[0]);
            Expect.Call(emf.FindAll()).Return(new IEasyMap[0]);
            mocks.ReplayAll();

            controller.View(id);

            mocks.VerifyAll();
            Assert.IsNotNull(controller.PropertyBag["watershed"]);
            Assert.IsNotNull(controller.PropertyBag["reports"]);
            Assert.IsNotNull(controller.PropertyBag["data"]);
            Assert.IsNotNull(controller.PropertyBag["easymaps"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestViewWatershedWithBadId()
        {
            string id = "00-00-00-00-00-00";

            Expect.Call(wss.Find(id)).Return(null);
            mocks.ReplayAll();

            controller.View(id);
        }
    }
}
