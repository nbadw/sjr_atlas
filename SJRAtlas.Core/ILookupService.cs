using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Core
{
    public interface ILookupService<T>
    {
        T Find(object id);
        T[] FindAll();
        T[] FindByQuery(string query);
        T[] FindAllByProperty(string property, object value);
    }
}
