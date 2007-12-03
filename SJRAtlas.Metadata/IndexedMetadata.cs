using System;
using System.Collections.Generic;
using System.Text;
using SJRAtlas.Core;
using Lucene.Net.Documents;
using System.IO;
using SJRAtlas.Metadata.Properties;
using System.Text.RegularExpressions;

namespace SJRAtlas.Metadata
{
    public class IndexedMetadata : IMetadata
    {
        private int id;
        private Document doc;

        public IndexedMetadata(int id, Document doc)
        {
            this.id = id;
            this.doc = doc;
        }

        #region IMetadata Members

        public int Id
        {
            get { return id; }
        }

        public string Title
        {
            get { return doc.Get("title") != null ? doc.Get("title") : "Untitled"; }
        }

        public string Abstract
        {
            get { return doc.Get("abstract"); }
        }

        public string Type
        {
            get { return doc.Get("_type"); }
        }

        public Uri DefaultLink
        {
            get
            {
                foreach (Uri resource in Resources)
                {
                    if (Type == "Tabular Data" && IsTabularDataUri(resource))
                    {
                        return resource;
                    }
                    if (Type == "Spatial Data" && IsSpatialDataUri(resource))
                    {
                        return resource;
                    }
                    if (Type == "Maps" && IsSummaryReportUri(resource))
                    {
                        return resource;
                    }
                }

                return null;
            }
        }

        public Uri[] Resources
        {
            get 
            { 
                Field[] fields = doc.GetFields("resource");
                if (fields == null)
                    return new Uri[0];

                List<Uri> resources = new List<Uri>(fields.Length);
                foreach(Field resourceField in fields)
                {
                    Uri createdUri;
                    if(Uri.TryCreate(resourceField.StringValue(), UriKind.RelativeOrAbsolute, out createdUri))
                        resources.Add(createdUri);
                }

                return resources.ToArray();
            }
        }

        public string Origin
        {
            get { return doc.Get("origin"); }
        }

        public string TimePeriod
        {
            get { return doc.Get("pubdate"); }
        }

        #endregion

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
