using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;

namespace SJRAtlas.Models.DataWarehouse
{
    public partial class AquaticSite
    {
        public static IList<AquaticSite> FindAllValid()
        {
            return AquaticSite.FindAll(DetachedCriteria.For<AquaticSite>()
                .Add(Expression.Gt("Id", 0)),
                Order.Asc("Id")
            );
        }
    }
}
