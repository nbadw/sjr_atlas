namespace SJRAtlas.Site.Controllers
{
    using System;
    using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;
    using Castle.Core.Logging;
    using System.Collections.Specialized;
    using SJRAtlas.Models;

    [Layout("sjratlas"), Rescue("generalerror")]
    public class PlaceNameController : SJRAtlasController
    {
        public void View(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            Place place = Place.Find(id);
            if (place.IsWithinBasin())
            {
                if (AtlasUtils.IsWaterBody(placename))
                {
                    WaterBody.ExistsForCgndbKeyOrAltCgndbKey(place.CgndbKey);
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("place", place.CgndbKey);
                    Redirect("", "waterbody", "view", parameters);
                    return;
                }

                Watershed.ExistsForCgndbKey(place.CgndbKey);
                if (watersheds != null && watersheds.Length == 1)
                {
                    IWatershed watershed = watersheds[0];
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("place", place.CgndbKey);
                    Redirect("", "watershed", "view", parameters);
                    return;
                }
            }

            // include basin maps
            List<InteractiveMap> interactiveMaps = place.RelatedInteractiveMaps();
            List<InteractiveMap> basinMaps = InteractiveMap.FindAllBasinMaps();
            interactiveMaps.AddRange(basinMaps);

            PropertyBag["place"] = placename;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["publications"] = place.RelatedPublications;
        }
    }
}
