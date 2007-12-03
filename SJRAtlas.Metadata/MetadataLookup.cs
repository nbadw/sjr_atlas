using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Analysis.Standard;
using SJRAtlas.Metadata.Properties;
using SJRAtlas.Core;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis;

namespace SJRAtlas.Metadata
{
    public class MetadataLookup : IMetadataLookup
    {
        public MetadataLookup() : this(new Settings().MetadataIndexPath)
        {

        }

        public MetadataLookup(string metadataIndexPath)
        {
            this.metadataIndexPath = metadataIndexPath;
        }

        #region ISearchService<IMetadata> Members

        public IMetadata[] FindByQuery(string query)
        {
            if (query == null || query == String.Empty)
                return new IMetadata[0];

            Directory directory = FSDirectory.GetDirectory(MetadataIndexPath, false);
            IndexSearcher isearcher = new IndexSearcher(directory);

            Query parsedQuery = new Lucene.Net.QueryParsers.QueryParser("all", new StandardAnalyzer()).Parse(query);
            Hits hits = isearcher.Search(parsedQuery);

            IMetadata[] results = new IMetadata[hits.Length()];
            for (int i = 0; i < hits.Length(); i++)
            {
                results[i] = new IndexedMetadata(hits.Id(i), hits.Doc(i));
            }

            isearcher.Close();
            directory.Close();

            return results;
        }

        public IMetadata[] FindByType(MetadataType type)
        {
            Directory directory = FSDirectory.GetDirectory(MetadataIndexPath, false);
            IndexSearcher isearcher = new IndexSearcher(directory);

            string query = Enum.GetName(typeof(MetadataType), type);
            Query parsedQuery = new Lucene.Net.QueryParsers.QueryParser("_type", new KeywordAnalyzer()).Parse(query);
            Hits hits = isearcher.Search(parsedQuery);

            IMetadata[] results = new IMetadata[hits.Length()];
            for (int i = 0; i < hits.Length(); i++)
            {
                results[i] = new IndexedMetadata(hits.Id(i), hits.Doc(i));
            }

            isearcher.Close();
            directory.Close();

            return results;
        }

        #endregion

        public string MetadataIndexPath
        {
            get { return metadataIndexPath; }
            set { metadataIndexPath = value; }
        }

        private string metadataIndexPath;

        #region ILookupService<IMetadata> Members

        public IMetadata Find(object id)
        {
            int indexId = (int)id;
            Directory directory = FSDirectory.GetDirectory(MetadataIndexPath, false);
            IndexReader index = IndexReader.Open(directory);
            IMetadata metadata = new IndexedMetadata(indexId, index.Document(indexId));
            index.Close();
            directory.Close();
            return metadata;
        }

        public IMetadata[] FindAll()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMetadata[] FindAllByProperty(string propery, object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
