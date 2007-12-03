namespace SJRAtlas.Site.Models
{
    using System;
    using System.Collections.Generic;
    using Castle.ActiveRecord;
    using SJRAtlas.Core;

    [ActiveRecord(Table="map_resources", Mutable = false)]
    public class EasyMap : ActiveRecordBase<EasyMap>, IEasyMap
    {
        private int id;
        private string title;
        private bool fullBasinCoverage;
        private string thumbnail;
        private DateTime createdOn;
        private DateTime modifiedOn;
        private string arcgisResource;
        private IMetadata metadata;
        private string largenail;

        //private string description;
        //private int version;
        //private DateTime date;
        //private string owner;
        //private string largenail;
        //private string link;

        [PrimaryKey]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Property]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        [Property("arcgis_resource")]
        public string ArcgisResource
        {
            get { return arcgisResource; }
            set { arcgisResource = value; }
        }

        //[Property(Column = "map_description")]
        public string Description
        {
            get { return (Metadata == null ? String.Empty : Metadata.Abstract); }
            //set { description = value; }
        }

        [Property(Column = "thumbnail_url")]
        public string Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }

        //[Property(Column = "map_version")]
        //public int Version
        //{
        //    get { return version; }
        //    set { version = value; }
        //}

        //[Property(Column = "map_date")]
        //public DateTime Date
        //{
        //    get { return date; }
        //    set { date = value; }
        //}

        //[Property(Column = "map_owner")]
        //public string Owner
        //{
        //    get { return owner; }
        //    set { owner = value; }
        //}

        [Property(Column = "large_thumbnail_url")]
        public string LargeThumbnail
        {
            get { return largenail; }
            set { largenail = value; }
        }

        //[Property(Column = "map_link")]
        //public string Link
        //{
        //    get { return link; }
        //    set { link = value; }
        //}

        [Property("full_basin_coverage")]
        public bool FullBasinCoverage
        {
            get { return fullBasinCoverage; }
            set { fullBasinCoverage = value; }
        }

        #region IEasyMap Members

        public string ResourceName
        {
            get { return this.ArcgisResource; }
        }

        public IMetadata Metadata
        {
            get { return metadata; }
            set { metadata = value; }
        }

        #endregion


    }    
}
