using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Core
{
    public class NullLookupService<T> : ILookupService<T>
    {
        #region ILookupService<T> Members

        public T Find(object id)
        {
            return default(T);
        }

        public T[] FindAll()
        {
            return new T[0];
        }

        public T[] FindByQuery(string query)
        {
            return new T[0];
        }

        public T[] FindAllByProperty(string propery, object value)
        {
            return new T[0];
        }

        #endregion
    }

    public class NullPlaceNameLookupService : NullLookupService<IPlaceName>, IPlaceNameLookup
    {
        public static NullPlaceNameLookupService INSTANCE = new NullPlaceNameLookupService();
    }

    public class NullWatershedLookupService : NullLookupService<IWatershed>, IWatershedLookup
    {
        public static NullWatershedLookupService INSTANCE = new NullWatershedLookupService();
    }

    public class NullMetadataLookupService : NullLookupService<IMetadata>, IMetadataLookup
    {
        public static NullMetadataLookupService INSTANCE = new NullMetadataLookupService();

        #region IMetadataLookup Members

        public IMetadata[] FindByType(MetadataType type)
        {
            return new IMetadata[0];
        }

        #endregion
    }

    public class NullEasyMapLookupService : NullLookupService<IEasyMap>, IEasyMapLookup
    {
        public static NullEasyMapLookupService INSTANCE = new NullEasyMapLookupService();
    }
}
