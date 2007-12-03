using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SJRAtlas.Models
{
    public class AtlasRepository : IAtlasRepository
    {
        public AtlasRepository() : this(new Hashtable())
        {
        }

        public AtlasRepository(IDictionary finders)
        {
            this.finders = finders;
        }

        private IDictionary finders;

        public IDictionary Finders
        {
            get { return finders; }
            set { finders = value; }
        }

        #region IAtlasRepository Members

        public T Find<T>(object id)
        {
            IEntityFinder<T> finder = (IEntityFinder<T>)Finders[typeof(T)];
            if (finder == null)
                return default(T);

            return finder.Find(id);
        }

        //public T[] FindByDefaultQuery<T>(object queryParameter)
        //{
        //    IEntityFinder<T> finder = (IEntityFinder<T>)Finders[typeof(T)];
        //    if (finder == null)
        //        return new T[0];

        //    return finder.FindByDefaultQuery(queryParameter);
        //}

        //public T[] FindByQuery<T>(string query, params object[] positionalParameters)
        //{
        //    IEntityFinder<T> finder = (IEntityFinder<T>)Finders[typeof(T)];
        //    if (finder == null)
        //        return new T[0];

        //    return finder.FindByQuery(query, positionalParameters);
        //}

        #endregion
    }
}
