using System;
using Castle.Core.Logging;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.CGNS
{
    public class GnssWebApiRequestBuilder : IGnssWebApiRequestBuilder
    {
        private Uri apiBase;
        private IDictionary<string, string> parameters;

        public GnssWebApiRequestBuilder(Uri apiBase)
        {
            this.apiBase = apiBase;
            this.parameters = new Dictionary<string, string>();
        }

        #region IGnssWebApiRequestBuilder Members

        public void ParseQuery(string query)
        {
            Logger.Debug("Parsing parameters for " + query);

            parameters.Add("output", "xml");
            parameters.Add("geoname", query);
            parameters.Add("regionCode", "13");

            // TODO: implements the optional search parameters described in GNSS_Users_Guide.pdf
        }

        public void AddCgndbKey(string id)
        {
            parameters.Add("cgndbKey", id);
        }

        public Uri Build()
        {
            if (parameters.ContainsKey("cgndbKey"))
            {
                string cgndb_key = parameters["cgndbKey"];
                parameters.Clear();
                parameters.Add("cgndbKey", cgndb_key);
            }

            StringBuilder requestBuilder = new StringBuilder();
            string valueJoin = "=";
            string parameterJoin = "&";

            requestBuilder.Append(apiBase.ToString());
            requestBuilder.Append("?");
            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                requestBuilder.Append(parameter.Key);
                requestBuilder.Append(valueJoin);
                requestBuilder.Append(parameter.Value);
                requestBuilder.Append(parameterJoin);
            }

            Uri builtUrl = new Uri(requestBuilder.ToString().Replace(' ', '+'));
            Logger.Debug("Built URL " + builtUrl.ToString());

            parameters.Clear();

            return builtUrl;
        }

        #endregion

        private ILogger logger = Castle.Core.Logging.NullLogger.Instance;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }
    }
}
