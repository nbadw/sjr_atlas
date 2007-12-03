using System;
using System.Collections.Generic;
using System.Text;
using SJRAtlas.Core;

namespace SJRAtlas.DataWarehouse
{
    public class DrainageUnitLookup : IWatershedLookup
    {
        //private Regex reDrainageCode;

        public DrainageUnitLookup()
        {
            //reDrainageCode = new Regex("[:z]{2}-[:z]{2}-[:z]{2}-[:z]{2}-[:z]{2}-[:z]{2}");
        }

        #region ILookupService<IWatershed> Members

        public IWatershed Find(object id)
        {
            return DrainageUnit.Find(id);
        }

        public IWatershed[] FindAll()
        {
            return DrainageUnit.FindAll();
        }

        public IWatershed[] FindByQuery(string query)
        {
            return DrainageUnit.FindAllByUnitNameSearch(query);
        }

        public IWatershed[] FindAllByProperty(string property, object value)
        {
            return DrainageUnit.FindAllByProperty(property, value);
        }

        #endregion
    }
}
