using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SJRAtlas.Models
{
    public interface IEntityFinder<T>
    {
        T Find(object id);
        //T[] FindByTextQuery<T>(params string[] fields);
        //T[] FindByTextQuery<T>(params string[] includedFields, params string[] excludedFields);
    }
}
