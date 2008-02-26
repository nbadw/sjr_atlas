using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NHibernate;
using Castle.ActiveRecord;
using Castle.Core.Logging;
using System.Collections.Specialized;

namespace SJRAtlas.Models.Query
{
    public class CustomQuery
    {
        private readonly string query;
        private int limit;
        private int offset;
        private List<QueryFilter> filters;
        private ILogger logger;

        public CustomQuery(string query)
        {
            this.query = query;
            this.limit = 50;
            this.offset = 0;
            filters = new List<QueryFilter>();
            logger = NullLogger.Instance;
        }

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public CustomQuery StartAtRow(int offset)
        {
            this.offset = offset;
            return this;
        }

        public CustomQuery LimitResultsTo(int limit)
        {
            this.limit = limit;
            return this;
        }

        #region Filters

        public CustomQuery ConfigureFilters(NameValueCollection parameters)
        {
            if (!String.IsNullOrEmpty(parameters["drainageCode"]))
                FilterByDrainageCode(parameters["drainageCode"]);

            if (!String.IsNullOrEmpty(parameters["waterbodyId"]))
            {
                int id = int.Parse(parameters["waterbodyId"]);
                if(id > 0)
                    FilterByWaterBodyId(id);
            }

            if (!String.IsNullOrEmpty(parameters["agencyCode"]))
                FilterByAgencyCode(parameters["agencyCode"]);

            if (!String.IsNullOrEmpty(parameters["aquaticSiteId"]))
            {
                int id = int.Parse(parameters["aquaticSite"]);
                if (id > 0)
                    FilterByAquaticSiteId(id);
            }

            if (!String.IsNullOrEmpty(parameters["startDate"]))
            {
                DateTime startDate = DateTime.Parse(parameters["startDate"]);
                if (startDate > DateTime.MinValue && startDate < DateTime.MaxValue)
                    FilterByStartDate(startDate);                
            }

            if (!String.IsNullOrEmpty(parameters["endDate"]))
            {
                DateTime endDate = DateTime.Parse(parameters["endDate"]);
                if (endDate > DateTime.MinValue && endDate < DateTime.MaxValue)
                    FilterByEndDate(endDate);
            }

            return this;
        }

        protected void AddFilter(QueryFilter filter)
        {
            filters.Add(filter);
        }

        public CustomQuery FilterBy(string column, string comparison, object value)
        {
            AddFilter(new QueryFilter(column, comparison, value));
            return this;
        }

        public CustomQuery FilterByDrainageCode(string drainageCode)
        {
            return FilterBy("DrainageCd", "=", drainageCode);
        }

        public CustomQuery FilterByWaterBodyId(int waterbodyId)
        {
            return FilterBy("WaterBodyID", "=", waterbodyId);
        }

        public CustomQuery FilterByAquaticSiteId(int aquaticSiteId)
        {
            return FilterBy("AquaticSiteID", "=", aquaticSiteId);
        }

        public CustomQuery FilterByAgencyCode(string agencyCode)
        {
            return FilterBy("AgencyCd", "=", agencyCode);
        }

        public CustomQuery FilterByStartDate(DateTime startDate)
        {
            return FilterBy("StartDate", ">=", startDate.ToString("yyyy/MM/dd"));
        }

        public CustomQuery FilterByEndDate(DateTime endDate)
        {
            return FilterBy("EndDate", "<=", endDate.ToString("yyyy/MM/dd"));
        }

        protected void AddFilters(IDbCommand command)
        {
            if (filters.Count == 0)
                return;

            StringBuilder filteredSql = new StringBuilder(command.CommandText);
            filteredSql.Append(" WHERE (");
            foreach (QueryFilter filter in filters)
            {
                string parameterName = String.Format("@{0}", filter.Column);
                filteredSql.AppendFormat("{0} {1} {2}", filter.Column, filter.Comparison, parameterName);

                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = parameterName;
                parameter.Value = filter.Value;
                command.Parameters.Add(parameter);

                if (filters.IndexOf(filter) < filters.Count - 1)
                    filteredSql.Append(" AND ");
            }
            filteredSql.Append(")");

            command.CommandText = filteredSql.ToString();
        }

