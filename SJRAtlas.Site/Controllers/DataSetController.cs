using System;
using Castle.MonoRail.Framework;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Controllers
{
    public class DataSetController : BaseController
    {
        [AjaxAction]
        public void List(string presentationType)
        {
            IList<DataSet> datasets;
            if (!String.IsNullOrEmpty(presentationType))
                datasets = DataSet.FindAllWithPresentation(presentationType);
            else
                datasets = DataSet.FindAll();

            Context.Response.ContentType = "text/javascript";
            RenderText(
                String.Format("{{ results: {0}, datasets: {1} }}", 
                    datasets.Count,
                    JavaScriptConvert.SerializeObject(datasets)
            ));
        }
    }
}
