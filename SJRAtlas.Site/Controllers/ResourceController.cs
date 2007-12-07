using System;
using Castle.MonoRail.Framework;

namespace SJRAtlas.Site.Controllers
{
	[Layout("sjratlas"), Rescue("generalerror")]    
    public class ResourceController : SJRAtlasController
    {
        //public ResourceController(string mapApplication, string summaryPath)
        //{
        //    this.mapApplication = mapApplication;
        //    this.summaryPath = summaryPath;
        //}

        //private string mapApplication;

        //public string MapApplication
        //{
        //    get { return mapApplication; }
        //    set { mapApplication = value; }
        //}

        //private string summaryPath;

        //public string SummaryPath
        //{
        //    get { return summaryPath; }
        //    set { summaryPath = value; }
        //}
	

        //public void Spatial(string map)
        //{
        //    string redirectTo = String.Format("{0}?resource={1}", MapApplication, map);
        //    Redirect(redirectTo);
        //}
        
        //public void Tabular(int id)
        //{
        //    //SJRAtlas.DataWarehouse.DataType tabular = SJRAtlas.DataWarehouse.DataType.Find(id);
        //    //PropertyBag["name"] = tabular.Name;
        //    //PropertyBag["queries"] = tabular.Queries;   
         
        //    DataWarehouse.Query[] queries = DataWarehouse.Query.FindAllByTabularResourceId(id);
        //    PropertyBag["queries"] = queries;
        //}
        
        //public void Graph()
        //{
            
        //}

        //public void Summary(string report)
        //{
        //    string redirectTo = String.Format("{0}{1}", SummaryPath, report);
        //    Redirect(redirectTo);
        //}
    }
}
