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
    public class PlaceInfoComponentTest : BaseViewComponentTest
    {
        private PlaceInfoComponent component;

        [SetUp]
        public void Setup()
        {
            component = new PlaceInfoComponent();
        }

        [TearDown]
        public void Teardown()
        {
            CleanUp();
        }

        [Test]
        [ExpectedException(typeof(ViewComponentException),
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
            string genericTerm = "Generic Term";
            string county = "Test County";
            double latitude = 65.0;
            double longitude = -65.0;

            Place place = new Place();
            component.Place = place;
            place.GenericTerm = genericTerm;
            place.County = county;
            place.Latitude = latitude;
            place.Longitude = longitude;

            PrepareViewComponent(component);
            component.Render();

            Assert.AreEqual(genericTerm, component.Context.ContextVars["type"]);
            Assert.AreEqual(county, component.Context.ContextVars["county"]);
            Assert.AreEqual(latitude, component.Context.ContextVars["latitude"]);
            Assert.AreEqual(longitude, component.Context.ContextVars["longitude"]);
            Assert.AreEqual("place/info", component.Context.ViewToRender);
        }
    }
}
