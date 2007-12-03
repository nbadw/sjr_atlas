using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Core;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("watershed")]
    public class WatershedComponent : ViewComponent
    {
        private IWatershed watershed;

        public override void Initialize()
        {
            watershed = (IWatershed)ComponentParams["watershed"];
            base.Initialize();
        }

        public override void Render()
        {
            PropertyBag["id"] = watershed.Id;
            PropertyBag["name"] = watershed.Name;
            PropertyBag["drainage_code"] = watershed.DrainageCode;
            PropertyBag["tributary_of"] = watershed.TributaryOf;
            PropertyBag["drains_into"] = watershed.DrainsInto;
            base.Render();
        }    
    }
}
