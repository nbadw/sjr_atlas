using System;
using Castle.ActiveRecord;

namespace SJRAtlas.DataWarehouse
{
    [ActiveRecord("auxWaterBody_DataType_XRef", Mutable = false)]
    public class WaterBodyDataTypeXRef : DataWarehouseARBase<WaterBodyDataTypeXRef>
    {
        #region ActiveRecord Properties

        private int dataRefId;
        [PrimaryKey("DataRefID")]
        public int DataRefId
        {
            get { return dataRefId; }
            set { dataRefId = value; }
        }

        private string drainageCd;
        [Property]
        public string DrainageCd
        {
            get { return drainageCd; }
            set { drainageCd = value; }
        }
        
        private int waterBodyId;
        [Property("WaterBodyID")]
        public int WaterBodyId
        {
            get { return waterBodyId; }
            set { waterBodyId = value; }
        }

        private string waterBodyName;
        [Property]
        public string WaterBodyName
        {
            get { return waterBodyName; }
            set { waterBodyName = value; }
        }

        private int dataTypeId;
        [Property("DataTypeID")]
        public int DataTypeId
        {
            get { return dataTypeId; }
            set { dataTypeId = value; }
        }

        private string dataName;
        [Property]
        public string DataName
        {
            get { return dataName; }
            set { dataName = value; }
        }
        #endregion

    }
}
