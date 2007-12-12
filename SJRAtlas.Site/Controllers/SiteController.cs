using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;

namespace SJRAtlas.Site.Controllers
{
    public class SiteController : SJRAtlasController
    {
        public void Index()
        {
            string[] mapsToDisplay = {
                    "Saint John River Basin", "Major Landowners Map", "Land Use",
                    "Topographic Map", "Lake Depths Map","Watershed Maps" };

            IList<InteractiveMap> maps = new List<InteractiveMap>(mapsToDisplay.Length);
            foreach (string title in mapsToDisplay)
            {
                maps.Add(AtlasMediator.FindInteractiveMapByTitle(title));
            }

            PropertyBag["interactive_maps"] = maps;
            RenderView("index");
        }

        public void About()
        {
            RenderView("about");
        }

        public void Forms()
        {
            RenderView("forms");
        }

        public void Reports()
        {
            RenderView("maps");
        }

        public void Maps()
        {
            //Logger.Info("Maps/Index action called");
            //// Grab all the easy maps
            //IEasyMap[] maps = EasyMapLookup.FindAll();
            //IMetadata[] mapMetadata = MetadataLookup.FindByType(MetadataType.Maps);
            //AttachMetadataToMaps(maps, mapMetadata);
            //PropertyBag["maps"] = maps;
            //PropertyBag["published_maps"] = RemoveEasyMaps(mapMetadata, maps);
            RenderView("maps");
        }
    }
}
