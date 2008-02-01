using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("data_sets")]
    public class DataSet : ActiveRecordBase<DataSet>, IMetadataAware
    {
        private int id;

        [PrimaryKey("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string title;

        [Property("title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string abstractText;

        [Property("abstract", ColumnType = "StringClob")]
        public string Abstract
        {
            get { return abstractText; }
            set { abstractText = value; }
        }

        private string author;

        [Property("author")]
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        private string origin;

        [Property("origin")]
        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        //private TimePeriod timePeriod;

        //public TimePeriod MyProperty
        //{
        //    get { return timePeriod; }
        //    set { timePeriod = value; }
        //}
        
        private IList<Presentation> presentations = new List<Presentation>();
                
        [HasMany(typeof(Presentation), Table="presentations", ColumnKey="data_set_id")]
        public IList<Presentation> Presentations
        {
            get { return presentations; }
            set { presentations = value; }
        }

        #region IMetadataAware Members

        private Metadata metadata;

        public Metadata GetMetadata()
        {
            if (metadata == null)
                metadata = Metadata.FindByOwner(this);

            return metadata;
        }

        #endregion
    }
}
