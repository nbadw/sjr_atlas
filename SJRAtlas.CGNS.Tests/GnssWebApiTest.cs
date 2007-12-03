namespace SJRAtlas.CGNS.Tests
{
    using System;
    using NUnit.Framework;
    using SJRAtlas.CGNS;
    using Rhino.Mocks;
    using System.IO;
    using System.Reflection;
    using System.Net;

    [TestFixture]
    public class GnssWebApiTest
    {
        private string RESULTS_XML = "SJRAtlas.CGNS.Tests.GnssSearchResults.xml";
        private MockRepository mocks;

        private Stream getResourceStream(string name)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        }

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
        }

        [TearDown]
        public void Teardown()
        {
            mocks.VerifyAll();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructWithNullThrowsNullArgumentException()
        {
            new GnssWebApi(new Uri(null));
        }

        [Test]
        [ExpectedException(typeof(UriFormatException))]
        public void TestConstructWithMalformedUrlStringThrowsUriFormatException()
        {
            new GnssWebApi("");
        }

        [Test]
        [ExpectedException(typeof(UriFormatException))]
        public void TestConstructWithRelativeUrlThrowsUriFormatException()
        {
            new GnssWebApi("/url/not/absolute");
        }	

        [Test]
        public void TestCreateRequestBuilderReturnsGnssWebApiRequestBuilder()
        {
            GnssWebApi gwa = new GnssWebApi("http://url.for.test/");
            Assert.IsInstanceOfType(typeof(GnssWebApiRequestBuilder), gwa.CreateRequestBuilder());
        }

        [Test]
        public void TestGetSearchResultsIsSuccessful()
        {
            Uri url = new Uri("mock://url.for.test/");
            WebRequest webRequest = mocks.CreateMock<WebRequest>();
            WebResponse webResponse = mocks.CreateMock<WebResponse>();
            GnssWebApi gwa = new GnssWebApi(url);
            
            Assert.IsTrue(WebRequest.RegisterPrefix("mock", new MockWebRequestCreator(webRequest)));

            Expect.Call(webRequest.GetResponse()).Return(webResponse);
            webRequest.Timeout = gwa.Timeout;
            LastCall.On(webRequest).Repeat.Once();
            Expect.Call(webResponse.GetResponseStream()).Return(getResourceStream(RESULTS_XML));

            mocks.ReplayAll();

            Assert.IsNotNull(gwa.FindByRequest(url));
        }

        private void SetupResponseStream(Uri url, GnssWebApi gwa)
        {
            WebRequest webRequest = mocks.CreateMock<WebRequest>();
            WebResponse webResponse = mocks.CreateMock<WebResponse>();

            Assert.IsTrue(WebRequest.RegisterPrefix("mock", new MockWebRequestCreator(webRequest)));

            Expect.Call(webRequest.GetResponse()).Return(webResponse);
            webRequest.Timeout = gwa.Timeout;
            LastCall.On(webRequest).Repeat.Once();
            Expect.Call(webResponse.GetResponseStream()).Return(getResourceStream(RESULTS_XML));
        }	
    }

    public class MockWebRequestCreator : IWebRequestCreate
    {
        private WebRequest mockedWebRequest;

        public MockWebRequestCreator(WebRequest mockedWebRequest)
        {
            this.mockedWebRequest = mockedWebRequest;
        }

        #region IWebRequestCreate Members

        public WebRequest Create(Uri uri)
        {
            return this.mockedWebRequest;
        }

        #endregion
    }
}
