namespace SJRAtlas.Site.Tests.Controllers
{
    using System;
    using Castle.MonoRail.TestSupport;
    using NUnit.Framework;
    using SJRAtlas.Site.Controllers;
    using Rhino.Mocks;

    [TestFixture]
    public class MetadataControllerTest : BaseControllerTest
    {
        //private MockRepository mocks;
        //private MetaDataController controller;
        //private IMetadataLookup metaLookup;

        //[SetUp]
        //public void Setup()
        //{
        //    mocks = new MockRepository();
        //    metaLookup = mocks.CreateMock<IMetadataLookup>();
        //    controller = new MetaDataController();
        //    controller.MetadataLookup = metaLookup;
        //    PrepareController(controller, "metadata", "");
        //}

        //[TearDown]
        //public void Teardown()
        //{
        //    mocks.VerifyAll();
        //}

        //[Test]
        //public void TestViewMetadataDefaultsToHTML()
        //{
        //    int id = 1;
        //    IMetadata metadata = mocks.CreateMock<IMetadata>();
        //    Expect.Call(metaLookup.Find(id)).Return(metadata);
        //    mocks.ReplayAll();

        //    controller.View(id, null);
        //    Assert.AreEqual("text/html", controller.Response.ContentType);
        //}

        //[Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void TestViewMetadataWithBadId()
        //{
        //    int id = 1;
        //    IMetadata metadata = mocks.CreateMock<IMetadata>();
        //    Expect.Call(metaLookup.Find(id)).Return(null);
        //    mocks.ReplayAll();

        //    controller.View(id, "html");
        //}

        //[Test]
        //public void TestViewMetadataAsHTML()
        //{
        //    int id = 1;
        //    string format = "html";
        //    IMetadata metadata = mocks.CreateMock<IMetadata>();
        //    Expect.Call(metaLookup.Find(id)).Return(metadata);
        //    mocks.ReplayAll();

        //    controller.View(id, format);
        //    Assert.AreEqual("text/html", controller.Response.ContentType);
        //    Assert.IsNotNull(controller.PropertyBag["metadata"]);
        //}	

        //[Test]
        //public void TestViewMetadataAsXML()
        //{
        //    //int id = 1;
        //    //string format = "xml";
        //    //IMetadata metadata = mocks.CreateMock<IMetadata>();
        //    //Expect.Call(metaLookup.Find(id)).Return(metadata);
        //    //metadata.ToXml(controller.Response.OutputStream);
        //    //LastCall.On(metadata).Repeat.Once();
        //    //mocks.ReplayAll();

        //    //controller.View(id, format);
        //    //Assert.AreEqual("text/xml", controller.Response.ContentType);
        //    Assert.Fail();
        //}

        //[Test]
        //public void TestViewMetadataAsPDF()
        //{
        //    //int id = 1;
        //    //string format = "pdf";
        //    //IMetadata metadata = mocks.CreateMock<IMetadata>();
        //    //Expect.Call(metaLookup.Find(id)).Return(metadata);
        //    //metadata.ToPdf(controller.Response.OutputStream);
        //    //LastCall.On(metadata).Repeat.Once();
        //    //mocks.ReplayAll();

        //    //controller.View(id, format);
        //    //Assert.AreEqual("application/pdf", controller.Response.ContentType);
        //    Assert.Fail();
        //}
	
    }
}
