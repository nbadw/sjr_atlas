using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Castle.MonoRail.TestSupport;
using Castle.MonoRail.Framework;
using SJRAtlas.Site.Components;
using SJRAtlas.Models;

namespace SJRAtlas.Site.Tests.Components
{
    [TestFixture]
    public class BasicInfoComponentTest : BaseViewComponentTest
    {
        private BasicInfoComponent component;

        [SetUp]
        public void Setup()
        {
            component = new BasicInfoComponent();
        }

        [TearDown]
        public void Teardown()
        {
            CleanUp();
        }

        [Test]
        [ExpectedException(typeof(ViewComponentException), ExpectedMessage = "" +
            "The ResourceComponent requires a view component " +
            "parameter named 'place' which should contain a 'Place' instance")]
        public void ThrowsExceptionIfNoPlaceParameterWasSupplied()
        {
            component.Place = null;
            component.Initialize();
        }

        [Test]
        public void TestRender()
        {
            string name = "NAME";
            string region = "REGION";
            string status = "OFFICIAL";
            string expectedTitle = String.Format("{0}, {1} ({2} Name)", name, region, status);

            Place place = new Place();
            component.Place = place;
            place.Name = name;
            place.Region = region;
            place.NameStatus = status;

            PrepareViewComponent(component);
            component.Render();

            Assert.AreEqual(expectedTitle, component.Context.ContextVars["title"]);
            Assert.AreEqual(place, component.Context.ContextVars["place"]);
            Assert.AreEqual("shared/basic_info", component.Context.ViewToRender);
        }	
    }
}