        private string InspectFilters()
        {
            StringBuilder builder = new StringBuilder("[");
            for(int i=0; i < filters.Count; i++)
            {
                builder.Append(filters[i].ToString());
                if (i < filters.Count - 1)
                    builder.Append(", ");
            }
            builder.Append("]");
            return builder.ToString();
        }

        #endregion

        public QueryResults Execute()
        {
            Logger.Info("Executing Custom Query");
            Logger.Debug(String.Format("Start Row: {0}, Limit: {1}", offset, limit));
            Logger.Debug(String.Format("Filters: {0}", InspectFilters()));
            QueryResults results = new QueryResults();
            ISession session = ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ActiveRecordBase));
            Logger.Info("Session Acquired");

            try
            {
                results.TotalRowCount = CountRows(session);
                IDbCommand select = (offset > 0) ? SelectWithOffset(session) : Select(session);

                using (IDataReader reader = select.ExecuteReader())
                {
                    string[] columns = new string[reader.FieldCount];
                    for (int i = 0; i < columns.Length; i++)
                    {
                        columns[i] = reader.GetName(i);
                    }

                    List<object[]> rows = new List<object[]>();
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        for (int i = 0; i < row.Length; i++)
                        {
                            object val = reader.GetValue(i);
                            row[i] = !(val is DBNull) ? val : null;
                        }
                        rows.Add(row);
                    }

                    results.Columns = columns;
                    results.Rows = rows;
                }
                Logger.Debug(results.ToString());
            }
            catch (Exception e)
            {                
                Logger.Error("Could not complete custom query", e);
            }
            finally
            {
                ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(session);
                Logger.Info("Session Released");
            }

            return results;
        }

        private int CountRows(ISession session)
        {
            IDbCommand count = session.Connection.CreateCommand();
            count.CommandText = String.Format("SELECT count(*) AS count_all FROM ({0}) as derived_table", query);
            AddFilters(count);
            Logger.Debug(count.CommandText);
            return ((int)count.ExecuteScalar());
        }

        private IDbCommand Select(ISession session)
        {
            IDbCommand select = session.Connection.CreateCommand();
            select.CommandText = String.Format(
                "SELECT TOP {0} * FROM ({1}) as derived_table",
                limit,
                query
            );
            AddFilters(select);
            Logger.Debug(select.CommandText);
            return select;
        }

        private IDbCommand SelectWithOffset(ISession session)
        {
            IDbCommand select = session.Connection.CreateCommand();
            select.CommandText = String.Format(
                "SELECT TOP {0} * INTO #limit_offset_temp FROM ({1}) as derived_table",
                limit + offset,
                query
            );
            AddFilters(select);
            Logger.Debug(select.CommandText);

            StringBuilder selectBuilder = new StringBuilder(select.CommandText);
            selectBuilder.Append(";\n");

            selectBuilder.AppendFormat("SET ROWCOUNT {0};\n", offset);
            if (Logger.IsDebugEnabled) Logger.Debug("SET ROWCOUNT {0}", offset);

            selectBuilder.Append("DELETE FROM #limit_offset_temp;\n");
            if (Logger.IsDebugEnabled) Logger.Debug("DELETE FROM #limit_offset_temp");

            selectBuilder.Append("SELECT * FROM #limit_offset_temp;\n");
            if (Logger.IsDebugEnabled) Logger.Debug("SELECT * FROM #limit_offset_temp");

            selectBuilder.Append("SET ROWCOUNT 0;\n");
            if (Logger.IsDebugEnabled) Logger.Debug("SET ROWCOUNT 0");

            selectBuilder.Append("DROP TABLE #limit_offset_temp;");
            if (Logger.IsDebugEnabled) Logger.Debug("DROP TABLE #limit_offset_temp");

            select.CommandText = selectBuilder.ToString();
            return select;
        }
    }
}
