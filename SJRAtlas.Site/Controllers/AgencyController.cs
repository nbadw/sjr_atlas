using System;
using Castle.MonoRail.Framework;
using System.Collections;

namespace SJRAtlas.Site.Controllers
{
    public class AgencyController : SJRAtlasController
    {
        //[AjaxAction]
        //public IDictionary List(string drainageCode, string waterbodyId, string aquaticSiteId)
        //{
        //    Hashtable results = new Hashtable();
        //    Agency[] agencies;

        //    if (!String.IsNullOrEmpty(drainageCode))
        //    {
        //        agencies = Agency.FindAllByDrainageCode(drainageCode);
        //    }
        //    else if (!String.IsNullOrEmpty(waterbodyId))
        //    {
        //        agencies = Agency.FindAllByWaterBodyId(waterbodyId);
        //    }
        //    else if (!String.IsNullOrEmpty(aquaticSiteId))
        //    {
        //        agencies = Agency.FindAllByAquaticSiteId(aquaticSiteId);
        //    }
        //    else
        //    {
        //        agencies = Agency.FindAll();
        //    }

        //    results["Agencies"] = agencies;
        //    return results;
        //}
    }
}
