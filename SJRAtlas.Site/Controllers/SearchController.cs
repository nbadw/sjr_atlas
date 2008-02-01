using System;
using System.Collections.Generic;
using SJRAtlas.Models;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using SJRAtlas.Models.Atlas;
using SJRAtlas.Models.DataWarehouse;

namespace SJRAtlas.Site.Controllers
{
	public class SearchController : BaseController
	{
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

            PropertyBag["results"] = results;
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
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("q", q);
                RedirectToAction("noresults", parameters);
                return;
            }

            PropertyBag["query"] = q;
            PropertyBag["results"] = results;
        }

        public void NoResults(string q)
        {
            PropertyBag["query"] = q;
            RenderView("no-results");
        }

        public void Advanced()
        {
            IList<Agency> agencies = AtlasMediator.FindAll<Agency>();
            IList<DataSet> datasets = AtlasMediator.FindAll<DataSet>();

            List<string> agencyNames = new List<string>(agencies.Count);
            foreach (Agency agency in agencies)
            {
                if (!String.IsNullOrEmpty(agency.Name))
                    agencyNames.Add(agency.Name);
            }
            agencyNames.Sort();

            PropertyBag["datasets"] = datasets;
            PropertyBag["agencies"] = agencyNames;
            //PropertyBag["optionstype"] = typeof(SearchOptions);
        }

        public void Tips()
        {
            RenderView("tips");
        }

        //public void SubmitAdvanced([DataBind("options", Validate=true)] SearchOptions options)
        //{
        //    if (HasValidationError(options))
        //    {
        //        Flash["options"] = options;
        //        Flash["summary"] = GetErrorSummary(options);
        //        RedirectToAction("advanced");
        //        return;
        //    }

        //    RenderSharedView("shared/todo");
        //}

        private Regex CreateWatershedTriggerRegex()
        {
            return new Regex(@"\swatershed$|\sbasin$|\scatchment$", RegexOptions.IgnoreCase);
        }
	}
}
