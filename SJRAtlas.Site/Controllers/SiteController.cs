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
                    "Saint John River Basin", "Major Landowners", "Land Use",
                    "Topographic Map", "Lake Map", "Data Collection Sites" };
            IList<InteractiveMap> maps = AtlasMediator.FindAllInteractiveMapsByTitles(mapTitles);
            PropertyBag["interactive_maps"] = maps;
            RenderView("index");
        }

        public void About()
        {
            Logger.Debug("Site/Maps action called");
            RenderView("about");
        }

        public void Forms()
        {
            Logger.Debug("Site/Maps action called");
            RenderView("forms");
        }

        public void Reports()
        {
            Logger.Debug("Site/Maps action called");
            PropertyBag["reports"] = PublishedReport.FindAll();
            RenderView("reports");
        }

        public void Maps()
        {            
            Logger.Debug("Site/Maps action called");
            PropertyBag["interactive_maps"] = AtlasMediator.FindAll<InteractiveMap>();
            PropertyBag["published_maps"] = AtlasMediator.FindAllPublishedMaps();
            RenderView("maps");
        }
    }
}
