using System;
using Castle.MonoRail.Framework;
using System.Collections.Generic;
using SJRAtlas.Models.DataWarehouse;
using NHibernate.Expression;
using Castle.ActiveRecord.Queries;
using Newtonsoft.Json;

namespace SJRAtlas.Site.Controllers
{
    public class AquaticSiteController : BaseController
    {
        [AjaxAction]
        public void AutoComplete(string query, int start, int limit)
        {
            Logger.Debug(String.Format("XHR call to AquaticSite Autocomplete for {0} received", query));
            string nameQuery = String.Format("%{0}%", query);

            AquaticSite[] aquaticSites = AquaticSite.SlicedFindAll(
                start, 
                limit, 
                CreateAutoCompleteCriteria(nameQuery),
                Order.Asc("AquaticSiteName")
            );

            int count = new ScalarProjectionQuery<AquaticSite, int>(
                Projections.RowCount(),
                CreateAutoCompleteCriteria(nameQuery)
            ).Execute();

            Context.Response.ContentType = "text/javascript";
            RenderText(String.Format("{{ results: {0}, aquaticSites: {1} }}", count,
                JavaScriptConvert.SerializeObject(aquaticSites)));
        }

        private DetachedCriteria CreateAutoCompleteCriteria(string nameQuery)
        {
            return DetachedCriteria.For<AquaticSite>()
                .Add(Expression.Like("AquaticSiteName", nameQuery));
        }
    }
}
