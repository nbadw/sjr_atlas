using System;
using SJRAtlas.Models.Atlas;
using Castle.MonoRail.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SJRAtlas.Site.Presentations
{
    public class TabularPresentationViewHandler : IPresentationViewHandler
    {
        #region IPresentationViewHandler Members

        public void RenderViewFor(Presentation presentation, IRailsEngineContext context)
        {
            TabularPresentation tabularPresentation = presentation as TabularPresentation;
            Controller controller = context.CurrentController;

            IList<Table> tables = tabularPresentation.GetTables();
            IList<ExtGrid> extGrids = new List<ExtGrid>(tables.Count);
            foreach (Table table in tables)
            {
                if (!String.IsNullOrEmpty(context.Params["drainageCode"]))
                {
                    table.FilterByDrainageCode(context.Params["drainageCode"]);
                }

                if (!String.IsNullOrEmpty(context.Params["waterbodyId"]))
                {
                    try
                    {
                        table.FilterByWaterBodyId(int.Parse(context.Params["waterbodyId"]));
                    }
                    catch (Exception e)
                    {
                        controller.Logger.Error("Exception in TabularPresentationViewHandler", e);
                    }
                }
                    
                extGrids.Add(new ExtGrid(table));
            }

            controller.PropertyBag["ext_grids"] = extGrids;
            controller.RenderSharedView("presentation/tables");
        }

        #endregion

        public class ExtGrid
        {
            private readonly Table table;

            public ExtGrid(Table table)
            {
                this.table = table;
            }

            public string Data()
            {
                List<List<object>> data = new List<List<object>>(table.Rows.Count);
                foreach (object[] row in table.Rows)
                {
                    List<object> rowData = new List<object>(row.Length);
                    foreach (object obj in row)
                    {
                        rowData.Add(!(obj is DBNull) ? obj : null);
                    }
                    data.Add(rowData);
                }
                return JavaScriptConvert.SerializeObject(data);
            }

            public string Fields()
            {
                return JavaScriptConvert.SerializeObject(ColumnsToDictionaryArray("name"));
            }

            public string Columns()
            {
                List<IDictionary<string, object>> columns = ColumnsToDictionaryArray("header");
                foreach (IDictionary<string, object> dict in columns)
                {
                    dict.Add("dataIndex", dict["header"]);
                    dict.Add("sortable", true);
                }
                return JavaScriptConvert.SerializeObject(columns);
            }

            public List<IDictionary<string, object>> ColumnsToDictionaryArray(string key)
            {
                List<IDictionary<string, object>> columns = new List<IDictionary<string, object>>(table.Columns.Length);
                foreach (string column in table.Columns)
                {
                    Dictionary<string, object> field = new Dictionary<string, object>();
                    field.Add(key, column);
                    columns.Add(field);
                }
                return columns;
            }

        }
    }
}
