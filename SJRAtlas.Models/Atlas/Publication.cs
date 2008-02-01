using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections.Generic;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("publications", DiscriminatorColumn="type", DiscriminatorType="String", DiscriminatorValue="Publication")]
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

        private string summary;

        [Property("summary")]
        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        private string author;

        [Property("author")]
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        private string file;

        [Property("filename")]
        public string File
        {
            get { return file; }
            set { file = value; }
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
