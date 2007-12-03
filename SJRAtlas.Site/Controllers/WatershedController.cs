namespace SJRAtlas.Site.Controllers
{
    using System;

    using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;
    using Castle.Core.Logging;
    using SJRAtlas.Core;
    using System.Reflection;
    using System.Collections;
    using SJRAtlas.DataWarehouse;
    using Castle.ActiveRecord.Queries;

    [Layout("sjratlas"), Rescue("generalerror")]
    public class WatershedController : SJRAtlasController
    {
        //[Cache(System.Web.HttpCacheability.Server, Duration = 300, VaryByParams = "id")]
        public void View(string id)
        {
            View(WatershedLookup.Find(id));
        }

        [Rescue("friendlyerror", typeof(ArgumentNullException))]
        public void View(IWatershed watershed)
        {
            if (watershed == null)
                throw new ArgumentNullException("watershed", "The requested watershed could not be found.");

            if (watershed.CgndbKey != null)
                watershed.PlaceName = PlaceNameLookup.Find(watershed.CgndbKey);

            string metadataQuery = MetadataUtils.BuildDefaultQuery(watershed.Name);
            Logger.Debug("Searching metadata index for " + watershed.Name + " using " + metadataQuery);

            string datasetQuery = MetadataUtils.BuildGetByTitlesQuery(
                AtlasUtils.DataSetTitles(watershed));
            Logger.Debug("Also searching for related datasets using " + datasetQuery);

            IMetadata[] metadata = MetadataLookup.FindByQuery(metadataQuery);
            IMetadata[] datasets = MetadataLookup.FindByQuery(datasetQuery);
            metadata = MergeMetadata(metadata, datasets);

            IEasyMap[] interactiveMaps = GetInteractiveMaps(metadata, AtlasUtils.IsWithinBasin(watershed));
            
            metadata = RemoveEasyMaps(metadata, interactiveMaps);

            PropertyBag["watershed"] = watershed;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["metadata"] = metadata;
        }

        [AjaxAction]
        public IWatershed[] List(string code)
        {      
            if (code == null)
                code = "";

            string tempMatch = String.Format("{0}-%-00-00-00-00-00-", code);
            if (tempMatch.StartsWith("-"))
                tempMatch = tempMatch.Remove(0, 1);

            string match = String.Empty;
            int dashCount = 0;
            while (dashCount < 6)
            {
                int dashIndex = tempMatch.IndexOf("-") + 1;
                match += tempMatch.Substring(0, dashIndex);
                tempMatch = tempMatch.Remove(0, dashIndex);
                dashCount++;
            }
            match = match.Substring(0, match.Length - 1);
            string exclude = match.Replace("%", "00");

            if (match == exclude)
                return new IWatershed[0];

            SimpleQuery<DrainageUnit> q = new SimpleQuery<DrainageUnit>("from DrainageUnit du where du.DrainageCode like ? and du.DrainageCode not like ?", match, exclude);
            IWatershed[] watersheds = q.Execute();
            return watersheds;
        }

        [AjaxAction]
        public IDictionary AutoComplete(string query)
        {
            Logger.Debug(String.Format("XHR call to Watershed Autocomplete for {0} received", query));
            string idQuery = String.Format("{0}%", query);
            string textQuery = String.Format("%{0}%", query);

            Hashtable results = new Hashtable();
            ArrayList resultList = new ArrayList();

            SimpleQuery<DrainageUnit> q = new SimpleQuery<DrainageUnit>("from DrainageUnit du where du.UnitName like ? or du.DrainageCode like ?", textQuery, idQuery);
            foreach (DrainageUnit watershed in q.Execute())
            {
                resultList.Add(watershed);
                if (resultList.Count == 10)
                    break;
            }

            results["Watersheds"] = resultList;
            Logger.Info(String.Format("Autocomplete XHR found {0} matches", resultList.Count));
            return results;
        }

        public void AutoCompleteData()
        {
            ArrayList resultList = new ArrayList();
        
            foreach (DrainageUnit watershed in DrainageUnit.FindAll())
            {
                resultList.Add(String.Format("{0} ({1})", watershed.Name, watershed.DrainageCode));
            }

            CancelLayout();
            Context.Response.ContentType = "text/plain";
            RenderText(Newtonsoft.Json.JavaScriptConvert.SerializeObject(resultList));
        }
    }
}
