using System;
using System.Data;
using System.Data.SqlClient;
using NHibernate;
using Castle.ActiveRecord;
using System.Collections.Generic;

namespace SJRAtlas.DataWarehouse
{
    [ActiveRecord("queries", Mutable = false)]
    public class Query : DataWarehouseARBase<Query>
    {
        private int id;

        [PrimaryKey]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string commandText;

        [Property("command_text")]
        public string CommandText
        {
            get { return commandText; }
            set { commandText = value; }
        }

        private DataType datatype;

        [BelongsTo("tabular_id")]
        public DataType DataType
        {
            get { return datatype; }
            set { datatype = value; }
        }

        private int tabularId;
        [Property("tabular_id")]
        public int TabularId
        {
            get { return tabularId; }
            set { tabularId = value; }
        }

        public static Query[] FindAllByTabularResourceId(int id)
        {
            return Query.FindAllByProperty("TabularId", id);
        }

        private void Execute()
        {
            if (CommandText == null)
                throw new Exception("Query Object Was Not Properly Created");         

            // execute query            
            ISession session = holder.CreateSession(typeof(Query));
            try
            {
                IDbCommand sql = session.Connection.CreateCommand();
                sql.CommandText = CommandText;
                using (IDataReader reader = sql.ExecuteReader())
                {
                    List<object[]> rows = new List<object[]>();
                    while (reader.Read())
                    {
                        columnNames = new string[reader.FieldCount];
                        object[] row = new object[reader.FieldCount];
                        for (int i = 0; i < row.Length; i++)
                        {
                            if (String.IsNullOrEmpty(columnNames[i]))
                                columnNames[i] = reader.GetName(i);
                            row[i] = reader.GetValue(i);
                        }
                        rows.Add(row);
                    }
                    tableRecords = rows.ToArray(); 
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(session);
            }
        }

        private string[] columnNames;

        public string[] ColumnNames
        {
            get
            {
                // lazy load
                if (columnNames == null)
                    Execute();

                return columnNames;
            }
        }

        private object[] tableRecords;

        public object[] QueryRecords
        {
            get
            {
                // lazy load
                if (tableRecords == null)
                    Execute();

                return tableRecords;
            }
        }
    }
}
