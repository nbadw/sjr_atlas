using System;
using SJRAtlas.Models;
using System.Collections.Generic;
using SJRAtlas.Models.DataWarehouse;
using SJRAtlas.Models.Atlas;
using Castle.ActiveRecord.Queries;
using Castle.MonoRail.Framework;
using Newtonsoft.Json;
using NHibernate.Expression;

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

        [AjaxAction]
        public void AutoComplete(string query, int start, int limit)
        {
            Logger.Debug(String.Format("XHR call to Watershed Autocomplete for {0} received", query));
            string idQuery = String.Format("{0}%", query);
            string textQuery = String.Format("%{0}%", query);

            DetachedCriteria criteria = DetachedCriteria.For<Watershed>();
            criteria.Add(Expression.Or(
                Expression.Like("Name", textQuery),
                Expression.Like("DrainageCode", idQuery)
            ));
            Watershed[] results = Watershed.SlicedFindAll(start, limit, criteria,
                Order.Asc("Name"));

            criteria = DetachedCriteria.For<Watershed>();
            criteria.Add(Expression.Or(
                Expression.Like("Name", textQuery),
                Expression.Like("DrainageCode", idQuery)
            ));
            int count = new ScalarProjectionQuery<Watershed, int>(
                NHibernate.Expression.Projections.RowCount(),
                criteria
            ).Execute();

            Context.Response.ContentType = "text/javascript";
            RenderText(String.Format("{{ results: {0}, watersheds: {1} }}", count,
                JavaScriptConvert.SerializeObject(results)));
        }
    }
}
