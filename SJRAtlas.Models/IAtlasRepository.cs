using System;
using System.Collections;
using SJRAtlas.Models.Finders;
namespace SJRAtlas.Models
{
    public interface IAtlasRepository
    {
        T Find<T>(object id);
        T GetFinder<T>();
    }

    public class NullAtlasRepository : IAtlasRepository
    {
        #region IAtlasRepository Members

        public T Find<T>(object id)
        {
            return default(T);
        }

        public T GetFinder<T>()
        {
            return default(T);
        }

        #endregion
    }
}
