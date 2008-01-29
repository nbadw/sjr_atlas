using System;
using Castle.MonoRail.Framework.Helpers;
using SJRAtlas.Models;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using Castle.Core.Logging;

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
                config = new AtlasHelperConfiguration(String.Empty, String.Empty, String.Empty, String.Empty);
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

        public string PresentationLinks(IList<Presentation> presentations)
        {
            StringBuilder presentationLinks = new StringBuilder();
            IList<Type> presentationTypes = GetOrderedPresentationTypes(presentations);  
            foreach (Type type in presentationTypes)
            {
                AppendPresentationLink(presentationLinks, type);
            }
            return presentationLinks.ToString();
        }

        private IList<Type> GetOrderedPresentationTypes(IList<Presentation> presentations)
        {
            List<Type> allTypes = new List<Type>();
            foreach (Presentation presentation in presentations)
            {
                if (!allTypes.Contains(presentation.GetType()))
                    allTypes.Add(presentation.GetType());
            }

            if (Config.LinkOrder.Count == 0)
                return allTypes;

            List<Type> orderedTypes = new List<Type>();
            foreach (Type orderedType in Config.LinkOrder)
            {
                if (allTypes.Contains(orderedType))
                {
                    orderedTypes.Add(orderedType);
                    allTypes.Remove(orderedType);
                }
            }
            orderedTypes.AddRange(allTypes);

            return orderedTypes;
        }

        private void AppendPresentationLink(StringBuilder presentationLinks, Type type)
        {
            if (Config.LinkTitles[type] != null)
                presentationLinks.
                    AppendFormat("<span class=\"presentation-link\">{0}</span>",
                    Link(Config.LinkTitles[type], DictHelper.Create(
                        "controller=presentation",
                        "action=view"
                    )));
        }

        #endregion
    }
}
