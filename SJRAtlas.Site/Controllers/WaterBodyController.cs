using System;
using System.Collections.Generic;
using SJRAtlas.Core;
using Castle.MonoRail.Framework;
using SJRAtlas.DataWarehouse;
using System.Collections;
using Castle.ActiveRecord.Queries;

namespace SJRAtlas.Site.Controllers
{
    [Layout("sjratlas"), Rescue("friendlyerror")]
    public class WaterBodyController : SJRAtlasController
    {
        public void View(string placename)
        {
            View(AtlasUtils.CreateWaterBodyFromPlaceName(PlaceNameLookup.Find(placename)));
        }

        public void View(IWaterBody waterbody)
        {
            if(waterbody == null)
                throw new ArgumentNullException("waterbody", "The requested waterbody could not be found.");
            
            string metadataQuery = MetadataUtils.BuildDefaultQuery(waterbody.Name);
            Logger.Debug("Searching metadata index for " + waterbody.Name + " using " + metadataQuery);

            string datasetQuery = MetadataUtils.BuildGetByTitlesQuery(
                AtlasUtils.DataSetTitles(waterbody));
            Logger.Debug("Also searching for related datasets using " + datasetQuery);

            IMetadata[] metadata = MetadataLookup.FindByQuery(metadataQuery);
            IMetadata[] datasets = MetadataLookup.FindByQuery(datasetQuery);
            metadata = MergeMetadata(metadata, datasets);

            IEasyMap[] interactiveMaps = GetInteractiveMaps(metadata, true);

            metadata = RemoveEasyMaps(metadata, interactiveMaps);
           
            PropertyBag["waterbody"] = waterbody;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["metadata"] = metadata;
        }

        [AjaxAction]
        public IDictionary AutoComplete(string query)
        {
            string idQuery = String.Format("{0}%", query);
            string textQuery = String.Format("%{0}%", query);

            Hashtable results = new Hashtable();
            ArrayList resultList = new ArrayList();

            SimpleQuery<WaterBody> q = new SimpleQuery<WaterBody>("from WaterBody wb where wb.Name like ? or wb.Id like ? or wb.Abbreviation like ?", textQuery, idQuery, textQuery);            
            foreach (WaterBody waterbody in q.Execute())
            {
                resultList.Add(waterbody);
                if (resultList.Count == 10)
                    break;
            }

            results["WaterBodies"] = resultList;
            return results;
        }

        public void AutoCompleteData()
        {
            ArrayList resultList = new ArrayList();

            foreach (WaterBody waterbody in WaterBody.FindAll())
            {
                string data = String.Format("{0} - {1}", waterbody.Id, waterbody.Name);
                if (waterbody.AltName != null)
                    data += " (" + waterbody.AltName + ")";
                resultList.Add(data);
            }

            CancelLayout();
            Context.Response.ContentType = "text/plain";
            RenderText(Newtonsoft.Json.JavaScriptConvert.SerializeObject(resultList));
        }
    }
}
