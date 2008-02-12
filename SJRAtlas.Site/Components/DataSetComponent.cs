using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models.Atlas;
using SJRAtlas.Models.DataWarehouse;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("dataset")]
    public class DataSetComponent : ViewComponent
    {
        private DataSet dataset;

        [ViewComponentParam(Required = true)]
        public DataSet DataSet
        {
            get { return dataset; }
            set { dataset = value; }
        }

        private WaterBody waterbody;

        [ViewComponentParam(Required = false)]
        public WaterBody WaterBody
        {
            get { return waterbody; }
            set { waterbody = value; }
        }

        private Watershed watershed;

        [ViewComponentParam(Required = false)]
        public Watershed Watershed
        {
            get { return watershed; }
            set { watershed = value; }
        }	

	    public override void Initialize()
        {
            if (dataset == null)
            {
                throw new ViewComponentException("The ResourceComponent requires a view component " +
                    "parameter named 'DataSet' which should contain a DataSet instance");
            }
            base.Initialize();
        }

        public override void Render()
        {
            PropertyBag["dataset"] = DataSet;
            PropertyBag["title"] = DataSet.Title;
            PropertyBag["abstract"] = DataSet.Abstract;
            PropertyBag["origin"] = DataSet.Origin;
            PropertyBag["presentations"] = DataSet.Presentations;

            if (Watershed != null)
                PropertyBag["filtered_by"] = String.Format("drainageCode={0}", Watershed.DrainageCode);
            else if (WaterBody != null)
                PropertyBag["filtered_by"] = String.Format("waterbodyId={0}", WaterBody.Id);
            else
                PropertyBag["filtered_by"] = String.Empty;

            RenderSharedView("data_set/data_set");
        }
    }
}
