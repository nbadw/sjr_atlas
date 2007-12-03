using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models
{
    public class DataSet : IMetadataAware
    {
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
