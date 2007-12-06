using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord("auxDataTypes")]
    public class DataSet : ActiveRecordBase<DataSet>, IMetadataAware
    {
        private int id;

        [PrimaryKey("DataTypeID")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public SJRAtlas.Models.Presentation[] Presentations
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #region IMetadataAware Members

        public Metadata GetMetadata()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
