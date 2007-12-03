using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Castle.ActiveRecord.Queries;

namespace SJRAtlas.Models.Finders
{
    public class IPublicationFinder : IEntityFinder<IPublication>
    {
        #region IEntityFinder<IPublication> Members

        public IPublication Find(object id)
        {
            return Publication.TryFind(id);
        }

        public IPublication[] FindByQuery(string query, params object[] positionalParamters)
        {
            SimpleQuery<Publication> command = new SimpleQuery<Publication>(
                query, positionalParamters);
            return command.Execute();
        }

        public IPublication[] FindByDefaultQuery(object queryParameter)
        {
            SimpleQuery<Publication> command = new SimpleQuery<Publication>(
                "from Publication p where p.Title like ? or p.Summary like ?",
                queryParameter, queryParameter);
            return command.Execute();
        }

        #endregion
    }
}
