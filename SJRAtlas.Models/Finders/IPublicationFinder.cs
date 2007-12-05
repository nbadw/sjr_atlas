using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Castle.ActiveRecord.Queries;
using NHibernate.Expression;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Finders
{
    public class IPublicationFinder : IEntityFinder<IPublication>
    {
        #region IEntityFinder<IPublication> Members

        public IPublication Find(object id)
        {
            return Publication.TryFind(id);
        }

        public virtual IPublication[] FindAllByQuery(string query)
        {
            SimpleQuery<Publication> command = new SimpleQuery<Publication>(
                "from Publication p where p.Title like ? or p.Abstract like ?",
                query, query);
            return command.Execute();
        }

        public T[] FindAllByQuery<T>(string query) where T : Publication
        {
            DetachedCriteria criteria = DetachedCriteria.For<T>();
            criteria.Add(Expression.Like("Title", query)).
                Add(Expression.Like("Abstract", query));

            return ActiveRecordMediator<T>.FindAll(criteria);
        }

        #endregion
    }
}
