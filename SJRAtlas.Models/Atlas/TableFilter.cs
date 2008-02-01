using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using NHibernate.SqlCommand;
using System.Data;

namespace SJRAtlas.Models.Atlas
{
    public class TableFilter
    {
        public TableFilter() { }

        public TableFilter(string column, string comparison, object value)
        {

        }

        private string column;

        public string Column
        {
            get { return column; }
            set { column = value; }
        }

        private string comparison;

        public string Comparison
        {
            get { return comparison; }
            set { comparison = value; }
        }

        private object val;

        public object Value
        {
            get { return val; }
            set { val = value; }
        }

        public class FilterConfigurer
        {

        }

        public class Filter
        {

        }

        //private string drainageCode;

        //public string DrainageCode
        //{
        //    get { return drainageCode; }
        //    set { drainageCode = value; }
        //}

        //private int waterbodyId;

        //public int WaterBodyId
        //{
        //    get { return waterbodyId; }
        //    set { waterbodyId = value; }
        //}

        //private string agencyCode;

        //public string AgencyCode
        //{
        //    get { return agencyCode; }
        //    set { agencyCode = value; }
        //}

        //private DateTime startDate;

        //public DateTime StartDate
        //{
        //    get { return startDate; }
        //    set { startDate = value; }
        //}

        //private DateTime endDate;

        //public DateTime EndDate
        //{
        //    get { return endDate; }
        //    set { endDate = value; }
        //}	
    }
}
