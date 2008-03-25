using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;
using NHibernate.Expression;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Controllers
{    
    [DefaultAction("Index")]
    public class SiteController : BaseController
    {
        public void Index()
        {
            Logger.Debug("Site/Index action called");
            string[] mapTitles = {
                    "Saint John River Basin", "Major Landowners Map", "Land Use",
                    "Topographic Map", "Lake Depths Map", "Data Collection Sites" };
            IList<InteractiveMap> maps = AtlasMediator.FindAllInteractiveMapsByTitles(mapTitles);
            PropertyBag["interactive_maps"] = maps;
            RenderView("index");
        }

        public void About()
        {
            Logger.Debug("Site/About action called");
            RenderView("about");
        }

        public void Forms()
        {
            Logger.Debug("Site/Forms action called");
            RenderView("forms");
        }

        public void Reports()
        {
            Logger.Debug("Site/Reports action called");
            PropertyBag["published_reports"] = AtlasMediator.FindAllPublishedReports();
            RenderView("reports");
        }

        public void Maps()
        {
            Logger.Debug("Site/Maps action called");

            InteractiveMap[] maps = (InteractiveMap[])AtlasMediator.FindAll<InteractiveMap>();
            Array.Sort<InteractiveMap>(maps);
        
            PropertyBag["interactive_maps"] = maps;
            PropertyBag["published_maps"] = AtlasMediator.FindAllPublishedMaps();
            RenderView("maps");
        }
    }
}
