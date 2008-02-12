using System;
using System.Collections.Generic;
using System.Text;
using SJRAtlas.Models.Atlas;
using System.Xml;

namespace Metadata.Indexer
{
    public class ItemToIndex
    {
        public ItemToIndex(string targetFile, Type targetType, XmlDocument metadataXML)
        {
            
        }

        private string targetFile;

        public string TargetFile
        {
            get { return targetFile; }
            set { targetFile = value; }
        }

        private Type targetType;

        public Type TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }

        private XmlDocument metadataXML;

        public XmlDocument MetadataXML
        {
            get { return metadataXML; }
            set { metadataXML = value; }
        }	

        public IMetadataAware CreateTargetInstance()
        {
            return null;
        }

        public SJRAtlas.Models.Atlas.Metadata CreateMetadata()
        {
            EnsureUTF16Encoding(MetadataXML);
            SJRAtlas.Models.Atlas.Metadata metadata = new SJRAtlas.Models.Atlas.Metadata();
            metadata.Filename = metadataFile;
            metadata.Content = xmlContent.OuterXml;
            return metadata;
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
    }
}
