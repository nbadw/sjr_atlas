using System;
using System.Collections.Generic;
using SJRAtlas.Core;
using System.Text;
using System.IO;
using SJRAtlas.Metadata.Properties;

namespace SJRAtlas.Metadata
{
    public class MetadataUtils : IMetadataUtils
    {
        #region IMetadataUtils Members

        public void ToPdf(System.IO.Stream stream)
        {
            //Prince pdf = new Prince(new Settings().PrinceCommandLineExecutable);
            ////pdf.SetLog('inetpath/public' + pdfname + ".log";
            //FileStream xmlInput = new FileStream(doc.Get("_key"), FileMode.Open);
            //bool success = pdf.Convert(xmlInput, stream);
            //if (!success)
            //    // if error...read log file...send to stream...close log file
            //    throw new Exception("There was an eror creating the PDF.");
            //// if error...read log file...send to stream...close log file
            //xmlInput.Close();
        }

        public void ToXml(System.IO.Stream stream)
        {
            //FileStream reader = new FileStream(doc.Get("_key"), FileMode.Open);
            //byte[] bytes = new byte[1024];
            //int len = 0;
            //while ((len = reader.Read(bytes, 0, bytes.Length)) != 0)
            //{
            //    stream.Write(bytes, 0, len);
            //}
            //reader.Close();
        }

        public void ToHtml(System.IO.Stream stream)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMetadata[] FilterByType(MetadataType type, IMetadata[] metadata)
        {
            List<IMetadata> matches = new List<IMetadata>();
            foreach (IMetadata item in metadata)
            {
                if (Enum.GetName(typeof(MetadataType), type).ToLower() == item.Type.ToLower().Replace(' ', '_'))
                    matches.Add(item);
            }
            return matches.ToArray();
        }

        public string BuildGetByTitlesQuery(params string[] titles)
        {
            StringBuilder query = new StringBuilder();
            foreach (string title in titles)
            {
                query.Append("title:\"" + title + "\" ");
            }            
            return query.ToString();
        }

        public string BuildDefaultQuery(string queryParam)
        {
            StringBuilder query = new StringBuilder();
            query.Append("title:\"" + queryParam + "\"^4 ");
            query.Append("keyword:\"" + queryParam + "\"^2 ");
            query.Append("abstract:\"" + queryParam + "\" ");
            query.Append("purpose:\"" + queryParam + "\" ");
            return query.ToString().TrimEnd();
        }

        public string BuildDefaultPlaceNameFilters(string name)
        {
            StringBuilder filteredQuery = new StringBuilder();
            string[] filters = { "river", "lake", "brook", "stream", "creek" };
            
            foreach (string filter in filters)
            {
                filteredQuery.Append(String.Format("-\"{0} {1}\" ", name, filter));
            }

            return filteredQuery.ToString().TrimEnd();
        }

        #endregion
    }
}
