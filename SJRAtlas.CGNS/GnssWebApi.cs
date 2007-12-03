using System;
using Castle.Core.Logging;
using System.IO;
using System.Net;

namespace SJRAtlas.CGNS
{
    public class GnssWebApi : IGnssWebApi
    {
        public GnssWebApi(string url) : this(new Uri(url, UriKind.Absolute))
        {            
        }

        public GnssWebApi(Uri baseUrl)
        {
            this.baseUrl = baseUrl;
            this.logger = Castle.Core.Logging.NullLogger.Instance;
        }
        
        #region IGnssWebApi Members

        public GnssPlaceName[] FindByRequest(Uri request)
        {
            return GnssSearchResults.CreateFromXml(GetResponseStream(request)).Items;
        }

        public IGnssWebApiRequestBuilder CreateRequestBuilder()
        {
            Logger.Debug("Creating instance of GnssRequestBuilder with base URL " + BaseUrl.ToString());
            GnssWebApiRequestBuilder builder = new GnssWebApiRequestBuilder(BaseUrl);
            builder.Logger = Logger;
            return builder;
        }

        #endregion

        private Stream GetResponseStream(Uri request)
        {
            WebRequest webRequest = WebRequest.Create(request);
            webRequest.Timeout = Timeout;
            return webRequest.GetResponse().GetResponseStream();
        }

        private Uri baseUrl;

        public Uri BaseUrl
        {
            get { return baseUrl; }
            set { baseUrl = value; }
        }

        private int timeout = 10000;

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        private ILogger logger;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }
    }
}
