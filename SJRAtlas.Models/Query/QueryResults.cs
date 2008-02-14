using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models.Query
{
    public class QueryResults
    {
        private int totalRowCount;

        public int TotalRowCount
        {
            get { return totalRowCount; }
            set { totalRowCount = value; }
        }

        private IList<object[]> rows = new List<object[]>();

        public IList<object[]> Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        private string[] columns = new string[0];

        public string[] Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("Query Results - ");
            builder.AppendFormat("TotalRowCount: {0}, Columns: {1}, Rows: {2}", TotalRowCount, InspectColumns(), InspectRows());
            return builder.ToString();
        }

        private string InspectRows()
        {
            StringBuilder builder = new StringBuilder("[");
            for(int i=0; i < Rows.Count; i++)
            {
                builder.Append(InspectArray(Rows[i]));
                if (i < Rows.Count - 1)
                    builder.Append(", ");
            }
            builder.Append("]");
            return builder.ToString();
        }

        private string InspectColumns()
        {
            return InspectArray(Columns);
        }

        private string InspectArray(object[] array)
        {
            StringBuilder builder = new StringBuilder("[");
            for (int i = 0; i < array.Length; i++)
            {
                builder.Append(array[i]);
                if (i < array.Length - 1)
                    builder.Append(", ");
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}
