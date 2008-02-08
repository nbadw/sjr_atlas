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
        public void List(string drainageCode, string waterbodyId, string aquaticSiteId)
        {
            IList<Agency> agencies;
            if (!String.IsNullOrEmpty(drainageCode))
            {
                agencies = Agency.FindAllByDrainageCode(drainageCode);
            }
            else if (!String.IsNullOrEmpty(waterbodyId))
            {
                agencies = Agency.FindAllByWaterBodyId(waterbodyId);
            }
            else if (!String.IsNullOrEmpty(aquaticSiteId))
            {
                agencies = Agency.FindAllByAquaticSiteId(aquaticSiteId);
            }
            else
            {
                agencies = Agency.FindAll();
            }

            CancelLayout();
            CancelView();
            Context.Response.ContentType = "text/javascript";
            RenderText(String.Format("{{ results: {0}, agencies: {1} }}", agencies.Count,
                JavaScriptConvert.SerializeObject(agencies)));
        }
    }
}
