using System;
using log4net;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using System.Xml.XPath;
using System.IO;
using System.Collections.Generic;
using SJRAtlas.Models;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;
using SJRAtlas.Models.Atlas;
using NHibernate.Expression;

namespace Atlas.Indexer
{
    public class Indexer
    {
        #region Constructor & Initialization

        public Indexer(string metadataDir)
        {
            MetadataDirectory = metadataDir;

            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(Indexer));
            Logger.Info("logging initialized...");

            Logger.Info("initializing active record framework");
            IConfigurationSource source = ActiveRecordSectionHandler.Instance;
            ActiveRecordStarter.Initialize(Assembly.Load("SJRAtlas.Models"), source);

            Logger.Info("loading rules from xml");
            Rules = LoadRules();

            Logger.Info("collecting IMetadataAware types");
            IMetadataAwareTypes = GetIMetadataAwareTypes();

            //logger.Info("removing current database entries");
            //RemoveCurrentDatabaseEntries(metadataAwareTypes);          
        }

        protected virtual Rule[] LoadRules()
        {
            List<Rule> rules = new List<Rule>();

            XPathDocument document = new XPathDocument(
                Assembly.Load("Atlas.Indexer")
                    .GetManifestResourceStream("Atlas.Indexer.Rules.xml")
            );
            XPathNavigator xpNavigator = document.CreateNavigator();
            XPathNodeIterator iterator = xpNavigator.Select("/rules/rule");
            while (iterator.MoveNext())
            {
                XPathNavigator nodeNavigator = iterator.Current.Clone();
                string fieldName = nodeNavigator.GetAttribute("field", 
                    nodeNavigator.NamespaceURI);
                string xPath = nodeNavigator.GetAttribute("xpath", 
                    nodeNavigator.NamespaceURI);
                Rule rule = new Rule(fieldName, xPath);
                rules.Add(rule);                
                if (logger.IsDebugEnabled)
                    logger.Debug(rule);
            }

            return rules.ToArray();
        }

        protected virtual Type[] GetIMetadataAwareTypes()
        {
            List<Type> types = new List<Type>();

            Type[] typesToCheck = System.Reflection.Assembly.Load("SJRAtlas.Models").GetTypes();
            foreach (Type type in typesToCheck)
            {
                if (type.GetInterface("IMetadataAware") != null && type.IsSubclassOf(typeof(ActiveRecordBase)))
                {
                    types.Add(type);
                    if (logger.IsDebugEnabled)
                        logger.Debug(type.Name);
                }
            }

            return types.ToArray();
        }

        #endregion


        #region Properties

        private ILog logger;

        public ILog Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        private Type[] iMetadataAwareTypes;

        public Type[] IMetadataAwareTypes
        {
            get { return iMetadataAwareTypes; }
            set { iMetadataAwareTypes = value; }
        }

        private string metadataDirectory;

        public string MetadataDirectory
        {
            get { return metadataDirectory; }
            set { metadataDirectory = value; }
        }

        private Rule[] rules;

        public Rule[] Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        #endregion
        
        public void Start()
        {
            logger.Info("indexing started");
            foreach (string metadataFile in GetMetadataFiles())
            {
                try
                {
                    Logger.Info("attempting to index " + Path.GetFullPath(metadataFile));
                    Index(metadataFile);
                }
                catch (Exception e)
                {
                    Logger.Info("skipping file because of error");
                    Logger.Error("could not index " + Path.GetFullPath(metadataFile), e);
                }
            }
            Logger.Info("indexing complete");
        }

        protected virtual string[] GetMetadataFiles()
        {
            return Directory.GetFiles(MetadataDirectory, "*.xml", System.IO.SearchOption.AllDirectories);
        }

