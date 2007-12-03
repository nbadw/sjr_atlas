using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord(DiscriminatorValue="PublishedMap")]
    public class PublishedMap : Publication, IMetadataAware
    {
        #region IMetadataAware Members

        public Metadata GetMetadata()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
