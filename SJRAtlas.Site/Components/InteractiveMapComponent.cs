using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("interactive_map")]
    public class InteractiveMapComponent : ViewComponent
    {
        private InteractiveMap interactiveMap;

        [ViewComponentParam(Required = true)]
        public InteractiveMap InteractiveMap
        {
            get { return interactiveMap; }
            set { interactiveMap = value; }
        }

        private LatLngCoord coordinate;

        [ViewComponentParam(Required = false)]
        public LatLngCoord Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }	

        public override void Initialize()
        {
            if (interactiveMap == null)
            {
                throw new ViewComponentException("The InteractiveMapComponent requires a view component " +
                    "parameter named 'InteractiveMap' which should contain a 'InteractiveMap' instance");
            }
            base.Initialize();
        }

        public override void Render()
        {
            PropertyBag["interactive_map"] = InteractiveMap;
            PropertyBag["title"] = InteractiveMap.Title;
            PropertyBag["thumbnail_url"] = InteractiveMap.ThumbnailUrl;
            PropertyBag["large_thumbnail_url"] = InteractiveMap.LargeThumbnailUrl;
            PropertyBag["description"] = InteractiveMap.Abstract;
            PropertyBag["coordinate"] = Coordinate;
            RenderSharedView("interactive_map/interactive_map");
        }
    }
}
