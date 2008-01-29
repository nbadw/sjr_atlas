using System;
using System.Collections.Generic;
using Castle.MonoRail.Framework;
using System.Collections;
using Castle.ActiveRecord.Queries;
using SJRAtlas.Models;

namespace SJRAtlas.Site.Controllers
{
    public class WaterBodyController : BaseController
    {
        public void View(string cgndbKey)
        {
            if (cgndbKey == null)
                throw new ArgumentNullException("cgndbKey");

            WaterBody waterbody = WaterBody.FindByCgndbKeyOrAltCgndbKey(cgndbKey);
            IList<InteractiveMap> interactiveMaps = waterbody.RelatedInteractiveMaps;
            IList<Publication> publications = waterbody.RelatedPublications; 
            IList<DataSet> datasets = waterbody.DataSets;

            PropertyBag["waterbody"] = waterbody;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["published_maps"] = GetPublicationsByType<PublishedMap>(publications);
            PropertyBag["published_reports"] = GetPublicationsByType<PublishedReport>(publications);
            PropertyBag["datasets"] = datasets;
        }

        //[AjaxAction]
        //public IDictionary AutoComplete(string query)
        //{
        //    string idQuery = String.Format("{0}%", query);
        //    string textQuery = String.Format("%{0}%", query);

        //    Hashtable results = new Hashtable();
        //    ArrayList resultList = new ArrayList();

        //    SimpleQuery<WaterBody> q = new SimpleQuery<WaterBody>("from WaterBody wb where wb.Name like ? or wb.Id like ? or wb.Abbreviation like ?", textQuery, idQuery, textQuery);            
        //    foreach (WaterBody waterbody in q.Execute())
        //    {
        //        resultList.Add(waterbody);
        //        if (resultList.Count == 10)
        //            break;
        //    }

        //    results["WaterBodies"] = resultList;
        //    return results;
        //}

        //public void AutoCompleteData()
        //{
        //    ArrayList resultList = new ArrayList();

        //    foreach (WaterBody waterbody in WaterBody.FindAll())
        //    {
        //        string data = String.Format("{0} - {1}", waterbody.Id, waterbody.Name);
        //        if (waterbody.AltName != null)
        //            data += " (" + waterbody.AltName + ")";
        //        resultList.Add(data);
        //    }

        //    CancelLayout();
        //    Context.Response.ContentType = "text/plain";
        //    RenderText(Newtonsoft.Json.JavaScriptConvert.SerializeObject(resultList));
        //}
    }
}
