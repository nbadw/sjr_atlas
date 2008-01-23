using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections;

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

        protected override bool BeforeSave(IDictionary state)
        {
            if ((DateTime)state["CreatedAt"] == DateTime.MinValue)
                state["CreatedAt"] = DateTime.Now;
            state["UpdatedAt"] = DateTime.Now;
            return true;
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

        private string description;

        [Property("description", ColumnType = "StringClob")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        
        private bool isBasinMap;

        [Property("full_basin_coverage")]
	    public bool IsBasinMap
	    {
		    get { return isBasinMap;}
		    set { isBasinMap = value;}
	    }

        private string serviceName;

        [Property("arcgis_service_name")]
        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        private string thumbnailUrl;

        [Property("thumbnail_url")]
        public string ThumbnailUrl
        {
            get { return thumbnailUrl; }
            set { thumbnailUrl = value; }
        }

        private string largeThumbnailUrl;

        [Property("large_thumbnail_url")]
        public string LargeThumbnailUrl
        {
            get { return largeThumbnailUrl; }
            set { largeThumbnailUrl = value; }
        }

        private DateTime createdAt;

        [Property("created_at")]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }

        private DateTime updatedAt;

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

        public static IList<InteractiveMap> FindAllByTitles(params string[] titles)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InteractiveMap>();
            criteria.Add(Expression.InG<string>("Title", titles));
            return InteractiveMap.FindAll(criteria, new Order[] { Order.Asc("Title") });
        }
    }
}
