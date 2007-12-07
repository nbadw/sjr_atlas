namespace SJRAtlas.Site.Controllers
{
	using System;

	using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;

	[Layout("sjratlas"), Rescue("generalerror")]
	public class MapsController : SJRAtlasController
	{
		public void Index(string output)
		{
            //Logger.Info("Maps/Index action called");
            //// Grab all the easy maps
            //IEasyMap[] maps = EasyMapLookup.FindAll();
            //IMetadata[] mapMetadata = MetadataLookup.FindByType(MetadataType.Maps);
            //AttachMetadataToMaps(maps, mapMetadata);
            //PropertyBag["maps"] = maps;
            //PropertyBag["published_maps"] = RemoveEasyMaps(mapMetadata, maps);
		}
	}   
}
