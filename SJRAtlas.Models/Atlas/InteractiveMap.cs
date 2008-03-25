using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections;
using Castle.ActiveRecord.Queries;
using SJRAtlas.Models.Query;

namespace SJRAtlas.Models.Atlas
{
    public partial class InteractiveMap : IMetadataAware, IComparable
    {
        public InteractiveMap()
        {
            _isBasinMap = false;
        }

        public IList<MapService> MapServices
        {
            get 
            {
                List<MapService> services = new List<MapService>(MapServiceListings.Count);
                foreach (MapServiceListing serviceList in MapServiceListings)
                {
                    services.Add(serviceList.MapService);
                }
                return services;
            }
        }

        protected override bool BeforeSave(IDictionary state)
        {
            if ((DateTime)state["CreatedAt"] == DateTime.MinValue)
                state["CreatedAt"] = DateTime.Now;
            state["UpdatedAt"] = DateTime.Now;
            return true;
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

        #region Finders

        public static IList<InteractiveMap> FindAllByQuery(string q)
        {
            string terms = QueryParser.BuildContainsTerms(q);
            SimpleQuery<InteractiveMap> query = new SimpleQuery<InteractiveMap>(QueryLanguage.Sql,
                String.Format(@"
                    SELECT        *
                    FROM          web_interactive_maps as interactive_map
                    WHERE         CONTAINS (interactive_map.title, '{0}')
                    OR            CONTAINS (interactive_map.abstract, '{0}')",
                    terms
                )
            );
            query.AddSqlReturnDefinition(typeof(InteractiveMap), "interactive_map");
            return query.Execute();
        }

        public static IList<InteractiveMap> FindAllWithFullBasinCoverageByQuery(string query)
        {
            DetachedCriteria criteria = DetachedCriteria.For<InteractiveMap>();
            criteria.Add(Expression.Or(
                Expression.Like("Title", query),
                Expression.Eq("IsBasinMap", true)));
            return ActiveRecordMediator<InteractiveMap>.FindAll(criteria,
                new Order[] { Order.Asc("Title") });
        }

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

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            InteractiveMap im2 = obj as InteractiveMap;
            return string.Compare(Title, im2.Title);
        }

        #endregion
    }
}
