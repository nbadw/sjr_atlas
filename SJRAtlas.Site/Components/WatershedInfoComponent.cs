using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models.DataWarehouse;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("watershed_info")]
    public class WatershedInfoComponent : ViewComponent
    {
        private Watershed watershed;

        [ViewComponentParam(Required = true)]
        public Watershed Watershed
        {
            get { return watershed; }
            set { watershed = value; }
        }

        public override void Initialize()
        {
            if (watershed == null)
            {
                throw new ViewComponentException("The ResourceComponent requires a view component " +
                    "parameter named 'Watershed' which should contain a Watershed instance");
            }
            base.Initialize();
        }

        public override void Render()
        {
            Place place = Watershed.Place;

            PropertyBag["name"] = Watershed.Name;
            PropertyBag["coordinate"] = Watershed.GetCoordinate();
            PropertyBag["flows_into"] = Watershed.FlowsInto;
            PropertyBag["drainage_code"] = Watershed.DrainageCode;
            PropertyBag["region"] = place.Region;
            PropertyBag["status_term"] = place.NameStatus;
            PropertyBag["type"] = place.GenericTerm;
            PropertyBag["county"] = place.County;
            PropertyBag["latitude"] = place.Latitude;
            PropertyBag["longitude"] = place.Longitude;
            PropertyBag["cgndb_key"] = place.CgndbKey;

            RenderSharedView("watershed/info");
        }
    }
}
