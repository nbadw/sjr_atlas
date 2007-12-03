using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using Castle.MonoRail.Framework;
using SJRAtlas.Core;
using System.Text.RegularExpressions;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("resource")]
    public class ResourceComponent : ViewComponent
    {
        private IMetadata metadata;

        [ViewComponentParam(Required = true)]
        public IMetadata Metadata
        {
            get { return metadata; }
            set { metadata = value; }
        }

        public override void Initialize()
        {
            if (metadata == null)
            {
                throw new ViewComponentException("The ResourceComponent requires a view component " +
                    "parameter named 'metadata' which should contain 'IMetadata' instance");
            }
            PropertyBag["tabular_data_link"] = null;
            PropertyBag["spatial_data_link"] = null;
            PropertyBag["summary_report_link"] = null;
            PropertyBag["graph_link"] = null;
        }

        public override void Render()
        {
            if (Metadata.Resources.Length == 0)
                return;

            foreach (Uri resource in Metadata.Resources)
            {
                if (IsTabularDataUri(resource))
                    PropertyBag["tabular_data_link"] = resource;
                else if (IsSpatialDataUri(resource))
                    PropertyBag["spatial_data_link"] = resource;
                else if (IsSummaryReportUri(resource))
                    PropertyBag["summary_report_link"] = resource;
                else if (IsGraphUri(resource))      
                    PropertyBag["graph_link"] = resource;
            }

            base.Render();
        }

        private bool IsGraphUri(Uri resource)
        {
            Regex regex = new Regex(@"/resource/graph.rails[?]id=\d+", RegexOptions.IgnoreCase);
            return regex.IsMatch(resource.OriginalString);
        }

        private bool IsSummaryReportUri(Uri resource)
        {
            Regex regex = new Regex(@"/resource/summary.rails[?]report=.+", RegexOptions.IgnoreCase);
            return regex.IsMatch(resource.OriginalString);
        }

        private bool IsSpatialDataUri(Uri resource)
        {
            Regex regex = new Regex(@"/resource/spatial.rails[?]map=.+", RegexOptions.IgnoreCase);
            return regex.IsMatch(resource.OriginalString);
        }

        private bool IsTabularDataUri(Uri resource)
        {
            Regex regex = new Regex(@"/resource/tabular.rails[?]id=\d+", RegexOptions.IgnoreCase);
            return regex.IsMatch(resource.OriginalString);
        }
    }
}
