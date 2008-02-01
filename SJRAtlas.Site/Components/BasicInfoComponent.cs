using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("basic_info")]
    public class BasicInfoComponent : ViewComponent
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
            PropertyBag["place"] = Place;
            PropertyBag["title"] = String.Format("{0}, {1} ({2} Name)", Place.Name,
                Place.Region, Place.NameStatus);
            RenderSharedView("shared/basic_info");
        }
    }
}
