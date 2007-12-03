using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models
{
    public class WaterBody : Place, ICoordinateAware
    {

        public string Name
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Watershed Watershed
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public SJRAtlas.Models.DataSet[] DataSets
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
