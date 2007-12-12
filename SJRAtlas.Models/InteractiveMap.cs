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
        public InteractiveMap()
        {
            isBasinMap = false;
        }

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

        private bool isBasinMap;

        [Property("is_basin_map")]
	    public bool IsBasinMap
	    {
		    get { return isBasinMap;}
		    set { isBasinMap = value;}
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

        public static IList<InteractiveMap> FindAllBasinMaps()
        {
            DetachedCriteria criteria = DetachedCriteria.For<InteractiveMap>();
            criteria.Add(Expression.Eq("IsBasinMap", true));
            return InteractiveMap.FindAll(criteria, new Order[] { Order.Asc("Title") });
        }

        public static InteractiveMap FindByTitle(string title)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InteractiveMap>();
            criteria.Add(Expression.Eq("Title", title));
            return InteractiveMap.FindFirst(criteria);
        }
    }
}
