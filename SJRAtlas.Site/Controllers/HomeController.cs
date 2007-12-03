namespace SJRAtlas.Site.Controllers
{
	using System;

	using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;
    using Castle.Core.Logging;
    using SJRAtlas.Core;

	[Layout("sjratlas"), Rescue("generalerror")]
    public class HomeController : SJRAtlasController
	{
		public void Index()
		{
            Logger.Info("Home/Index action called");

            string[] mapsToDisplay = 
                {
                    "Saint John River Basin",
                    "Major Landowners Map",
                    "Land Use",
                    "Topographic Map",
                    "Lake Depths Map",
                    "Watershed Maps"
                };
            List<IEasyMap> maps = new List<IEasyMap>(mapsToDisplay.Length);
            foreach (string title in mapsToDisplay)
            {
                IEasyMap[] results = EasyMapLookup.FindAllByProperty("Title", title);
                if (results != null && results.Length == 1)
                    maps.Add(results[0]);
            }

            IEasyMap[] easymaps = maps.ToArray();
            AttachMetadataToMaps(easymaps);
            PropertyBag["maps"] = easymaps;
		}
	}
}
