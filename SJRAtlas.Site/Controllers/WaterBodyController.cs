using System;
using System.Collections.Generic;
using Castle.MonoRail.Framework;
using System.Collections;
using Castle.ActiveRecord.Queries;
using SJRAtlas.Models.DataWarehouse;
using SJRAtlas.Models.Atlas;
using Newtonsoft.Json;
using NHibernate.Expression;

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
        public void AutoComplete(string query, int start, int limit)
        {
            string idQuery = String.Format("{0}%", query);
            string textQuery = String.Format("%{0}%", query);  

            DetachedCriteria criteria = DetachedCriteria.For<WaterBody>();
            criteria.Add(Expression.Or(
                Expression.Or(
                    Expression.Like("Name", textQuery),
                    Expression.Like("Abbreviation", textQuery)),                
                Expression.Sql("WaterBodyID LIKE '" + idQuery + "'")
            ));
            WaterBody[] results = WaterBody.SlicedFindAll(start, limit, criteria,
                Order.Asc("Name"));

            criteria = DetachedCriteria.For<WaterBody>();
            criteria.Add(Expression.Or(
                Expression.Or(
                    Expression.Like("Name", textQuery),
                    Expression.Like("Abbreviation", textQuery)),
                Expression.Sql("WaterBodyID LIKE '" + idQuery + "'")
            ));
            int count = new ScalarProjectionQuery<WaterBody, int>(
                NHibernate.Expression.Projections.RowCount(),
                criteria
            ).Execute();

            Context.Response.ContentType = "text/javascript";
            RenderText(String.Format("{{ results: {0}, waterbodies: {1} }}", count,
                JavaScriptConvert.SerializeObject(results)));
        }
    }
}
