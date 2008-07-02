using System;
using System.Collections.Generic;
using SJRAtlas.Models;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using SJRAtlas.Models.Atlas;
using SJRAtlas.Models.DataWarehouse;
using Newtonsoft.Json;

namespace SJRAtlas.Site.Controllers
{
	public class SearchController : BaseController
	{
        public void Index()
        {

        }

        public void Quick(string q)
        {
            Regex triggerRegex = CreateWatershedTriggerRegex();
            if (triggerRegex.IsMatch(q.Trim()))
            {
                Logger.Debug("Quick Search is redirecting to the Watershed Search");
                q = triggerRegex.Replace(q, "");
                RedirectToAction("watersheds", "q=" + q);
                return;
            }
            else
            {
                Logger.Debug("Quick Search is redirecting to the Gazetteer Search");
                RedirectToAction("places", "q=" + q);
                return;
            }
        }

        public void Places(string q)
        {
            IList<Place> results = AtlasMediator.FindAllPlacesByQuery(q);

            if (results.Count == 0)
            {
                RedirectToAction("watersheds", "q=" + q);
                return;
            }
            else if (results.Count == 1)
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("cgndbKey", results[0].CgndbKey);
                Redirect("", "place", "view", parameters);
                return;
            }

            List<PlaceAttributes> resultAttributes = new List<PlaceAttributes>(results.Count);
            foreach (Place result in results)
            {
                NameValueCollection queryParams = new NameValueCollection(1);
                queryParams["cgndbKey"] = result.CgndbKey;
                PlaceAttributes attributes = new PlaceAttributes(result);
                attributes["url"] = UrlBuilder.BuildUrl(
                    Context.UrlInfo, "place", "view", queryParams);
                resultAttributes.Add(attributes);
            }

            PropertyBag["results_raw"] = results; // for use if javascript is not enabled
            PropertyBag["results"] = JavaScriptConvert.SerializeObject(resultAttributes);
            PropertyBag["query"] = q;
            RenderView("places");
        }

        public void Watersheds(string q)
        {
            IList<Watershed> results = AtlasMediator.FindAllWatershedsByQuery(q);

            if (results.Count == 1)
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("drainageCode", results[0].DrainageCode);
                Redirect("", "watershed", "view", parameters);
                return;
            }

            if(results.Count == 0)
            {
                RedirectToAction("all", "q=" + q);
                return;               
            }

            PropertyBag["query"] = q;
            PropertyBag["results"] = results;
        }

        public void All(string q)
        {
            IList<DataSet> datasets = AtlasMediator.FindAllDataSetsByQuery(q);
            IList<InteractiveMap> interactiveMaps = AtlasMediator.FindAllInteractiveMapsByQuery(q);
            IList<Publication> publications = AtlasMediator.FindAllPublicationsByQuery(q);
            
            if (datasets.Count == 0 && interactiveMaps.Count == 0 && publications.Count == 0)
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("q", q);
                RedirectToAction("noresults", parameters);
                return;
            }

            PropertyBag["query"] = q;
            PropertyBag["datasets"] = datasets;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["published_maps"] = GetPublicationsByType<PublishedMap>(publications);
            PropertyBag["published_reports"] = GetPublicationsByType<PublishedReport>(publications);

        }

        public void NoResults(string q)
        {
            PropertyBag["query"] = q;
            RenderView("no-results");
        }

        public void Advanced(int dataSetId, string agencyCode, string drainageCode,
            int waterbodyId, int aquaticSiteId, DateTime startDate, DateTime endDate)
        {            
            DataSet dataset = AtlasMediator.Find<DataSet>(dataSetId);
            foreach (Presentation presentation in dataset.Presentations)
            {
                if (presentation is TabularPresentation)
                {
                    Redirect("presentation", "view",
                        Castle.MonoRail.Framework.Helpers.DictHelper.Create(
                            "id=" + presentation.Id,
                            "agencyCode=" + agencyCode,
                            "drainageCode=" + drainageCode,
                            "waterbodyId=" + waterbodyId,
                            "aquaticSiteId" + aquaticSiteId,
                            "startDate=" + startDate,
                            "endDate=" + endDate
                    ));
                    return;
                }
            }

            throw new Exception("Dataset has no tabular information");
        }

        public void Tips()
        {
            RenderView("tips");
        }

        private Regex CreateWatershedTriggerRegex()
        {
            return new Regex(@"\swatershed$|\sbasin$|\scatchment$", RegexOptions.IgnoreCase);
        }

        public class PlaceAttributes : Dictionary<string, object>
        {
            public PlaceAttributes(Place place) : base()
            {                
                this["name"] = place.Name;
                this["cgndb_key"] = place.CgndbKey;
                this["region"] = place.Region;
                this["status_term"] = place.NameStatus;
                this["type"] = place.ConciseTerm;
                this["county"] = place.County;
                this["latitude"] = place.Latitude;
                this["longitude"] = place.Longitude;
            }
        }
	}
}
