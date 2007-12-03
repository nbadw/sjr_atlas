using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SJRAtlas.Site.Components;
using SJRAtlas.Core;
using Rhino.Mocks;
using Castle.MonoRail.TestSupport;
using Castle.MonoRail.Framework;

namespace SJRAtlas.Site.Tests.Components
{
    [TestFixture]
    public class ResourceComponentTest : BaseViewComponentTest
    {
        private ResourceComponent component;
        private IMetadata metadata;
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
            metadata = mocks.DynamicMock<IMetadata>();
            component = new ResourceComponent();
        }

        [TearDown]
        public void Teardown()
        {
            mocks.VerifyAll();
            CleanUp();
        }

        private void PrepareResources(params string[] resources)
        {
            List<Uri> uris = new List<Uri>(resources.Length);
            foreach(string resource in resources)
            {
                Uri createdUri;
                if(Uri.TryCreate(resource, UriKind.RelativeOrAbsolute, out createdUri))
                    uris.Add(createdUri);
            }

            Expect.Call(metadata.Resources).Repeat.Any().Return(uris.ToArray());

            component.Metadata = metadata;
            PrepareViewComponent(component);
        }

        private void AssertHasSpatialData(bool value)
        {
            Assert.AreEqual(value, (bool)component.Context.ContextVars["has_spatial_data"]);
        }

        private void AssertHasTabularData(bool value)
        {
            Assert.AreEqual(value, (bool)component.Context.ContextVars["has_tabular_data"]);
        }

        private void AssertHasSummaryReport(bool value)
        {
            Assert.AreEqual(value, (bool)component.Context.ContextVars["has_summary_report"]);
        }

        private void AssertHasGraph(bool value)
        {
            Assert.AreEqual(value, (bool)component.Context.ContextVars["has_graph"]);
        }

        [Test]
        [ExpectedException(typeof(ViewComponentException), 
            "The ResourceComponent requires a view component " +
            "parameter named 'metadata' which should contain 'IMetadata' instance")]
        public void ThrowsExceptionIfNoMetadataWasSupplied()
        {
            mocks.ReplayAll();
            component.Metadata = null;
            component.Initialize();
        }

        [Test]
        public void TestSpatialDataResourceMatch()
        {
            PrepareResources("http://gis.mektekdev.com/demo/Default.aspx?resource=Map_Resource");
            mocks.ReplayAll();
            component.Render();
            AssertHasSpatialData(true);
            AssertHasTabularData(false);
            AssertHasSummaryReport(false);
            AssertHasGraph(false);
        }

        [Test]
        public void TestTabularDataResourceMatch()
        {
            PrepareResources("/resource/tabular.rails?id=1");
            mocks.ReplayAll();
            component.Render();
            AssertHasTabularData(true);
        }

        [Test]
        public void TestSummaryReportResourceMatch()
        {
            PrepareResources("/resource/publications/report/static-resource-title.pdf");
            mocks.ReplayAll();
            component.Render();
            AssertHasSummaryReport(true);
        }

        [Test]
        public void TestGraphResourceMatch()
        {
            PrepareResources("/resource/graph/1");
            mocks.ReplayAll();
            component.Render();
            AssertHasGraph(true);
        }
    }
}
