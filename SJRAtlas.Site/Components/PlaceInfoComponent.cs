using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("place_info")]
    public class PlaceInfoComponent : ViewComponent
    {
        private IPlace place;

        [ViewComponentParam(Required = true)]
        public IPlace Place
        {
            get { return place; }
            set { place = value; }
        }

        public override void Initialize()
        {
            if (place == null)
            {
                throw new ViewComponentException("The ResourceComponent requires a view component " +
                    "parameter named 'place' which should contain an 'IPlace' instance");
            }
            base.Initialize();
        }

        public override void Render()
        {
            PropertyBag["type"] = Place.GenericTerm;
            PropertyBag["county"] = Place.County;
            PropertyBag["latitude"] = Place.Latitude;
            PropertyBag["longitude"] = Place.Longitude;
            RenderSharedView("place/info");
        }
    }
}
