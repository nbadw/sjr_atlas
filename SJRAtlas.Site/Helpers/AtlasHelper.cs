using System;
using Castle.MonoRail.Framework.Helpers;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using Castle.Core.Logging;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Helpers
{
    public class AtlasHelper : UrlHelper
    {
        public AtlasHelper()
        {
            try
            {
                config = GlobalApplication.ContainerAccessor.Container.Resolve<AtlasHelperConfiguration>();
            }
            catch (Exception e)
            {
                config = new AtlasHelperConfiguration(String.Empty, String.Empty, 
                    String.Empty, String.Empty, String.Empty);
                ILogger logger = GlobalApplication.CreateLogger(GetType().FullName);
                if (logger != null)
                {
                    logger.Error("The AtlasHelperConfiguration was not properly registered, please make sure you register it in Config/components.config", e);
                    logger.Info("Using an unconfigured AtlasHelperConfiguration fallback instance");
                }
            }
        }

        private AtlasHelperConfiguration config;

        public AtlasHelperConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        #region Mapping Application Helpers        
                
        public string MapLink(string innerContent, IDictionary parameters)
        {
            return MapLink(innerContent, Config.DefaultServiceName, parameters, null);            
        }

        public string MapLink(string innerContent, IDictionary parameters, IDictionary anchorAttributes)
        {
            return MapLink(innerContent, Config.DefaultServiceName, parameters, anchorAttributes);
        }

        public string MapLink(string innerContent, string serviceName)
        {
            return MapLink(innerContent, serviceName, null, null);
        }

        public string MapLink(string innerContent, string serviceName, IDictionary parameters)
        {
            return MapLink(innerContent, serviceName, parameters, null);
        }

        public string MapLink(string innerContent, string serviceName, IDictionary parameters, IDictionary anchorAttributes)
        {
            if (parameters == null)
                parameters = DictHelper.Create();
            if (anchorAttributes == null)
                anchorAttributes = DictHelper.Create();

            return String.Format("<a href=\"{0}\" {1}>{2}</a>", MapUrlFor(serviceName, parameters), GetAttributes(anchorAttributes), innerContent);
        }

        private string MapUrlFor(string serviceName, IDictionary parameters)
        {  
            object coordinateParam = parameters["coordinate"];
            if (coordinateParam != null && coordinateParam is LatLngCoord)
            {
                LatLngCoord coordinate = (LatLngCoord)coordinateParam;
                parameters.Remove("coordinate");
                parameters.Add("markerPoint[0]", String.Format("{0},{1}", coordinate.Latitude, coordinate.Longitude));
                if (parameters["Units"] == null)
                    parameters.Add("Units", Config.DefaultUnits);
                if (parameters["CoordSys"] == null)
                    parameters.Add("CoordSys", Config.DefaultCoordinateSystem);
            }
            
            return String.Format("{0}?resource={1}&{2}", Config.MapApplicationUrl, serviceName, BuildQueryString(parameters));
        }

        #endregion

        #region Presentation Link Helpers

        public string PresentationLinks(IList<Presentation> presentations, string filteredBy)
        {
            StringBuilder presentationLinks = new StringBuilder();
            
            // XXX: haven't thought about what to do if there are multiple presentations
            //      of the same type.  We can only show 4 links at the moment so we're going
            //      to take only the first occurance of each presentation type
            Dictionary<Type, Presentation> presentationsByType = new Dictionary<Type, Presentation>();
            foreach (Presentation presentation in presentations)
            {
                if(!presentationsByType.ContainsKey(presentation.GetType()))
                    presentationsByType.Add(presentation.GetType(), presentation);
            }

            foreach (Type type in Config.LinkOrder)
            {
                if (!presentationsByType.ContainsKey(type))
                    continue;

                Presentation presentation = presentationsByType[type];
                if (Config.LinkTitles.ContainsKey(type))
                    presentationLinks.
                        AppendFormat("<span class=\"presentation-link\">{0}</span>",
                        Link(Config.LinkTitles[type], DictHelper.Create(
                            "controller=presentation",
                            "action=view",
                            String.Format("querystring=id={0}&{1}", presentation.Id, filteredBy)
                        )));
            }

            return presentationLinks.ToString();
        }

        #endregion

        public string MetadataLink(DataSet dataset)
        {
            return String.Format("<span class=\"metadata-link\">{0}</span>", Link(
                "View Metadata", 
                DictHelper.Create(
                    "controller=metadata",
                    "action=view",
                    "querystring=owner=" + dataset.Id.ToString() + 
                    "&type=" + dataset.GetType().Name
            )));
        }

        public string ContentPath()
        {
            return Config.ContentPath;
        }
    }
}
