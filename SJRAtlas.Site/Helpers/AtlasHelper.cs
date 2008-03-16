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

        public string MapLink(InteractiveMap map)
        {
            return MapLink(map, null);
        }

        public string MapLink(InteractiveMap map, IDictionary anchorAttributes)
        {
            if (anchorAttributes == null)
                anchorAttributes = DictHelper.Create();

            return String.Format("<a href=\"{0}?id={1}\" {2}>{3}</a>", 
                Config.MapApplicationUrl, 
                map.Id,
                GetAttributes(anchorAttributes), 
                map.Title);
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
