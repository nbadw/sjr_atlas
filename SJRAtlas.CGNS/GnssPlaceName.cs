using System;
using System.Xml.Serialization;
using SJRAtlas.Core;

namespace SJRAtlas.CGNS
{
    public partial class GnssPlaceName : IPlaceName
    {
        public string ToXml()
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            Serializer.Serialize(writer, this);
            string xml = writer.ToString();
            writer.Close();
            return xml;
        }

        public static GnssPlaceName CreateFromXml(System.IO.Stream stream)
        {
            return (GnssPlaceName)Serializer.Deserialize(stream);
        }

        private static XmlSerializer Serializer
        {
            get
            {
                if (serializer == null)
                    serializer = new XmlSerializer(typeof(GnssPlaceName));

                return serializer;
            }
        }

        private static XmlSerializer serializer;

        #region IPlaceName Members

        public string Id
        {
            get { return cgndb_key; }
        }

        public string Name
        {
            get { return geoname; }
        }

        public string Region
        {
            get { return region_name; }
        }

        public string County
        {
            get { return location; }
        }

        public string Latitude
        {
            get { return latitude; }
        }

        public string Longitude
        {
            get { return longitude;  }
        }

        public double LatDec
        {
            get { return double.Parse(latdec); }
        }

        public double LongDec
        {
            get { return double.Parse(londec); }
        }

        public string StatusTerm
        {
            get { return status_term; }
        }

        public string ConciseTerm
        {
            get { return concise_term; }
        }

        public string GenericTerm
        {
            get { return generic_term; }
        }
        #endregion
    }
}
