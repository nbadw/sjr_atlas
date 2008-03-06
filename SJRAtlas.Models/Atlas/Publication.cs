using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections.Generic;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("web_publications", DiscriminatorColumn = "type", DiscriminatorType = "String", DiscriminatorValue = "Publication")]
    public class Publication : ActiveRecordBase<Publication>, IMetadataAware
    {
        public static IList<Publication> FindAllByQuery(string query)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Publication>();
            criteria.Add(Expression.Like("Title", query));
            return ActiveRecordMediator<Publication>.FindAll(criteria,
                new Order[] { Order.Asc("Title") });
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
