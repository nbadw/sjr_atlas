using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using SJRAtlas.CGNS;
using SJRAtlas.Core;
using System.Net;

namespace SJRAtlas.CGNS.Tests
{
    [TestFixture]
    public class GeographicalNameSearchServiceTest
    {
        private MockRepository mocks; 

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
        public void TestServiceConstructedWithNullUrl()
        {
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(null);
        }

        [Test]
        [ExpectedException(typeof(WebException))]
        public void TestFindByQueryThrowsWebException()
        {
            string query = "test";
            Uri builtUrl = new Uri("http://url.for.test/");
            Exception gnssException = new WebException();
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>();
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            builder.ParseQuery(query);
            LastCall.On(builder).Repeat.Once();
            Expect.Call(builder.Build()).Return(builtUrl);
            Expect.Call(gwa.FindByRequest(builtUrl)).Throw(gnssException);

            mocks.ReplayAll();

            gnss.FindByQuery(query);
        }

        [Test]
        public void TestFindByQueryReturnsResults()
        {
            string query = "test";
            Uri builtUrl = new Uri("http://url.for.test/");
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>();
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            builder.ParseQuery(query);
            LastCall.On(builder).Repeat.Once();
            Expect.Call(builder.Build()).Return(builtUrl);
            Expect.Call(gwa.FindByRequest(builtUrl)).Return(new GnssPlaceName[0]);

            mocks.ReplayAll();

            Assert.IsInstanceOfType(typeof(IPlaceName[]), gnss.FindByQuery(query));
        }

        [Test]
        public void TestFindByQueryDoesNotReturnNull()
        {
            string query = "test";
            Uri builtUrl = new Uri("http://url.for.test/");
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>();
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            builder.ParseQuery(query);
            LastCall.On(builder).Repeat.Once();
            Expect.Call(builder.Build()).Return(builtUrl);
            Expect.Call(gwa.FindByRequest(builtUrl)).Return(null);

            mocks.ReplayAll();

            Assert.IsNotNull(gnss.FindByQuery(query));
        }

        [Test]
        [ExpectedException(typeof(WebException))]
        public void TestFindByIdThrowsWebException()
        {
            string id = "ABCDE";
            Uri builtUrl = new Uri("http://url.for.test/");
            Exception gnssException = new WebException();
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>();
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            builder.AddCgndbKey(id);
            LastCall.On(builder).Repeat.Once();
            Expect.Call(builder.Build()).Return(builtUrl);
            Expect.Call(gwa.FindByRequest(builtUrl)).Throw(gnssException);

            mocks.ReplayAll();

            gnss.Find(id);
        }

        [Test]
        public void TestFindByIdReturnsNull()
        {
            string id = "ABCDE";
            Uri builtUrl = new Uri("http://url.for.test/");
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>();
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            builder.AddCgndbKey(id);
            LastCall.On(builder).Repeat.Once();
            Expect.Call(builder.Build()).Return(builtUrl);
            Expect.Call(gwa.FindByRequest(builtUrl)).Return(null);

            mocks.ReplayAll();

            Assert.IsNull(gnss.Find(id));
        }

        [Test]
        public void TestFindByIdReturnsResult()
        {
            string id = "ABCDE";
            Uri builtUrl = new Uri("http://url.for.test/");
            GnssPlaceName placename = mocks.CreateMock<GnssPlaceName>();
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>();
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            builder.AddCgndbKey(id);
            LastCall.On(builder).Repeat.Once();
            Expect.Call(builder.Build()).Return(builtUrl);
            Expect.Call(gwa.FindByRequest(builtUrl)).Return(new GnssPlaceName[] { placename });

            mocks.ReplayAll();
                        
            Assert.IsInstanceOfType(typeof(IPlaceName), gnss.Find(id));
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestFindAllNotSupported()
        {
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);
            mocks.ReplayAll();
            gnss.FindAll();
        }

        [Test]
        public void TestFindAllByProperty()
        {
            string property = "CgndbKey";
            string value = "ABCDE";
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>(); 
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            builder.AddCgndbKey(value);
            LastCall.On(builder).Repeat.Once();
            Expect.Call(builder.Build()).Return(null);
            Expect.Call(gwa.FindByRequest(null)).Return(new GnssPlaceName[0]);

            mocks.ReplayAll();
            gnss.FindAllByProperty(property, value);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestFindAllByNonExistantProperty()
        {
            string property = "NonExistantProperty";
            string value = "test";
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>(); 
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            mocks.ReplayAll();

            gnss.FindAllByProperty(property, value);
        }

        [Test]
        public void TestFindAllByPropertyDoesNotReturnNull()
        {
            string property = "CgndbKey";
            string value = "ABCDE";
            IGnssWebApi gwa = mocks.CreateMock<IGnssWebApi>(); 
            IGnssWebApiRequestBuilder builder = mocks.CreateMock<IGnssWebApiRequestBuilder>();
            GeographicalNameSearchService gnss = new GeographicalNameSearchService(gwa);

            Expect.Call(gwa.CreateRequestBuilder()).Return(builder);
            builder.AddCgndbKey(value);
            LastCall.On(builder).Repeat.Once();
            Expect.Call(builder.Build()).Return(null);
            Expect.Call(gwa.FindByRequest(null)).Return(null);

            mocks.ReplayAll();
            Assert.IsNotNull(gnss.FindAllByProperty(property, value));
        }
	
    }
}
