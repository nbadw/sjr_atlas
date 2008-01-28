using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;

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
            PropertyBag["title"] = DataSet.Title;
            PropertyBag["abstract"] = DataSet.Abstract;
            PropertyBag["origin"] = DataSet.Origin;
            PropertyBag["presentations"] = DataSet.Presentations;
            RenderSharedView("data_set/data_set");
        }
    }
}
