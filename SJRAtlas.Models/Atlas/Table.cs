using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Castle.ActiveRecord;
using NHibernate;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("tabular_queries")]
    public class Table : ActiveRecordBase<Table>
    {
        public static IList<Table> FindTablesForTabularPresentation(int presentationId)
        {
            return Table.FindAllByProperty("PresentationId", presentationId);
        }

        #region Properties

        #region ActiveRecord
        private string defaultSelectStatement;

        private int id;

        [PrimaryKey("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        [Property("select_statement")]
        public string DefaultSelectStatement
        {
            get { return defaultSelectStatement; }
            set 
            { 
                defaultSelectStatement = value;
                NeedsLoading = true;
            }
        }

        private int presentationId;

        [Property("presentation_id")]
        public int PresentationId
        {
            get { return presentationId; }
            set { presentationId = value; }
        }
        
        #endregion

        private string[] columns;
       
        public string[] Columns
        {
            get 
            {
                if (NeedsLoading)
                    LoadFromDatabase();

                return columns; 
            }
        }

        private IList<object[]> rows;

        public IList<object[]> Rows
        {
            get 
            {
                if (NeedsLoading)
                    LoadFromDatabase();

                return rows; 
            }
        }

        private bool needsLoading = true;

        protected bool NeedsLoading
        {
            get { return needsLoading; }
            set { needsLoading = value; }
        }

        #endregion

        #region Filters

        private IList<Filter> filters = new List<Filter>();

        public void AddFilter(Filter filter)
        {
            filters.Add(filter);
        }

        public Table FilterBy(string column, string comparison, object value)
        {
            AddFilter(new Filter(column, comparison, value));
            return this;
        }

        public Table FilterByDrainageCode(string drainageCode)
        {
            return FilterBy("DrainageCd", "=", drainageCode);
        }

        public Table FilterByWaterBodyId(int waterbodyId)
        {
            return FilterBy("WaterBodyID", "=", waterbodyId);
        }

        public Table FilterByAgencyCode(string agencyCode)
        {
            return FilterBy("AgencyCd", "=", agencyCode);
        }

        public Table FilterByStartDate(DateTime startDate)
        {
            return FilterBy("StartDate", ">=", startDate);
        }

        public Table FilterByEndDate(DateTime endDate)
        {
            return FilterBy("EndDate", "<=", endDate);
        }

        #endregion

        protected virtual void LoadFromDatabase()
        {
            ISession session = holder.CreateSession(typeof(Table));
            IDbCommand command = session.Connection.CreateCommand();
            ConfigureCommand(command);
            try
            {
                using (IDataReader reader = command.ExecuteReader())
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
                            row[i] = reader.GetValue(i);
                        }
                        rows.Add(row);
                    }

                    this.columns = columns;
                    this.rows = rows;
                    NeedsLoading = false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(
                    String.Format(
                        "The table could not be loading using the supplied SQL: {0}",
                        command.CommandText),
                    e);
            }
            finally
            {
                ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(session);
            }
        }

        protected virtual void ConfigureCommand(IDbCommand command)
        {
            if (filters.Count == 0)
            {
                command.CommandText = DefaultSelectStatement;
                return;
            }

            StringBuilder filteredSql = new StringBuilder();
            filteredSql.AppendFormat("SELECT * FROM ({0}) AS inner_table WHERE (", DefaultSelectStatement);
            foreach (Filter filter in filters)
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

        public class Filter
        {
            public Filter(string column, string comparison, object value)
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
}
