using System;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord("publications", DiscriminatorColumn="type", DiscriminatorType="String", DiscriminatorValue="Publication")]
    public class Publication : ActiveRecordBase<Publication>, IPublication, IEntity
    {
        #region IPublication Members

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
        public string Abstract
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

        #region IEntity Members

        public object GetId()
        {
            return Id;
        }

        #endregion
    }
}
