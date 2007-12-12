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
    public class PlaceController : SJRAtlasController
    {
        public void View(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            Place place = AtlasMediator.Find<Place>(id);
            if (place.IsWithinBasin())
            {
                if (AtlasMediator.WaterBodyExistsForCgndbKeyOrAltCgndbKey(place.CgndbKey))
                {                    
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("place", place.CgndbKey);
                    Redirect("", "waterbody", "view", parameters);
                    return;
                }
                                
                if (AtlasMediator.WatershedExistsForCgndbKey(place.CgndbKey))
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("place", place.CgndbKey);
                    Redirect("", "watershed", "view", parameters);
                    return;
                }
            }
                        
            IList<InteractiveMap> interactiveMaps = place.RelatedInteractiveMaps;

            if (place.IsWithinBasin())
            {
                foreach (InteractiveMap basinMap in AtlasMediator.FindAllBasinMaps())
                {
                    if (!interactiveMaps.Contains(basinMap))
                        interactiveMaps.Add(basinMap);
                }
            }

            PropertyBag["place"] = place;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["publications"] = place.RelatedPublications;
        }
    }
}
