using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;

namespace SJRAtlas.Models.Finders
{
    public class ClosestWatershedToPlaceFinder : IEntityFinder<ClosestWatershedToPlace>
    {
        public virtual ClosestWatershedToPlace FindByCgndbKey(string cgndbKey)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ClosestWatershedToPlace>();
            criteria.Add(Expression.Eq("Place", cgndbKey));
            return ClosestWatershedToPlace.FindOne(criteria);
        }

        #region IEntityFinder<ClosestWatershedToPlace> Members

        public ClosestWatershedToPlace Find(object id)
        {
            return ClosestWatershedToPlace.TryFind(id);
        }

        #endregion
    }
}
