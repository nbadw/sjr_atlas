using System;
using SJRAtlas.Models.Atlas;
using Castle.MonoRail.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;
using SJRAtlas.Models.Query;
using Castle.MonoRail.Framework.Helpers;
using System.Collections;
using System.Collections.Specialized;
using Microsoft.Office.Interop.Excel;
using IFilter = Castle.MonoRail.Framework.IFilter;
using Filter = Castle.MonoRail.Framework.Filter;
using System.IO;
using Castle.Core.Logging;
using SJRAtlas.Models;
using System.Runtime.InteropServices;
using System.Reflection;

namespace SJRAtlas.Site.Presentations
{
    public class TabularPresentationViewHandler : IPresentationViewHandler
    {
        #region IPresentationViewHandler Members

        public void RegisterDynamicActions(Controller controller)
        {
            controller.DynamicActions["table"] = new TableDynamicAction();
            controller.DynamicActions["download"] = new DownloadDynamicAction();
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
                tableConfig["download"] = controller.UrlBuilder.BuildUrl(
                    context.UrlInfo,
                    controller.Name,
                    "download",
                    DictHelper.Create("table=" + table.Id.ToString())
                );
                tableConfig["title"] = table.Title;
                tableConfig["columns"] = table.ColumnNames();
                ((ArrayList)configuration["tables"]).Add(tableConfig);
            }

            controller.PropertyBag["presentation"] = tabularPresentation;
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

            if (!String.IsNullOrEmpty(parameters["aquaticSiteId"]))
                filters["aquaticSiteId"] = parameters["aquaticSiteId"];

            if (!String.IsNullOrEmpty(parameters["startDate"]))
                filters["startDate"] = parameters["startDate"];

            if (!String.IsNullOrEmpty(parameters["endDate"]))
                filters["endDate"] = parameters["endDate"];

            return filters;
        }

        #endregion

        #region TableDynamicAction

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

        #endregion

        #region DownloadDynamicAction

        public class DownloadDynamicAction : IDynamicAction
        {
            private ILogger logger;

            public DownloadDynamicAction()
            {
                this.logger = GlobalApplication.CreateLogger(typeof(DownloadDynamicAction));
            }

            #region IDynamicAction Members

            public void Execute(Controller controller)
            {
                string filename = Path.GetTempFileName();
                logger.Info("Saving tabular data into " + filename);
                controller.PropertyBag["filename"] = filename;

                TabularQuery query = TabularQuery.Find(int.Parse(controller.Params["table"]));
                CustomQuery customQuery = new CustomQuery(query.SelectStatement);
                customQuery.Logger = GlobalApplication.CreateLogger(typeof(CustomQuery));

                QueryResults results = customQuery
                    .ConfigureFilters(controller.Params)
                    .Execute();

                /*
                 * IMPORTANT: To make sure the Excel applicaton quits properly follow the below steps:
                 * SEE: http://support.microsoft.com/default.aspx?scid=kb;en-us;317109
                 *  
                 * 1. each COM object should be a new variable
                 * 2. use System.Runtime.InteropServices.Marshal.ReleaseComObject when finished with an object
                 * 3. set variable to null
                 * 4. call the Quit method of the application
                 * 5. run garbage collection methods 
                 */

                logger.Debug("initializing Excel application");
                Application excel = new Application();
                Workbooks books = excel.Workbooks;
                Workbook book = books.Add(Missing.Value) as Workbook;
                Sheets sheets = book.Worksheets as Sheets;
                Worksheet sheet;

                logger.Debug("removing unneeded sheets");
                while (sheets.Count > 1)
                {
                    sheet = book.ActiveSheet as Worksheet;
                    sheet.Delete();
                }

                sheet = book.ActiveSheet as Worksheet;
                sheet.Name = query.Title;

                // first row is column names
                for (int i = 0; i < results.Columns.Length; i++)
                {
                    ((Range)sheet.Cells[1, i + 1]).Font.Bold = true;
                    sheet.Cells[1, i + 1] = results.Columns[i];                   
                }

                // subsequent rows are values
                for (int i = 0; i < results.Rows.Count; i++)
                {
                    object[] row = results.Rows[i];
                    for (int j = 0; j < row.Length; j++)
                    {
                        sheet.Cells[i + 2, j + 1] = row[j];
                    }
                }

                logger.Debug("saving excel workbook");
                book.SaveCopyAs(filename);
                book.Saved = true;
                books.Close();

                logger.Debug("cleaning up COM objects");
                Marshal.ReleaseComObject(sheet);
                Marshal.ReleaseComObject(sheets);
                Marshal.ReleaseComObject(book);
                Marshal.ReleaseComObject(books);

                sheet = null;
                sheets = null;
                book = null;
                books = null;

                logger.Debug("closing excel application");
                excel.Quit();

                logger.Debug("final clean up and freeing of resources");
                Marshal.ReleaseComObject(excel);
                excel = null;

                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);

                logger.Debug("sending content");
                controller.CancelView();
                controller.CancelLayout();

                IResponse response = controller.Context.Response;
                string attachment = String.Format("attachment; filename=\"{0}.xls\"", query.Title);
                string contentLength = new FileInfo(filename).Length.ToString();

                response.Clear();
                response.ContentType = "application/excel";
                response.AppendHeader("Content-Disposition", attachment);
                response.AppendHeader("Content-Length", contentLength);
                response.WriteFile(filename);
            }

            #endregion
        }

        #endregion
    }
    
}
