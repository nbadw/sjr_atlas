using System;
using Castle.MonoRail.Framework.Helpers;
using System.Collections;
using SJRAtlas.Site.Controllers;

namespace SJRAtlas.Site.Helpers
{
    public class ResourceHelper : UrlHelper
    {
        public string Href(Uri resource)
        {
            if (resource == null)
                return "#";

            if (!resource.IsAbsoluteUri)
                resource = new Uri(Controller.Context.ApplicationPath + resource.OriginalString, UriKind.Relative);

            return resource.ToString();
        }
    }
}
