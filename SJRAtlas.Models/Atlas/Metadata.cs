using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Xml;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("web_metadata")]
    public class Metadata : ActiveRecordBase<Metadata>
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

        private string uri;

        [Property("uri", Unique = true)]
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }
        
        private IMetadataAware owner;

        [Any(typeof(int), MetaType = typeof(string), TypeColumn = "metadata_aware_type", 
            IdColumn = "metadata_aware_id", Cascade = CascadeEnum.SaveUpdate)]
        [Any.MetaValue("Publication", typeof(Publication))]
        [Any.MetaValue("PublishedMap", typeof(PublishedMap))]
        [Any.MetaValue("PublishedReport", typeof(PublishedReport))]
        [Any.MetaValue("DataSet", typeof(DataSet))]
        [Any.MetaValue("InteractiveMap", typeof(InteractiveMap))]
        public IMetadataAware Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public static Metadata FindByOwner(IMetadataAware owner)
        {
            DetachedCriteria criteria = DetachedCriteria.For<Metadata>();
            criteria.Add(Expression.Eq("Owner", owner));
            return ActiveRecordMediator<Metadata>.FindOne(criteria);
        }

        public static Metadata FindByOwner(int ownerId, string type)
        {
            NHibernate.ISession session = ActiveRecordMediator
                .GetSessionFactoryHolder()
                .CreateSession(typeof(Metadata));

            try
            {
                return session.CreateSQLQuery(
                        "SELECT * FROM web_metadata WHERE " + 
                        "metadata_aware_id = " + ownerId.ToString() +
                        " AND metadata_aware_type = '" + type + "'"
                    )
                    .AddEntity(typeof(Metadata))
                    .UniqueResult<Metadata>();
            }
            finally
            {
                ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(session);
            }
        }
    }
}
