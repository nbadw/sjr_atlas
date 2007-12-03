using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Core
{
    public class DummyPlaceNameLookup : IPlaceNameLookup
    {
        #region ISearchService<IPlaceName> Members

        public IPlaceName[] FindByQuery(string query)
        {
            int len = 5;
            IPlaceName[] placenames = new IPlaceName[len];
            for (int i = 0; i < len; i++)
            {
                placenames[i] = new DummyPlaceName();
            }
            return placenames;
        }

        public IPlaceName Find(object id)
        {
            return new DummyPlaceName();
        }

        #endregion

        #region ILookupService<IPlaceName> Members


        public IPlaceName[] FindAll()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IPlaceName[] FindByQuery()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IPlaceName[] FindAllByProperty(string propery, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class DummyPlaceName : IPlaceName
    {
        public DummyPlaceName()
        {
            id = "ABCDE";
            name = "Dummy Result";
            region = "New Brunswick";
            county = null;
            latitude = "65° 50' West";
            longitude = "45° 20' North";
            latdec = 0.0;
            longdec = 0.0;
        }

        #region IPlaceName Members

        public string Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Region
        {
            get { return region; }
        }

        public string County
        {
            get { return county; }
        }

        public string Latitude
        {
            get { return latitude; }
        }

        public string Longitude
        {
            get { return longitude; }
        }

        public double LatDec
        {
            get { return latdec; }
        }

        public double LongDec
        {
            get { return longdec; }
        }

        public string StatusTerm
        {
            get { return "STATUS"; }
        }

        public string ConciseTerm
        {
            get { return "CONCISE"; }
        }

        public string GenericTerm
        {
            get { return "GENERIC"; }
        }

        #endregion

        private string id;
        private string name;
        private string region;
        private string county;
        private string latitude;
        private string longitude;
        private double latdec;
        private double longdec;
    }

    public class DummyWatershedLookup : IWatershedLookup
    {
        #region ISearchService<IWatershed> Members

        public IWatershed[] FindByQuery(string query)
        {
            int len = 5;
            IWatershed[] watersheds = new IWatershed[len];
            for (int i = 0; i < len; i++)
            {
                watersheds[i] = new DummyWatershed();
            }
            return watersheds;
        }

        public IWatershed Find(object id)
        {
            return new DummyWatershed();
        }

        #endregion

        #region ILookupService<IWatershed> Members


        public IWatershed[] FindAll()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IWatershed[] FindByQuery()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IWatershed[] FindAllByProperty(string propery, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class DummyWatershed : IWatershed
    {

        #region IWatershed Members

        public string Id
        {
            get { return "ID";  }
        }

        public string Name
        {
            get { return "Watershed Name"; }
        }

        public string DrainageCode
        {
            get { return "00-00-00-00-00-00"; }
        }

        public string TributaryOf
        {
            get { return "parent tributary"; }
        }

        public string DrainsInto
        {
            get { return null; }
        }
        #endregion

        #region IWatershed Members


        public string CgndbKey
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IPlaceName PlaceName
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }

    public class DummyMetadataLookup : IMetadataLookup
    {
        #region ISearchService<IMetadata> Members

        public IMetadata[] FindByQuery(string query)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ILookupService<IMetadata> Members

        public IMetadata Find(object id)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMetadata[] FindAll()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMetadata[] FindByQuery()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMetadata[] FindAllByProperty(string propery, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IMetadataLookup Members

        public IMetadata[] FindByType(MetadataType type)
        {
            return new IMetadata[0];
        }

        #endregion
    }
}
