using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Castle.ActiveRecord;
using NHibernate;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("web_tabular_queries")]
    public class TabularQuery : ActiveRecordBase<TabularQuery>
    {
        public static IList<TabularQuery> FindQueriesForTabularPresentation(int presentationId)
        {
            return TabularQuery.FindAllByProperty("PresentationId", presentationId);
        }

        #region ActiveRecord Properties

        private string selectStatement;

        private int id;

        [PrimaryKey("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        [Property("select_statement")]
        public string SelectStatement
        {
            get { return selectStatement; }
            set { selectStatement = value; }
        }

        private int presentationId;

        [Property("presentation_id")]
        public int PresentationId
        {
            get { return presentationId; }
            set { presentationId = value; }
        }

        private string title;

        [Property("title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
                
        #endregion

        public string[] ColumnNames()
        {
            ISession session = ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(typeof(TabularQuery));
            try
            {
                IDbCommand command = session.Connection.CreateCommand();
                command.CommandText = String.Format(
                    "SELECT TOP 1 * FROM ({0}) as temp_table", 
                    SelectStatement
                );
                string[] columnNames;
                using (IDataReader reader = command.ExecuteReader())
                {
                    columnNames = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columnNames[i] = reader.GetName(i);
                    }
                }

                return columnNames;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(session);
            }
        }
    }
}