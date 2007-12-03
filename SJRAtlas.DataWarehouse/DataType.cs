using System;
using System.Collections.Generic;
using Castle.ActiveRecord;

namespace SJRAtlas.DataWarehouse
{
    public partial class DataType
    {
        public int Id
        {
            get { return this.DataTypeID; }
            set { this.DataTypeID = value; }
        }

        public string Name
        {
            get { return this.DataName; }
            set { this.DataName = value; }
        }

        private IList<Query> queries;

        [HasMany(typeof(Query), Table = "queries", ColumnKey = "tabular_id", Lazy = true)]
        public IList<Query> Queries
        {
            get { return queries; }
            set { queries = value; }
        }
    }
}
