using System;
using SJRAtlas.Models.Atlas;
using Castle.MonoRail.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;
using SJRAtlas.Models.Query;
using Castle.MonoRail.Framework.Helpers;
using System.Collections;
using System.Collections.Specialized;

namespace SJRAtlas.Site.Presentations
{
    public class TabularPresentationViewHandler : IPresentationViewHandler
    {
        #region IPresentationViewHandler Members

        public void RegisterDynamicActions(Controller controller)
        {
            controller.DynamicActions["table"] = new TableDynamicAction();
        }

        public void RenderViewFor(Presentation presentation, IRailsEngineContext context)
        {
            TabularPresentation tabularPresentation = presentation as TabularPresentation;
            Controller controller = context.CurrentController;
            
            // configuration as json output = { baseParams: {...}, tables: [...] }
            Hashtable configuration = new Hashtable(2);
            configuration["filters"] = CollectFilterParameters(controller.Params);
            configuration["tables"] = new ArrayList();
             
            foreach (TabularQuery table in tabularPresentation.GetQueries())
            {
                Hashtable tableConfig = new Hashtable();
                tableConfig["url"] = controller.UrlBuilder.BuildUrl(
                    context.UrlInfo,
                    controller.Name,
                    "table",
                    DictHelper.Create("table=" + table.Id.ToString())
                );
                tableConfig["columns"] = table.ColumnNames();
                ((ArrayList)configuration["tables"]).Add(tableConfig);
            }

            controller.PropertyBag["configuration"] = JavaScriptConvert.SerializeObject(configuration);
            controller.RenderSharedView("presentation/tables");
        }

        private Hashtable CollectFilterParameters(NameValueCollection parameters)
        {
            Hashtable filters = new Hashtable();
            
            if(!String.IsNullOrEmpty(parameters["drainageCode"]))
                filters["drainageCode"] = parameters["drainageCode"];

            if (!String.IsNullOrEmpty(parameters["waterbodyId"]))
                filters["waterbodyId"] = parameters["waterbodyId"];

            if (!String.IsNullOrEmpty(parameters["agencyCode"]))
                filters["agencyCode"] = parameters["agencyCode"];

            if (!String.IsNullOrEmpty(parameters["aquaticSite"]))
                filters["aquaticSite"] = parameters["aquaticSite"];

            if (!String.IsNullOrEmpty(parameters["startDate"]))
                filters["startDate"] = parameters["startDate"];

            if (!String.IsNullOrEmpty(parameters["endDate"]))
                filters["endDate"] = parameters["endDate"];

            return filters;
        }

        #endregion

        public class TableDynamicAction : IDynamicAction
        {
            #region IDynamicAction Members

            public void Execute(Controller controller)
            {
                int start = int.Parse(controller.Params["start"]);
                int limit = int.Parse(controller.Params["limit"]);
                                
                TabularQuery query = TabularQuery.Find(int.Parse(controller.Params["table"]));
                CustomQuery customQuery = new CustomQuery(query.SelectStatement);
                customQuery.Logger = GlobalApplication.CreateLogger(typeof(CustomQuery));

                QueryResults results = customQuery
                    .StartAtRow(start)
                    .LimitResultsTo(limit)
                    .ConfigureFilters(controller.Params)
                    .Execute();

                ArrayList rows = new ArrayList(results.Rows.Count);
                foreach (object[] row in results.Rows)
                {
                    Hashtable attributes = new Hashtable(results.Columns.Length);
                    for (int i = 0; i < results.Columns.Length; i++)
                    {
                        attributes[results.Columns[i]] = row[i];
                    }
                    rows.Add(attributes);
                }

                controller.Context.Response.ContentType = "text/javascript";
                controller.RenderText(
                    "{ results: " +
                    results.TotalRowCount.ToString() + 
                    ", rows: " + 
                    JavaScriptConvert.SerializeObject(rows) + 
                    " }"
                );
            }

            #endregion
        }

    }
}
