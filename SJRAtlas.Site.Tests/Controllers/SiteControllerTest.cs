using System;
using Castle.MonoRail.TestSupport;
using NUnit.Framework;
using SJRAtlas.Site.Controllers;
using Rhino.Mocks;
using SJRAtlas.Models;
using System.Collections.Generic;

namespace SJRAtlas.Site.Tests.Controllers
{    
    [TestFixture]
    public class SiteControllerTest : BaseControllerTest
    {
        private MockRepository mocks = new MockRepository();
        
        [Test]
        public void TestIndexAction()
        {
            AtlasMediator mediator = mocks.CreateMock<AtlasMediator>();
            IList<InteractiveMap> mapResult = new List<InteractiveMap>();

            Expect.Call(mediator.FindInteractiveMapByTitle("")).IgnoreArguments().
                Repeat.Times(6).Return(new InteractiveMap());
            mocks.ReplayAll();

            SiteController controller = new SiteController();
            controller.AtlasMediator = mediator;
            PrepareController(controller, "site", "index");
            controller.Index();
            
            Assert.IsNotNull(controller.PropertyBag["interactive_maps"]);
            Assert.AreEqual(@"site\index", controller.SelectedViewName);
            mocks.VerifyAll();
        }

        [Test]
        public void TestAbout()
        {
            SiteController controller = new SiteController();
            PrepareController(controller, "site", "about");
            controller.About();
            Assert.AreEqual(@"site\about", controller.SelectedViewName);
        }

        [Test]
        public void TestForms()
        {
            SiteController controller = new SiteController();
            PrepareController(controller, "site", "forms");
            controller.Forms();
            Assert.AreEqual(@"site\forms", controller.SelectedViewName);
        }

        [Test]
        public void TestReports()
        {
            SiteController controller = new SiteController();
            PrepareController(controller, "site", "reports");
            controller.Reports();
            Assert.AreEqual(@"site\reports", controller.SelectedViewName);
        }

        [Test]
        public void TestMaps()
        {
            SiteController controller = new SiteController();
            PrepareController(controller, "site", "maps");
            controller.Maps();
            Assert.AreEqual(@"site\maps", controller.SelectedViewName);
        }
    }
}
