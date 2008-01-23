using System;
using Castle.MonoRail.Framework.Helpers;
using SJRAtlas.Models;
using System.Collections;
using System.Configuration;

namespace SJRAtlas.Site.Helpers
{
    public class MapHelper : AbstractHelper
    {
        public MapHelper()
        {
            mapApplicationUrl = ConfigurationManager.AppSettings["MapApplicationUrl"];
            defaultServiceName = ConfigurationManager.AppSettings["ViewLocationServiceName"];
            cgnsCoordinateSystem = ConfigurationManager.AppSettings["CgnsCoordinateSystem"];
            cgnsUnits = ConfigurationManager.AppSettings["CgnsUnits"];
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

        private string cgnsCoordinateSystem;

        public string CgnsCoordinateSystem
        {
            get { return cgnsCoordinateSystem; }
            set { cgnsCoordinateSystem = value; }
        }

        private string cgnsUnits;

        public string CgnsUnits
        {
            get { return cgnsUnits; }
            set { cgnsUnits = value; }
        }
                
        public string ApplicationLink(string innerContent, IDictionary parameters)
        {
            return ApplicationLink(innerContent, DefaultServiceName, parameters);            
        }

        public string ApplicationLink(string innerContent, IDictionary parameters, IDictionary anchorAttributes)
        {
            return ApplicationLink(innerContent, DefaultServiceName, parameters, anchorAttributes);
        }

        public string ApplicationLink(string innerContent, string serviceName, IDictionary parameters)
        {
            return ApplicationLink(innerContent, serviceName, parameters, null);
        }

        public string ApplicationLink(string innerContent, string serviceName, IDictionary parameters, IDictionary anchorAttributes)
        {
            return String.Format("<a href=\"{0}\" {1}>{2}</a>", UrlFor(serviceName, parameters), GetAttributes(anchorAttributes), innerContent);
        }

        private string UrlFor(string serviceName, IDictionary parameters)
        {
            if (IsCgns(parameters["cgns"]))
            {
                parameters.Remove("cgns");
                parameters.Add("Units", CgnsUnits);
                parameters.Add("CoordSys", CgnsCoordinateSystem);
            }      
      
            LatLngCoord coordinate = GetCoordinate(parameters["coordinate"]);
            if(coordinate != null)
            {
                parameters.Remove("coordinate");
                parameters.Add("markerPoint[0]", String.Format("{0},{1}", coordinate.Latitude, coordinate.Longitude));
            }
            
            return String.Format("{0}?resource={1}&{2}", MapApplicationUrl, serviceName, BuildQueryString(parameters));
        }

        private LatLngCoord GetCoordinate(object coordinateParam)
        {
            if (coordinateParam != null && coordinateParam is LatLngCoord)
            {
                return (LatLngCoord)coordinateParam;
            }

            return null;
        }

        private bool IsCgns(object cgnsParam)
        {
            try
            {
                return bool.Parse((string)cgnsParam);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //private string UrlFor(InteractiveMap interactiveMap, LatLngCoord coordinate)
        //{
        //    return String.Format("{0}&markerPoint[0]={1},{2}", UrlFor(interactiveMap), UrlEncode(coordinate.Latitude), UrlEncode(coordinate.Longitude));
        //}

        //private string UrlFor(InteractiveMap interactiveMap, LatLngCoord coordinate, string coordinateSystem, string units)
        //{
        //    return String.Format("{0}&Units={1}&CoordSys={2}", UrlFor(interactiveMap, coordinate), UrlEncode(coordinateSystem), UrlEncode(units));
        //}
    }
}
