using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord;
using System.Reflection;
using System.Xml.XPath;
using log4net.Config;
using System.IO;

namespace Metadata.Indexer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration config = new Configuration();

            InitLog4NetFramework();
            ILog logger = LogManager.GetLogger(typeof(Program)); 
            logger.Info("log4net initialized...");

            logger.Info("initializing active record framework");
            InitActiveRecordFramework();

            logger.Info("loading rules from xml");
            Rule[] rules = LoadRules(Assembly.Load("Metadata.Indexer").GetManifestResourceStream("Metadata.Indexer.Rules.xml"));
            if (logger.IsDebugEnabled)
            {
                foreach (Rule rule in rules)
                {
                    logger.Debug(rule);
                }
            }

            logger.Info("collecting IMetadataAware types");
            Type[] metadataAwareTypes = CollectMetadataAwareTypes();
            if (logger.IsDebugEnabled)
            {
                foreach (Type type in metadataAwareTypes)
                {
                    logger.Debug(type.Name);
                }
            }
            
            logger.Info("removing current database entries");
            RemoveCurrentDatabaseEntries(metadataAwareTypes);

            logger.Info("collecting metadata files");
            string[] metadataFiles = CollectMetadataFiles(config.MetadataDirectory);
            if (logger.IsDebugEnabled)
            {
                foreach (string file in metadataFiles)
                {
                    logger.Debug(file);
                }
            }

            logger.Debug("creating indexer");
            Indexer indexer = new Indexer();
            indexer.Logger = LogManager.GetLogger(typeof(Indexer));
            indexer.MetadataAwareTypes = metadataAwareTypes;
            indexer.MetadataDirectory = config.MetadataDirectory;
            indexer.Rules = rules;

            logger.Info("indexing started");
            foreach (string metadataFile in metadataFiles)
            {
                try
                {
                    logger.Info("attempting to index " + Path.GetFullPath(metadataFile));
                    indexer.Index(metadataFile);
                }
                catch (Exception e)
                {
                    logger.Info("skipping file because of error");
                    logger.Error("could not index " + Path.GetFullPath(metadataFile), e);
                }
            }
            logger.Info("indexing complete");
        }

        private static void InitActiveRecordFramework()
        {
            IConfigurationSource source = ActiveRecordSectionHandler.Instance;
            ActiveRecordStarter.Initialize(Assembly.Load("SJRAtlas.Models"), source);
        }

        private static void InitLog4NetFramework()
        {
            XmlConfigurator.Configure();
        }
        
        private static Rule[] LoadRules(Stream stream)
        {
            List<Rule> rules = new List<Rule>();

            XPathDocument document = new XPathDocument(stream);
            XPathNavigator xpNavigator = document.CreateNavigator();
            XPathNodeIterator iterator = xpNavigator.Select("/rules/rule");
            while (iterator.MoveNext())
            {
                XPathNavigator nodeNavigator = iterator.Current.Clone();
                string fieldName = nodeNavigator.GetAttribute("field", nodeNavigator.NamespaceURI);
                string xPath = nodeNavigator.GetAttribute("xpath", nodeNavigator.NamespaceURI);
                Rule rule = new Rule(fieldName, xPath);
                rules.Add(rule);
            }

            return rules.ToArray();
        }

        private static string[] CollectMetadataFiles(string metadataDirectory)
        {
            return Directory.GetFiles(metadataDirectory, "*.xml", System.IO.SearchOption.AllDirectories);
        }

        private static Type[] CollectMetadataAwareTypes()
        {
            List<Type> types = new List<Type>();

            Type[] typesToCheck = System.Reflection.Assembly.Load("SJRAtlas.Models").GetTypes();
            foreach (Type type in typesToCheck)
            {
                if (type.GetInterface("IMetadataAware") != null && type.IsSubclassOf(typeof(ActiveRecordBase)))
                {
                    types.Add(type);
                }
            }

            return types.ToArray();
        }

        private static void RemoveCurrentDatabaseEntries(Type[] metadataAwareTypes)
        {
            SJRAtlas.Models.Atlas.Metadata.DeleteAll();
            Type activeRecordType = typeof(ActiveRecordBase);
            MethodInfo deleteAll = activeRecordType.GetMethod("DeleteAll", BindingFlags.Static | BindingFlags.NonPublic, 
                null, new Type[] { typeof(Type) }, null);
            foreach (Type type in metadataAwareTypes)
            {
                deleteAll.Invoke(null, new object[] { type });
            }
        }
    }
}
