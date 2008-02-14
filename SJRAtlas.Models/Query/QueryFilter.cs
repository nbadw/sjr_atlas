using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models.Query
{
    public class QueryFilter
    {
        public QueryFilter(string column, string comparison, object value)
        {
            this.column = column;
            this.comparison = comparison;
            this.value = value;
        }

        private string column;

        public string Column
        {
            get { return column; }
        }

        private string comparison;

        public string Comparison
        {
            get { return comparison; }
        }

        private object value;

        public object Value
        {
            get { return value; }
        }
    }
}
