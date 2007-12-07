using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("basic_info")]
    public class BasicInfoComponent : ViewComponent
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
            PropertyBag["is_a_watershed"] = (Place is Watershed);
            PropertyBag["is_a_waterbody"] = (Place is WaterBody);
            PropertyBag["place"] = Place;
            PropertyBag["title"] = String.Format("{0}, {1} ({2} Name)", Place.Name,
                Place.Region, Place.NameStatus);
            RenderSharedView("shared/basic_info");
        }
    }
}
