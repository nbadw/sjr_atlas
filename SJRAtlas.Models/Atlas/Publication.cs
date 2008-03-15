using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections.Generic;
using SJRAtlas.Models.Query;
using Castle.ActiveRecord.Queries;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("web_publications", DiscriminatorColumn = "type", DiscriminatorType = "String", DiscriminatorValue = "Publication")]
    public class Publication : ActiveRecordBase<Publication>, IMetadataAware
    {
        public static IList<Publication> FindAllByQuery(string q)
        {
            string terms = QueryParser.BuildContainsTerms(q);
            SimpleQuery<Publication> query = new SimpleQuery<Publication>(QueryLanguage.Sql,
                String.Format(@"
                    SELECT        *
                    FROM          web_publications as publication
                    WHERE         CONTAINS (publication.title, '{0}')
                    OR            CONTAINS (publication.abstract, '{0}')",
                    terms
                )
            );
            query.AddSqlReturnDefinition(typeof(Publication), "publication");
            return query.Execute();
        }

        #region ActiveRecord Properties

        private int id;

        [PrimaryKey(PrimaryKeyType.Native, "id")]
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

        [Property("abstract")]
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

        private string uri;

        [Property("uri")]
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        private string mimeType;

        [Property("mime_type")]
        public string MimeType
        {
            get { return mimeType; }
            set { mimeType = value; }
        }
	

        private DateTime createdAt = DateTime.Now;

        [Property("created_at")]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }

        private DateTime updatedAt = DateTime.Now;

        [Property("updated_at")]
        public DateTime UpdatedAt
        {
            get { return updatedAt; }
            set { updatedAt = value; }
        }	

        #endregion

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
