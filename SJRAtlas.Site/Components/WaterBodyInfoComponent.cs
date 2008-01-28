using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;

namespace SJRAtlas.Site.Components
{    
    [ViewComponentDetails("waterbody_info")]
    public class WaterBodyInfoComponent : ViewComponent
    {
        private WaterBody waterbody;

        [ViewComponentParam(Required = true)]
        public WaterBody WaterBody
        {
            get { return waterbody; }
            set { waterbody = value; }
        }

        public override void Initialize()
        {
            if (waterbody == null)
            {
                throw new ViewComponentException("The ResourceComponent requires a view component " +
                    "parameter named 'WaterBody' which should contain a 'WaterBody' instance");
            }
            base.Initialize();
        }

        public override void Render()
        {
            Place place = WaterBody.Place != null ? WaterBody.Place : WaterBody.AltPlace;
            Watershed watershed = WaterBody.Watershed;

            PropertyBag["name"] = WaterBody.Name;
            PropertyBag["coordinate"] = WaterBody.GetCoordinate();
            PropertyBag["flows_into"] = watershed.FlowsInto;
            PropertyBag["drainage_code"] = watershed.DrainageCode;
            PropertyBag["region"] = place.Region;
            PropertyBag["status_term"] = place.NameStatus;
            PropertyBag["type"] = place.GenericTerm;
            PropertyBag["county"] = place.County;
            PropertyBag["latitude"] = place.Latitude;
            PropertyBag["longitude"] = place.Longitude;
            PropertyBag["cgndb_key"] = place.CgndbKey;

            RenderSharedView("waterbody/info");
        }
    }
}
