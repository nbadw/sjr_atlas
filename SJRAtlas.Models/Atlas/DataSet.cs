using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using Newtonsoft.Json;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("web_data_sets")]
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

        private bool isWaterBodyFilterAware;

        [Property("waterbody_filter_aware")]
        public bool IsWaterBodyFilterAware
        {
            get { return isWaterBodyFilterAware; }
            set { isWaterBodyFilterAware = value; }
        }

        private bool isWatershedFilterAware;

        [Property("watershed_filter_aware")]
        public bool IsWatershedFilterAware
        {
            get { return isWatershedFilterAware; }
            set { isWatershedFilterAware = value; }
        }

        private bool isAgencyFilterAware;

        [Property("agency_filter_aware")]
        public bool IsAgencyFilterAware
        {
            get { return isAgencyFilterAware; }
            set { isAgencyFilterAware = value; }
        }

        private bool isAquaticSiteFilterAware;

        [Property("aquatic_site_filter_aware")]
        public bool IsAquaticSiteFilterAware
        {
            get { return isAquaticSiteFilterAware; }
            set { isAquaticSiteFilterAware = value; }
        }

        private bool isDateFilterAware;

        [Property("date_filter_aware")]
        public bool IsDateFilterAware
        {
            get { return isDateFilterAware; }
            set { isDateFilterAware = value; }
        }
                
        private IList<Presentation> presentations = new List<Presentation>();
            
        [JsonIgnore]
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
