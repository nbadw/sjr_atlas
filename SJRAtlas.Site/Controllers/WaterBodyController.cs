using System;
using System.Collections.Generic;
using Castle.MonoRail.Framework;
using System.Collections;
using Castle.ActiveRecord.Queries;
using SJRAtlas.Models.DataWarehouse;
using SJRAtlas.Models.Atlas;
using Newtonsoft.Json;

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

        [AjaxAction]
        public void AutoComplete(string query)
        {
            string idQuery = String.Format("{0}%", query);
            string textQuery = String.Format("%{0}%", query);                        

            SimpleQuery<WaterBody> q = new SimpleQuery<WaterBody>("from WaterBody wb where wb.Name like ? or wb.Id like ? or wb.Abbreviation like ?", textQuery, idQuery, textQuery);                        
            WaterBody[] results = q.Execute();

            List<WaterBodyAttributes> waterbodies = new List<WaterBodyAttributes>(results.Length);
            foreach(WaterBody waterbody in results)
            {
                waterbodies.Add(new WaterBodyAttributes(waterbody));
            }

            PropertyBag["waterbodies"] = JavaScriptConvert.SerializeObject(waterbodies);
            PropertyBag["resultsCount"] = waterbodies.Count;
            CancelLayout();
            Context.Response.ContentType = "text/javascript";
        }

        public class WaterBodyAttributes : Dictionary<string, object>
        {
            public WaterBodyAttributes(WaterBody waterbody)
                : base()
            {
                this["id"] = waterbody.Id;
                this["name"] = waterbody.Name;
                this["alt_name"] = waterbody.AltName;
            }
        }
    }
}
