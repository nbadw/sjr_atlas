namespace SJRAtlas.Site.Tests.Controllers
{
    using System;
    using Castle.MonoRail.TestSupport;
    using NUnit.Framework;
    using SJRAtlas.Site.Controllers;

    [TestFixture]
    public class AboutControllerTestCase : BaseControllerTest
    {
        private AboutController controller;

        [SetUp]
        public void Init()
        {
            controller = new AboutController();
            PrepareController(controller);
        }

        [Test]
        public void TestIndex()
        {
            controller.Index();
        }
    }
}
