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
    public class PlaceController : BaseController
    {
        public void View(string cgndbKey)
        {
            if (cgndbKey == null)
                throw new ArgumentNullException("cgndbKey");

            Place place = AtlasMediator.Find<Place>(cgndbKey);
            if (place.IsWithinBasin())
            {
                if (AtlasMediator.WaterBodyExistsForCgndbKeyOrAltCgndbKey(place.CgndbKey))
                {                    
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("cgndbKey", place.CgndbKey);
                    Redirect("", "waterbody", "view", parameters);
                    return;
                }
                                
                if (AtlasMediator.WatershedExistsForCgndbKey(place.CgndbKey))
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("cgndbKey", place.CgndbKey);
                    Redirect("", "watershed", "view", parameters);
                    return;
                }
            }
                        
            IList<InteractiveMap> interactiveMaps = new List<InteractiveMap>(place.RelatedInteractiveMaps);

            if (place.IsWithinBasin())
            {
                foreach (InteractiveMap basinMap in AtlasMediator.FindAllBasinMaps())
                {
                    if (!interactiveMaps.Contains(basinMap))
                        interactiveMaps.Add(basinMap);
                }
            }

            IList<Publication> publications = place.RelatedPublications;
            List<PublishedMap> publishedMaps = new List<PublishedMap>();
            List<PublishedReport> publishedReports = new List<PublishedReport>();
            foreach (Publication publication in publications)
            {
                if (publication is PublishedMap)
                    publishedMaps.Add((PublishedMap)publication);
                else if (publication is PublishedReport)
                    publishedReports.Add((PublishedReport)publication);
            }

            PropertyBag["place"] = place;
            PropertyBag["interactive_maps"] = interactiveMaps;
            PropertyBag["published_maps"] = publishedMaps;
            PropertyBag["published_reports"] = publishedReports;
        }
    }
}
