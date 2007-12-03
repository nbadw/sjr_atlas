using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord("metadata")]
    public class Metadata : ActiveRecordBase<Metadata>, IEntity
    {
        private int id;

        [PrimaryKey(PrimaryKeyType.Increment, "id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }	

        private string content;

        [Property("content")]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string filename;

        [Property("filename", Unique = true)]
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        private IMetadataAware metadataOwner;

        [Any(typeof(int), MetaType = typeof(string), TypeColumn = "metadata_aware_type", IdColumn = "metadata_aware_id", Cascade = CascadeEnum.SaveUpdate)]
        [Any.MetaValue("PublishedMap", typeof(PublishedMap))]
        [Any.MetaValue("PublishedReport", typeof(PublishedReport))]
        public IMetadataAware MetadataOwner
        {
            get { return metadataOwner; }
            set { metadataOwner = value; }
        }

        #region IEntity Members

        public object GetId()
        {
            return Id;
        }

        #endregion
    }
}
