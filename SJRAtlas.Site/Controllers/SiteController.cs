using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;
using NHibernate.Expression;

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
                    "Topographic Map", "Lake Depths Map", "Watershed Maps" };
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
