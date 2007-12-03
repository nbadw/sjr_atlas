using System;
using Castle.MonoRail.Framework.Helpers;
using System.Collections;
using SJRAtlas.Site.Controllers;

namespace SJRAtlas.Site.Helpers
{
    public class BreadCrumbHelper : UrlHelper
    {
        private string homeText;
        private IDictionary homeParameters;

        public BreadCrumbHelper()
        {
            homeText = "Home";
            homeParameters = new Hashtable();
            homeParameters["controller"] = "home";
            homeParameters["action"] = "index";
        }

        public string CrumbPath()
        {
            if(typeof(HomeController) == Controller.GetType())
            {
                return Link(homeText, homeParameters);
            }

            IDictionary parameters = new Hashtable();
            parameters["controller"] = Controller.Name;
            parameters["action"] = Controller.Action;
            return Link(homeText, homeParameters) + " > " +
                   Link(Capitalize(Controller.Name), parameters);
        }

        public string CrumbPath(IDictionary anchorAttributes)
        {
            return Link(homeText, homeParameters, anchorAttributes);
        }

        private string Capitalize(string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
    }
}
