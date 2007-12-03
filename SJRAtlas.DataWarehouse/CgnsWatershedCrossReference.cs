using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.DataWarehouse
{
    [ActiveRecord("auxCGNS_Watershed_XRef", Mutable = false)]
    public class CgnsWatershedCrossReference : DataWarehouseARBase<CgnsWatershedCrossReference>
    {
        private int id;
        private int joinCount;
        private string cgndbKey;
        private string drainageCode;

        [PrimaryKey("OBJECTID")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Property("Join_Count")]
        public int JoinCount
        {
            get { return joinCount; }
            set { joinCount = value; }
        }

        [Property("cgndb_key")]
        public string CgndbKey
        {
            get { return cgndbKey; }
            set { cgndbKey = value; }
        }

        [Property("DrainageCd")]
        public string DrainageCode
        {
            get { return drainageCode; }
            set { drainageCode = value; }
        }	
    }
}
