using System;
using SJRAtlas.Models.Atlas;
using Castle.MonoRail.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;
using SJRAtlas.Models.Query;

namespace SJRAtlas.Site.Presentations
{
    public class TabularPresentationViewHandler : IPresentationViewHandler
    {
        #region IPresentationViewHandler Members

        public void RenderViewFor(Presentation presentation, IRailsEngineContext context)
        {
            TabularPresentation tabularPresentation = presentation as TabularPresentation;
            Controller controller = context.CurrentController;
            
            List<QueryResults> results = new List<QueryResults>();
            foreach (TabularQuery query in tabularPresentation.GetQueries())
            {
                CustomQuery customQuery = new CustomQuery(query.SelectStatement);
                results.Add(customQuery.LimitResultsTo(15).StartAtRow(30).Execute());
            }

            List<ExtGrid> grids = new List<ExtGrid>();
            foreach (QueryResults result in results)
            {
                grids.Add(new ExtGrid(result));
            }

            controller.PropertyBag["ext_grids"] = grids;
            controller.RenderSharedView("presentation/tables");
        }

        #endregion

        public class ExtGrid
        {
            private readonly QueryResults results;

            public ExtGrid(QueryResults results)
            {
                this.results = results;
            }

            public string Data()
            {
                return JavaScriptConvert.SerializeObject(results.Rows);
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
                }
                return JavaScriptConvert.SerializeObject(columns);
            }

            public List<IDictionary<string, object>> ColumnsToDictionaryArray(string key)
            {
                List<IDictionary<string, object>> columns = new List<IDictionary<string, object>>(results.Columns.Length);
                foreach (string column in results.Columns)
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
