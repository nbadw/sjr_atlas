using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord("interactive_maps")]
    public class InteractiveMap : ActiveRecordBase<InteractiveMap>, IMetadataAware
    {
        #region ActiveRecord Properties

        private int id;	

        [PrimaryKey("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        #endregion

        #region IMetadataAware Members

        public Metadata GetMetadata()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
