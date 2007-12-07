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
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            component = new BasicInfoComponent();
        }

        [TearDown]
        public void Teardown()
        {
            CleanUp();
        }

        [Test]
        [ExpectedException(typeof(ViewComponentException),
            "The ResourceComponent requires a view component " +
            "parameter named 'place' which should contain an 'IPlace' instance")]
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

            IPlace place = mocks.CreateMock<IPlace>();
            component.Place = place;
            Expect.Call(place.Name).Return(name);
            Expect.Call(place.Region).Return(region);
            Expect.Call(place.NameStatus).Return(status);
            mocks.ReplayAll();

            PrepareViewComponent(component);
            component.Render();

            Assert.AreEqual(expectedTitle, component.Context.ContextVars["title"]);
            Assert.AreEqual(place, component.Context.ContextVars["place"]);
            Assert.IsFalse((bool)component.Context.ContextVars["is_a_watershed"]);
            Assert.IsFalse((bool)component.Context.ContextVars["is_a_waterbody"]);
            Assert.AreEqual("shared/basic_info", component.Context.ViewToRender);
            mocks.VerifyAll();
        }

        [Test]
        public void TestRenderWhenIsAWatershed()
        {
            Watershed place = new Watershed();
            component.Place = place;
            PrepareViewComponent(component);
            component.Render();
            Assert.IsTrue((bool)component.Context.ContextVars["is_a_watershed"]);
            Assert.IsFalse((bool)component.Context.ContextVars["is_a_waterbody"]);
        }

        [Test]
        public void TestRenderWhenIsAWaterBody()
        {
            WaterBody place = new WaterBody();
            component.Place = place;
            PrepareViewComponent(component);
            component.Render();
            Assert.IsTrue((bool)component.Context.ContextVars["is_a_waterbody"]);
            Assert.IsFalse((bool)component.Context.ContextVars["is_a_watershed"]);
        }	
    }
}
