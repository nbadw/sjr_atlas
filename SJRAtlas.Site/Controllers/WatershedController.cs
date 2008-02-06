using System;
using SJRAtlas.Models;
using System.Collections.Generic;
using SJRAtlas.Models.DataWarehouse;
using SJRAtlas.Models.Atlas;
using Castle.ActiveRecord.Queries;
using Castle.MonoRail.Framework;
using Newtonsoft.Json;

namespace SJRAtlas.Site.Controllers
{
    public class WatershedController : BaseController
    {
        public void View(string cgndbKey, string drainageCode)
        {
            if (String.IsNullOrEmpty(cgndbKey) && String.IsNullOrEmpty(drainageCode))
                throw new ArgumentException(@"Action: View on Controller: Watershed must be passed
                    either a CGNDB Key or a Drainage Unit Code");

            Watershed watershed = String.IsNullOrEmpty(drainageCode) ?
                AtlasMediator.FindWatershedByCgndbKey(cgndbKey) :
                AtlasMediator.Find<Watershed>(drainageCode);
            IList<InteractiveMap> interactiveMaps = watershed.RelatedInteractiveMaps;
            IList<Publication> publications = watershed.RelatedPublications;
            
            PropertyBag["watershed"] = watershed;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["published_maps"] = GetPublicationsByType<PublishedMap>(publications);
            PropertyBag["published_reports"] = GetPublicationsByType<PublishedReport>(publications);
            PropertyBag["datasets"] = watershed.DataSets;
        }

        //[AjaxAction]
        //public IWatershed[] List(string code)
        //{      
        //    if (code == null)
        //        code = "";

        //    string tempMatch = String.Format("{0}-%-00-00-00-00-00-", code);
        //    if (tempMatch.StartsWith("-"))
        //        tempMatch = tempMatch.Remove(0, 1);

        //    string match = String.Empty;
        //    int dashCount = 0;
        //    while (dashCount < 6)
        //    {
        //        int dashIndex = tempMatch.IndexOf("-") + 1;
        //        match += tempMatch.Substring(0, dashIndex);
        //        tempMatch = tempMatch.Remove(0, dashIndex);
        //        dashCount++;
        //    }
        //    match = match.Substring(0, match.Length - 1);
        //    string exclude = match.Replace("%", "00");

        //    if (match == exclude)
        //        return new IWatershed[0];

        //    SimpleQuery<DrainageUnit> q = new SimpleQuery<DrainageUnit>("from DrainageUnit du where du.DrainageCode like ? and du.DrainageCode not like ?", match, exclude);
        //    IWatershed[] watersheds = q.Execute();
        //    return watersheds;
        //}

        [AjaxAction]
        public void AutoComplete(string query)
        {
            Logger.Debug(String.Format("XHR call to Watershed Autocomplete for {0} received", query));
            string idQuery = String.Format("{0}%", query);
            string textQuery = String.Format("%{0}%", query);

            SimpleQuery<Watershed> q = new SimpleQuery<Watershed>("from Watershed ws where ws.Name like ? or ws.DrainageCode like ?", textQuery, idQuery);
            Watershed[] results = q.Execute();

            List<WatershedAttributes> watersheds = new List<WatershedAttributes>(results.Length);
            foreach (Watershed waterbody in results)
            {
                watersheds.Add(new WatershedAttributes(waterbody));
            }

            PropertyBag["watersheds"] = JavaScriptConvert.SerializeObject(watersheds);
            PropertyBag["resultsCount"] = watersheds.Count;
            CancelLayout();
            Context.Response.ContentType = "text/javascript";
        }

        public class WatershedAttributes : Dictionary<string, object>
        {
            public WatershedAttributes(Watershed watershed)
                : base()
            {
                this["drainage_code"] = watershed.DrainageCode;
                this["name"] = watershed.Name;
            }
        }
    }
}
