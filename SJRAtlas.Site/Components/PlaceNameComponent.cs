using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Core;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("placename")]
    public class PlaceNameComponent : ViewComponent
    {
        private IPlaceName placename;

        public override void Initialize()
        {
            placename = (IPlaceName)ComponentParams["placename"];
            base.Initialize();
        }

        public override void Render()
        {
            PropertyBag["id"] = placename.Id;
            PropertyBag["name"] = placename.Name;
            PropertyBag["region"] = placename.Region;
            PropertyBag["latitude"] = placename.Latitude;
            PropertyBag["longitude"] = placename.Longitude;
            PropertyBag["status_term"] = placename.StatusTerm;
            PropertyBag["generic_term"] = placename.GenericTerm;
            PropertyBag["county"] = placename.County;
            PropertyBag["latdec"] = placename.LatDec;
            PropertyBag["londec"] = placename.LongDec;
            PropertyBag["view_map_resource"] = "sjriver";

            base.Render();
        }
    }
}
