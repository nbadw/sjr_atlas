using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord.Queries;

namespace SJRAtlas.Models.Finders
{
    public class InteractiveMapFinder : IEntityFinder<InteractiveMap>
    {
        #region IEntityFinder<InteractiveMap> Members

        public InteractiveMap Find(object id)
        {
            return InteractiveMap.TryFind(id);
        }

        public virtual InteractiveMap[] FindAllByQuery(string query)
        {
            SimpleQuery<InteractiveMap> command = new SimpleQuery<InteractiveMap>(
                "from InteractiveMap i where i.Title like ?", query);
            return command.Execute();
        }

        #endregion
    }
}