        public virtual void Index(string metadataFile)
        {
            if (metadataFile == null)
                throw new ArgumentNullException("metadataFile");

            if (!File.Exists(metadataFile))
                throw new ArgumentException(String.Format("{0} does not exist", metadataFile));

            
            ItemToIndex itemToIndex = BuildItemToIndex(metadataFile);
            IMetadataAware item = itemToIndex.CreateTargetInstance(Rules);
            Metadata metadata = itemToIndex.CreateMetadata();

            Metadata existingMetadata = FindExistingMetadata(metadata);

            if(existingMetadata != null)
            {
                // update
                existingMetadata.Content = metadata.Content;
                existingMetadata.Owner = item;
                existingMetadata.UpdateAndFlush();
            }
            else
            {
                // create
                metadata.Owner = item;
                metadata.SaveAndFlush();
            }
        }

        private Metadata FindExistingMetadata(Metadata metadata)
        {
            try
            {
                DetachedCriteria criteria = DetachedCriteria.For<Metadata>();
                criteria.Add(Expression.Eq("Filename", metadata.Filename));
                return Metadata.FindOne(criteria);
            }
            catch(ActiveRecordException e)
            {
                Logger.Error("More than one existing metadata found", e);
                throw e;
            }
        }

        protected virtual ItemToIndex BuildItemToIndex(string metadataFile)
        {            
            string targetFile = FindMatchingFileForMetadata(metadataFile);
            Logger.Debug(
                String.Format(
                    "Linking file {0} to metadata file {1}",
                    targetFile,
                    metadataFile
            ));

            string possibleTypeName = GuessTypeNameFromDirectory(metadataFile);
            Type metadataAwareType = CheckForMatchingType(possibleTypeName);
            Logger.Debug(
                String.Format(
                    "IMetadataAware match found: {0} -> {1}",
                    possibleTypeName,
                    metadataAwareType.Name
            ));

            XmlDocument xmlContent = new XmlDocument();
            xmlContent.Load(metadataFile);
            if (xmlContent.DocumentElement.Name != "metadata")
                throw new Exception(
                    String.Format(
                        "The root element of {0} should be <metadata> but was <{1}>",
                        metadataFile,
                        xmlContent.DocumentElement.Name
                ));

            return new ItemToIndex(targetFile, metadataFile, metadataAwareType);
        }

        protected virtual string FindMatchingFileForMetadata(string metadataFile)
        {
            //string matchingFile = metadataFile;
            //Directory.GetFiles(metadataDirectory, "*.xml", System.IO.SearchOption.AllDirectories);
            return null;
        }

        protected virtual string GuessTypeNameFromDirectory(string metadataFile)
        {
            // based on the directory the file is in we can take a guess at it's type.  
            // for example, the following types and directory paths may be matches:
            //  - PublishedMap -> {metadata_dir}/Published_Maps/map.pdf
            //  - PublishedReport -> {metadata_dir}/Published_Reports/path/to/some/report.doc
            //  - DataSet -> {metadata_dir}/dataSets/dataset.xml
            string fullPathToFile = Path.GetFullPath(metadataFile);
            string fullPathToMetadataDir = Path.GetFullPath(MetadataDirectory);

            // file must be contained in the metadata directory
            if (!fullPathToFile.ToLower().StartsWith(fullPathToMetadataDir.ToLower()))
                throw new Exception(String.Format("{0} is outside the metadata directory {1}", metadataFile, MetadataDirectory));

            if (Path.GetDirectoryName(fullPathToFile).ToLower() == fullPathToMetadataDir.ToLower())
                throw new Exception(String.Format("Type cannot be inferred from {0} since it resides at the root of the metadata directory", metadataFile));

            string remainingPath = fullPathToFile.Substring(fullPathToMetadataDir.Length);

            if (remainingPath.StartsWith(@"\"))
                remainingPath = remainingPath.Substring(1);

            return remainingPath.Split('\\')[0];
        }

        protected virtual Type CheckForMatchingType(string possibleTypeName)
        {
            string matchTo = possibleTypeName.ToLower().Replace("_", "");
            
            foreach (Type type in IMetadataAwareTypes)
            {
                string typeName = type.Name.ToLower();
                if (typeName == matchTo || typeName + "s" == matchTo)
                    return type;                
            }

            throw new Exception(String.Format("No IMetadataAware classes could be found to match the type named {0}", possibleTypeName));
        }
        
    }
}
