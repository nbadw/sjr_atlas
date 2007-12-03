using System;
using Castle.Core.Logging;
using SJRAtlas.Core;
using System.Reflection;

namespace SJRAtlas.CGNS
{
    public class GeographicalNameSearchService : IPlaceNameLookup
    {
        public GeographicalNameSearchService(IGnssWebApi gnssWebApi)
        {
            if (gnssWebApi == null)
            {
                throw new ArgumentNullException("gnssWebApi");
            }

            this.gnssWebApi = gnssWebApi;
            this.logger = Castle.Core.Logging.NullLogger.Instance;
        }

        #region ILookupService<IPlaceName> Members

        public IPlaceName[] FindByQuery(string query)
        {
            Logger.Info("Searching Placenames for " + query);
            IGnssWebApiRequestBuilder builder = GnssWebApi.CreateRequestBuilder();
            builder.ParseQuery(query);
            IPlaceName[] results = GnssWebApi.FindByRequest(builder.Build());

            if (results == null) results = new IPlaceName[0];
            
            Logger.Info("Placename search for " + query + " returned " + results.Length + " results");
            return results;
        }

        public IPlaceName Find(object id)
        {
            Logger.Info("Searching Placenames for CGNDB Key=" + id);
            IGnssWebApiRequestBuilder builder = GnssWebApi.CreateRequestBuilder();
            builder.AddCgndbKey(id.ToString());
            IPlaceName[] results = GnssWebApi.FindByRequest(builder.Build());

            if (results == null)
            {
                Logger.Info("Placename search for CGNDB Key=" + id + " returned no result");
                return null;
            }
            else
            {
                Logger.Info("Placename search for CGNDB Key=" + id + " returned " + results.Length + " results");
                return results[0];
            }
        }

        public IPlaceName[] FindAll()
        {
            throw new NotSupportedException("The FindAll() functionality of the GNSS is not supported, please use FindByQuery(query), FindById(id), or FindAllByProperty(property, value) instead.");
        }

        public IPlaceName[] FindAllByProperty(string property, object value)
        {
            Logger.Info("Searching GNSS for Placenames where " + property + "=" + value.ToString());

            IPlaceName[] results;
            try
            {
                string method = "Add" + property;
                IGnssWebApiRequestBuilder builder = GnssWebApi.CreateRequestBuilder();
                Type builderType = builder.GetType();
                MethodInfo methodInfo = builderType.GetMethod(method, new Type[] { value.GetType() });
                methodInfo.Invoke(builder, new object[] { value });

                results = GnssWebApi.FindByRequest(builder.Build());
            }
            catch (Exception e)
            {
                Logger.Error("FindAllByProperty could not be performed on property " + property, e);
                throw new NotSupportedException("The FindAllByProperty(property, value) functionality of the GNSS does not support the property " + property);
            }

            if (results == null) 
                results = new IPlaceName[0];

            Logger.Info("Placename search for " + property + "=" + value.ToString() + " returned " + results.Length + " results");
            return results;
        }
        #endregion

        private IGnssWebApi gnssWebApi;

        public IGnssWebApi GnssWebApi
        {
            get { return gnssWebApi; }
            set { gnssWebApi = value; }
        }

        private ILogger logger;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }
    }
}
