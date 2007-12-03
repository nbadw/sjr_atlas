namespace SJRAtlas.Site.Controllers
{
    using System;
    using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;
    using Castle.Core.Logging;
    using SJRAtlas.Core;
    using System.Collections.Specialized;

    [Layout("sjratlas"), Rescue("generalerror")]
    public class PlaceNameController : SJRAtlasController
    {
        //[Cache(System.Web.HttpCacheability.Server, Duration = 300, VaryByParams = "id")]
        public void View(string id)
        {
            View(PlaceNameLookup.Find(id));
        }

        [Rescue("friendlyerror", typeof(ArgumentNullException))]
        public void View(IPlaceName placename)
        {
            if (placename == null)
                throw new ArgumentNullException("placename", "The requested place name could not be found.");

            bool withinBasin = AtlasUtils.IsWithinBasin(placename);
            if (withinBasin)
            {
                if (AtlasUtils.IsWaterBody(placename))
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("placename", placename.Id);
                    Redirect("", "waterbody", "view", parameters);
                    return;
                }

                IWatershed[] watersheds = WatershedLookup.FindAllByProperty("CgndbKey", placename.Id);
                if (watersheds != null && watersheds.Length == 1)
                {
                    IWatershed watershed = watersheds[0];
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("id", watershed.DrainageCode);
                    Redirect("", "watershed", "view", parameters);
                    return;
                }
            }
            
            string metadataQuery = MetadataUtils.BuildDefaultQuery(placename.Name) + " " +
                MetadataUtils.BuildDefaultPlaceNameFilters(placename.Name);
            Logger.Debug("Searching metadata index for " + placename.Name + " using " + metadataQuery);
            IMetadata[] metadata = MetadataLookup.FindByQuery(metadataQuery);
            
            IEasyMap[] interactiveMaps;
            if (withinBasin)
                interactiveMaps = GetInteractiveMaps(metadata, true);
            else
                interactiveMaps = GetInteractiveMaps(metadata, false);

            // remove the easymaps that have been pulled from the database
            metadata = RemoveEasyMaps(metadata, interactiveMaps);

            PropertyBag["placename"] = placename;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["metadata"] = metadata;
        }
    }
}
