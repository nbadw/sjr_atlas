using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace SJRAtlas.Models
{
    [ActiveRecord("interactive_maps")]
    public class InteractiveMap : ActiveRecordBase<InteractiveMap>, IMetadataAware
    {
        public static IList<InteractiveMap> FindAllByQuery(string query)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InteractiveMap>();
            criteria.Add(Expression.Like("Title", query));
            return ActiveRecordMediator<InteractiveMap>.FindAll(criteria, 
                new Order[] { Order.Asc("Title") });
        }

        #region ActiveRecord Properties

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
        
        #endregion

        #region IMetadataAware Members

        public Metadata GetMetadata()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
