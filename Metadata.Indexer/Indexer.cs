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

namespace Metadata.Indexer
{
    public class Indexer
    {
        #region Properties

        private ILog logger;

        public ILog Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        private Type[] metadataAwareTypes;

        public Type[] MetadataAwareTypes
        {
            get { return metadataAwareTypes; }
            set { metadataAwareTypes = value; }
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

        public void Index(string metadataFile)
        {
            if (metadataFile == null)
                throw new ArgumentNullException("metadataFile");

            if (!File.Exists(metadataFile))
                throw new ArgumentException(String.Format("{0} does not exist", metadataFile));
            
            ItemToIndex itemToIndex = BuildItemToIndex(metadataFile);

            IMetadataAware metadataOwner = itemToIndex.CreateTargetInstance();            
            Logger.Info("IMetadataAware instance created");

            SJRAtlas.Models.Atlas.Metadata metadata = itemToIndex.CreateMetadata();
            Logger.Info("Metadata instance created");

            //metadata.Save();
        }

        public ItemToIndex BuildItemToIndex(string metadataFile)
        {
            string targetFile = FindMatchingFileForMetadata(metadataFile);
            logger.Debug(
                String.Format(
                    "Linking file {0} to metadata file {1}", 
                    targetFile, 
                    metadataFile
            ));

            string possibleTypeName = GuessTypeNameFromDirectory(metadataFile);
            Type metadataAwareType = CheckForMatchingType(possibleTypeName);
            logger.Debug(
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

            return new ItemToIndex(targetFile, metadataAwareType, xmlContent);
        }

        private string FindMatchingFileForMetadata(string metadataFile)
        {
            string matchingFile = metadataFile;
            Directory.GetFiles(metadataDirectory, "*.xml", System.IO.SearchOption.AllDirectories);
            throw new Exception("Not Implemented");
        }

        public string GuessTypeNameFromDirectory(string metadataFile)
        {
            // based on the directory the file is in we can take a guess at it's type.  
            // for example, the following types and directory paths may be matches:
            //  - PublishedMap -> {metadata_dir}/PublishedMaps/map.pdf
            //  - PublishedReport -> {metadata_dir}/PublishedReports/path/to/some/report.doc
            //  - DataSet -> {metadata_dir}/dataSets/dataset.xml
            string fullPathToFile = Path.GetFullPath(metadataFile);
            string fullPathToMetadataDir = Path.GetFullPath(MetadataDirectory);

            // file must be contained in the metadata directory
            if (!fullPathToFile.ToLower().StartsWith(fullPathToMetadataDir.ToLower()))            
                throw new Exception(String.Format("{0} is outside the metadata directory {1}", metadataFile, MetadataDirectory));

            if(Path.GetDirectoryName(fullPathToFile).ToLower() == fullPathToMetadataDir.ToLower())
                throw new Exception(String.Format("Type cannot be inferred from {0} since it resides at the root of the metadata directory", metadataFile));

            string remainingPath = fullPathToFile.Substring(fullPathToMetadataDir.Length);

            if (remainingPath.StartsWith(@"\"))
                remainingPath = remainingPath.Substring(1);

            return remainingPath.Split('\\')[0];
        }

        public Type CheckForMatchingType(string possibleTypeName)
        {
            // possibleTypeName may be pluralized so check for that too
            foreach (Type type in MetadataAwareTypes)
            {
                if (type.Name == possibleTypeName || String.Format("{0}s", type.Name) == possibleTypeName)
                    return type;
            }

            throw new Exception(String.Format("No IMetadataAware classes could be found to match the type named {0}", possibleTypeName));
        }

        public void EnsureUTF16Encoding(XmlDocument xmlDocument)
        {
            // get the declaration
            XmlDeclaration declaration = null;
            if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                declaration = (XmlDeclaration)xmlDocument.FirstChild;
            }

            if (declaration == null)
            {
                Logger.Debug("Creating XML Declaration for document");
                declaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-16", "no");
                xmlDocument.InsertBefore(declaration, xmlDocument.DocumentElement);
            }
            else if (declaration.Encoding == "UTF-8")
            {
                Logger.Debug("Document is in UTF-8, value will be changed to UTF-16");
                declaration.Encoding = "UTF-16";
            }
        }

        public IMetadataAware CreateMetadataOwner(Type metadataAwareType, XmlDocument xmlDocument)
        {
            Logger.Debug("Creating metadata owner");
            IMetadataAware metadataOwner = (IMetadataAware)Activator.CreateInstance(metadataAwareType);

            Logger.Debug("Setting object properties");
            XPathNavigator navigator = xmlDocument.CreateNavigator();
            foreach (Rule rule in Rules)
            {
                Logger.Debug(String.Format("Processing {0}", rule));
                XPathExpression xpath = navigator.Compile(rule.Xpath);
                XPathNodeIterator iterator = navigator.Select(xpath);

                while (iterator.MoveNext())
                {
                    string property = rule.Name.Substring(0, 1).ToUpper() + rule.Name.Substring(1);
                    string value = iterator.Current.Value;
                    PropertyInfo propertyInfo = metadataOwner.GetType().GetProperty(property);

                    if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    {
                        Logger.Debug(String.Format("Attempting to set {0}.{1}={2}", metadataAwareType.Name, property, value));
                        propertyInfo.GetSetMethod().Invoke(metadataOwner, new object[] { value });                        
                    }
                    else
                    {
                        Logger.Debug(String.Format("Could not set {0}.{1}={2} since property does not exist", metadataAwareType.Name,
                            property, value));
                    }
                }
            }

            return metadataOwner;
        }

        public SJRAtlas.Models.Atlas.Metadata CreateMetadata(IMetadataAware metadataOwner, string metadataFile, XmlDocument xmlContent)
        {
            Logger.Debug("Creating metadata");
            SJRAtlas.Models.Atlas.Metadata metadata = new SJRAtlas.Models.Atlas.Metadata();
            metadata.Filename = metadataFile;
            metadata.Content = xmlContent.OuterXml;
            metadata.Owner = metadataOwner;
            return metadata;
        }
    }
}
