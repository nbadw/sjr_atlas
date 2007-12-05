using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models.Finders
{
    public class NullFinder<T> : IEntityFinder<T>
    {
        #region IEntityFinder<T> Members

        public T Find(object id)
        {
            return default(T);
        }

        #endregion
    }
}
