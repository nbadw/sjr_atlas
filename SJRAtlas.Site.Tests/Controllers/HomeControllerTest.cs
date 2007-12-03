namespace SJRAtlas.Site.Tests.Controllers
{
    using System;
    using Castle.MonoRail.TestSupport;
    using NUnit.Framework;
    using SJRAtlas.Site.Controllers;

    [TestFixture]
    public class HomeControllerTestCase : BaseControllerTest
    {
        private HomeController controller;

        [SetUp]
        public void Init()
        {
            controller = new HomeController();
            PrepareController(controller);
        }

        [Test]
        public void TestIndexAction()
        {
            controller.Index();

            //Assert.IsNotNull(controller.PropertyBag["AccessDate"]);
        }
    }
}
