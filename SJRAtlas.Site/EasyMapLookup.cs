using System;
using SJRAtlas.Core;
using SJRAtlas.Site.Models;

namespace SJRAtlas.Site
{
    public class EasyMapLookup : IEasyMapLookup
    {
        #region ILookupService<IEasyMap> Members

        public IEasyMap Find(object id)
        {
            return EasyMap.Find(id);
        }

        public IEasyMap[] FindAll()
        {
            return EasyMap.FindAll();
        }

        public IEasyMap[] FindByQuery(string query)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEasyMap[] FindAllByProperty(string property, object value)
        {
            return EasyMap.FindAllByProperty(property, value);
        }

        #endregion
    }
}
