using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace SJRAtlas.Site.Helpers
{
    public class AtlasHelperConfiguration
    {
        public AtlasHelperConfiguration(string mapApplicationUrl, string defaultServiceName,
            string defaultCoordinateSystem, string defaultUnits)
        {
            this.mapApplicationUrl = mapApplicationUrl;
            this.defaultServiceName = defaultServiceName;
            this.defaultCoordinateSystem = defaultCoordinateSystem;
            this.defaultUnits = defaultUnits;
            this.linkTitles = new Dictionary<Type, string>();
            this.linkOrder = new List<Type>();
        }

        private IList<Type> linkOrder;

        public IList<Type> LinkOrder
        {
            get { return linkOrder; }
            set { linkOrder = value; }
        }
        
        private IDictionary<Type, string> linkTitles;

        public IDictionary<Type, string> LinkTitles
        {
            get { return linkTitles; }
            set { linkTitles = value; }
        }

        private string mapApplicationUrl;

        public string MapApplicationUrl
        {
            get { return mapApplicationUrl; }
            set { mapApplicationUrl = value; }
        }

        private string defaultServiceName;

        public string DefaultServiceName
        {
            get { return defaultServiceName; }
            set { defaultServiceName = value; }
        }

        private string defaultCoordinateSystem;

        public string DefaultCoordinateSystem
        {
            get { return defaultCoordinateSystem; }
            set { defaultCoordinateSystem = value; }
        }

        private string defaultUnits;

        public string DefaultUnits
        {
            get { return defaultUnits; }
            set { defaultUnits = value; }
        }
    }
}
