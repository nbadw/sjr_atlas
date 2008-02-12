using System;
using System.Collections.Generic;
using System.Text;
using SJRAtlas.Models.Atlas;
using System.Xml;
using log4net;
using System.Xml.XPath;
using System.Reflection;

namespace Atlas.Indexer
{
    public class ItemToIndex
    {
        private string targetFile;
        private string metadataFile;
        private Type targetType;
        private ILog logger;

        public ItemToIndex(string targetFile, string metadataFile, Type targetType)
        {
            this.targetFile = targetFile;
            this.metadataFile = metadataFile;
            this.targetType = targetType;
            this.logger = LogManager.GetLogger(typeof(ItemToIndex));
        }

        public IMetadataAware CreateTargetInstance(Rule[] rules)
        {
            logger.Debug("Creating instance of " + targetType.Name);
            IMetadataAware instance = (IMetadataAware)Activator.CreateInstance(targetType);
            logger.Debug("Setting properties");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(metadataFile);
            XPathNavigator navigator = xmlDocument.CreateNavigator();
            foreach (Rule rule in rules)
            {
                logger.Debug(String.Format("Processing {0}", rule));
                XPathExpression xpath = navigator.Compile(rule.Xpath);
                XPathNodeIterator iterator = navigator.Select(xpath);

                while (iterator.MoveNext())
                {
                    string property = rule.Name.Substring(0, 1).ToUpper() + rule.Name.Substring(1);
                    string value = iterator.Current.Value;
                    PropertyInfo propertyInfo = instance.GetType().GetProperty(property);

                    if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                    {
                        logger.Debug(String.Format("Attempting to set {0}.{1}={2}", targetType.Name, property, value));
                        propertyInfo.GetSetMethod().Invoke(instance, new object[] { value });
                    }
                    else
                    {
                        logger.Debug(String.Format("Could not set {0}.{1}={2} since property does not exist", targetType.Name,
                            property, value));
                    }
                }
            }
            return instance;
        }

        public virtual Metadata CreateMetadata()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(metadataFile);
            EnsureUTF16Encoding(xml);
            Metadata metadata = new Metadata();
            metadata.Filename = metadataFile;
            metadata.Content = xml.OuterXml;
            return metadata;
        }

        private void EnsureUTF16Encoding(XmlDocument xmlDocument)
        {
            // get the declaration
            XmlDeclaration declaration = null;
            if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                declaration = (XmlDeclaration)xmlDocument.FirstChild;
            }

            if (declaration == null)
            {
                declaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-16", "no");
                xmlDocument.InsertBefore(declaration, xmlDocument.DocumentElement);
            }
            else if (declaration.Encoding == "UTF-8")
            {
                declaration.Encoding = "UTF-16";
            }
        }
    }
}
