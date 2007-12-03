using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord(DiscriminatorValue = "PublishedReport")]
    public class PublishedReport : Publication, IMetadataAware
    {
        #region IMetadataAware Members

        public Metadata GetMetadata()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
