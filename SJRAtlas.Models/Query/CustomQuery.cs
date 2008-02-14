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
                FilterByWaterBodyId(int.Parse(parameters["waterbodyId"]));

            if (!String.IsNullOrEmpty(parameters["agencyCode"]))
                FilterByAgencyCode(parameters["agencyCode"]);

            //if (!String.IsNullOrEmpty(parameters["aquaticSite"]))
            //    filters["aquaticSite"] = parameters["aquaticSite"];

            if (!String.IsNullOrEmpty(parameters["startDate"]))
                FilterByStartDate(DateTime.Parse(parameters["startDate"]));

            if (!String.IsNullOrEmpty(parameters["endDate"]))
                FilterByEndDate(DateTime.Parse(parameters["endDate"]));

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

        public CustomQuery FilterByAgencyCode(string agencyCode)
        {
            return FilterBy("AgencyCd", "=", agencyCode);
        }

        public CustomQuery FilterByStartDate(DateTime startDate)
        {
            return FilterBy("StartDate", ">=", startDate);
        }

        public CustomQuery FilterByEndDate(DateTime endDate)
        {
            return FilterBy("EndDate", "<=", endDate);
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

        #endregion

        public QueryResults Execute()
        {
            Logger.Info("Executing Custom Query");
            Logger.Debug(String.Format("Start Row: {0}, Limit: {1}", offset, limit));
            QueryResults results = new QueryResults();
            ISession session = ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(ActiveRecordBase));
            Logger.Info("Session Acquired");

            try
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    results.TotalRowCount = CountRows(session, transaction);

                    IDbCommand select = (offset > 0) ? SelectWithOffset(session, transaction) : Select(session, transaction);

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

                    if(offset > 0)
                        RemoveTemporaryTable(session, transaction);
                }
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

        private int CountRows(ISession session, ITransaction transaction)
        {
            IDbCommand count = session.Connection.CreateCommand();
            count.CommandText = String.Format("SELECT count(*) AS count_all FROM ({0}) as derived_table", query);
            AddFilters(count);
            Logger.Debug(count.CommandText);
            transaction.Enlist(count);
            return ((int)count.ExecuteScalar());
        }

        private IDbCommand Select(ISession session, ITransaction transaction)
        {
            IDbCommand select = session.Connection.CreateCommand();
            select.CommandText = String.Format(
                "SELECT TOP {0} * FROM ({1}) as derived_table",
                limit,
                query
            );
            AddFilters(select);
            Logger.Debug(select.CommandText);
            transaction.Enlist(select);
            return select;
        }

        private IDbCommand SelectWithOffset(ISession session, ITransaction transaction)
        {
            IDbCommand tempTable = session.Connection.CreateCommand();
            tempTable.CommandText = String.Format(
                "SELECT TOP {0} * INTO #limit_offset_temp FROM ({1}) as derived_table",
                limit + offset,
                query
            );
            AddFilters(tempTable);
            Logger.Debug(tempTable.CommandText);
            transaction.Enlist(tempTable);
            tempTable.ExecuteNonQuery();

            IDbCommand setOffset = session.Connection.CreateCommand();
            setOffset.CommandText = "SET ROWCOUNT " + offset.ToString();
            Logger.Debug(setOffset.CommandText);
            transaction.Enlist(setOffset);
            setOffset.ExecuteNonQuery();

            IDbCommand deleteFrom = session.Connection.CreateCommand();
            deleteFrom.CommandText = "DELETE FROM #limit_offset_temp";
            Logger.Debug(deleteFrom.CommandText);
            transaction.Enlist(deleteFrom);
            deleteFrom.ExecuteNonQuery();

            IDbCommand select = session.Connection.CreateCommand();
            select.CommandText = "SELECT * FROM #limit_offset_temp";
            Logger.Debug(select.CommandText);
            transaction.Enlist(select);
            return select;
        }

        private void RemoveTemporaryTable(ISession session, ITransaction transaction)
        {
            IDbCommand setRowCount = session.Connection.CreateCommand();
            setRowCount.CommandText = "SET ROWCOUNT 0";
            Logger.Debug(setRowCount.CommandText);
            transaction.Enlist(setRowCount);
            setRowCount.ExecuteNonQuery();

            IDbCommand dropTemp = session.Connection.CreateCommand();
            dropTemp.CommandText = "DROP TABLE #limit_offset_temp";
            Logger.Debug(dropTemp.CommandText);
            transaction.Enlist(dropTemp);
            dropTemp.ExecuteNonQuery();
        }
    }
}
