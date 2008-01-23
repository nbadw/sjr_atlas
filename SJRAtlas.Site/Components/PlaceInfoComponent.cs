using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("place_info")]
    public class PlaceInfoComponent : ViewComponent
    {
        private Place place;

        [ViewComponentParam(Required = true)]
        public Place Place
        {
            get { return place; }
            set { place = value; }
        }

        public override void Initialize()
        {
            if (place == null)
            {
                throw new ViewComponentException("The ResourceComponent requires a view component " +
                    "parameter named 'place' which should contain a 'Place' instance");
            }
            base.Initialize();
        }

        public override void Render()
        {
            PropertyBag["cgndb_key"] = Place.CgndbKey;
            PropertyBag["name"] = Place.Name;
            PropertyBag["region"] = Place.Region;
            PropertyBag["status_term"] = Place.NameStatus;
            PropertyBag["coordinate"] = Place.GetCoordinate();
            PropertyBag["type"] = Place.GenericTerm;
            PropertyBag["county"] = Place.County;
            PropertyBag["latitude"] = Place.Latitude;
            PropertyBag["longitude"] = Place.Longitude;
            RenderSharedView("place/info");
        }
    }
}
