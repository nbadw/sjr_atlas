using System;

namespace Atlas.Indexer.Console
{
    class Application
    {
        static void Main(string[] args)
        {
            Configuration config = new Configuration();
            Indexer indexer = new Indexer(config.MetadataDirectory);
            indexer.Start();
        }
    }
}
