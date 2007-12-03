namespace SJRAtlas.CGNS.Tests
{
    using System;
    using NUnit.Framework;
    using SJRAtlas.CGNS;

    [TestFixture]
    public class GnssWebApiRequestBuilderTest
    {
        private GnssWebApiRequestBuilder builder;
        private Uri baseUrl;

        [SetUp]
        public void Setup()
        {
            baseUrl = new Uri("http://test.base.url/api");
            builder = new GnssWebApiRequestBuilder(baseUrl);
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void TestBuiltUrlIsEscaped()
        {
            builder.ParseQuery("saint john");
            Uri url = builder.Build();
            Assert.IsFalse(url.ToString().Contains(" "), "Unescaped URL: " + url.ToString());
        }

        [Test]
        public void TestBuiltUrlContainsBaseUrl()
        {
            Uri url = builder.Build();
            Assert.IsTrue(url.ToString().StartsWith(baseUrl.ToString()), url.ToString() + " does not start with base URL " + baseUrl.ToString());
        }
	
	
    }
}
