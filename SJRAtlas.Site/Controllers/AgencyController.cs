using System;
using Castle.MonoRail.Framework;
using System.Collections;
using System.Collections.Generic;
using SJRAtlas.Models.DataWarehouse;
using Newtonsoft.Json;

namespace SJRAtlas.Site.Controllers
{
    public class AgencyController : BaseController
    {
        [AjaxAction]
        public void List(string drainageCode, int waterbodyId, int aquaticSiteId)
        {
            IList<Agency> agencies = Agency.FindAllOrByShortListQuery(
                drainageCode,
                waterbodyId,
                aquaticSiteId);

            Context.Response.ContentType = "text/javascript";
            RenderText(String.Format("{{ results: {0}, agencies: {1} }}", agencies.Count,
                JavaScriptConvert.SerializeObject(agencies)));
        }
    }
}
