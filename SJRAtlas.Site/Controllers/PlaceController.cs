using System;
using Castle.MonoRail.Framework;
using System.Collections.Generic;
using Castle.Core.Logging;
using System.Collections.Specialized;
using SJRAtlas.Models;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Controllers
{
    [Layout("sjratlas"), Rescue("generalerror")]
    public class PlaceController : BaseController
    {
        public void View(string cgndbKey)
        {
            if (cgndbKey == null)
                throw new ArgumentNullException("cgndbKey");

            Place place = AtlasMediator.Find<Place>(cgndbKey);

            if (place.IsWithinBasin() &&
                AtlasMediator.WaterBodyExistsForCgndbKeyOrAltCgndbKey(place.CgndbKey))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("cgndbKey", place.CgndbKey);
                Redirect("", "waterbody", "view", parameters);
                return;
            }
            
            if (place.IsWithinBasin() &&
                AtlasMediator.WatershedExistsForCgndbKey(place.CgndbKey))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("cgndbKey", place.CgndbKey);
                Redirect("", "watershed", "view", parameters);
                return;
            }

            IList<InteractiveMap> interactiveMaps = place.RelatedInteractiveMaps;
            IList<Publication> publications = place.RelatedPublications;

            PropertyBag["place"] = place;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["published_maps"] = GetPublicationsByType<PublishedMap>(publications);
            PropertyBag["published_reports"] = GetPublicationsByType<PublishedReport>(publications); ;
        }
    }
}
