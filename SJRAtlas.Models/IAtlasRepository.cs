using System;
using System.Collections;
namespace SJRAtlas.Models
{
    public interface IAtlasRepository
    {
        T Find<T>(object id);
    }

    public class NullAtlasRepository : IAtlasRepository
    {
        #region IAtlasRepository Members

        public T Find<T>(object id)
        {
            return default(T);
        }

        #endregion
    }
}
