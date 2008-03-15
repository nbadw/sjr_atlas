using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using Newtonsoft.Json;
using Castle.ActiveRecord.Queries;
using NHibernate;
using SJRAtlas.Models.Query;

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

        private string _abstract;

        [Property("abstract", ColumnType = "StringClob")]
        public string Abstract
        {
            get { return _abstract; }
            set { _abstract = value; }
        }

        private string origin;

        [Property("origin")]
        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        private string timePeriod;

        [Property("time_period")]
        public string TimePeriod
        {
            get { return timePeriod; }
            set { timePeriod = value; }
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

        public static IList<DataSet> FindAllWithPresentation(string presentationType)
        {
            SimpleQuery<DataSet> query = new SimpleQuery<DataSet>(QueryLanguage.Sql, @"
                SELECT  {dataset.*}
                FROM     web_data_sets as dataset, web_presentations as presentation
                WHERE   (dataset.id = presentation.data_set_id AND presentation.type = ?)",
                presentationType
            );
            query.AddSqlReturnDefinition(typeof(DataSet), "dataset");
            return query.Execute();
        }

        public static IList<DataSet> FindAllByQuery(string q)
        {
            string terms = QueryParser.BuildContainsTerms(q);
            SimpleQuery<DataSet> query = new SimpleQuery<DataSet>(QueryLanguage.Sql, 
                String.Format(@"
                    SELECT      {{dataset.*}}
                    FROM          web_data_sets as dataset, web_metadata as metadata
                    WHERE         metadata.metadata_aware_type = 'DataSet' 
                    AND           dataset.id = metadata.metadata_aware_id
                    AND (
                        CONTAINS (metadata.content, '{0}')
                        OR CONTAINS (dataset.title, '{0}')
                        OR CONTAINS (dataset.abstract, '{0}')
                    )",
                    terms
                )
            );
            query.AddSqlReturnDefinition(typeof(DataSet), "dataset");
            return query.Execute();
        }
    }
}
