using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Metadata.Indexer
{
    public class Utils
    {
        public string GuessTypeNameFromDirectory(string metadataFile, string metadataDirectory)
        {
            // based on the directory the file is in we can take a guess at it's type.  
            // for example, the following types and directory paths may be matches:
            //  - PublishedMap -> {metadata_dir}/PublishedMaps/map.pdf
            //  - PublishedReport -> {metadata_dir}/PublishedReports/path/to/some/report.doc
            //  - DataSet -> {metadata_dir}/dataSets/dataset.xml
            string fullPathToFile = Path.GetFullPath(metadataFile);
            string fullPathToMetadataDir = Path.GetFullPath(metadataDirectory);

            // file must be contained in the metadata directory
            if (!fullPathToFile.ToLower().StartsWith(fullPathToMetadataDir.ToLower()))
                throw new Exception(String.Format("{0} is outside the metadata directory {1}", metadataFile, metadataDirectory));

            if (Path.GetDirectoryName(fullPathToFile).ToLower() == fullPathToMetadataDir.ToLower())
                throw new Exception(String.Format("Type cannot be inferred from {0} since it resides at the root of the metadata directory", metadataFile));

            string remainingPath = fullPathToFile.Substring(fullPathToMetadataDir.Length);

            if (remainingPath.StartsWith(@"\"))
                remainingPath = remainingPath.Substring(1);

            return remainingPath.Split('\\')[0];
        }

        public Type CheckForMatchingType(string possibleTypeName, Type[] metadataAwareTypes)
        {
            // possibleTypeName may be pluralized so check for that too
            foreach (Type type in metadataAwareTypes)
            {
                if (type.Name == possibleTypeName || String.Format("{0}s", type.Name) == possibleTypeName)
                    return type;
            }

            throw new Exception(String.Format("No IMetadataAware classes could be found to match the type named {0}", possibleTypeName));
        }

        public bool IsUTF16Encoded(XmlDocument xmlDocument)
        {
            // get the declaration
            XmlDeclaration declaration = null;
            if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                declaration = (XmlDeclaration)xmlDocument.FirstChild;
            }

            return declaration != null;
        }

        public void BuildInteractiveMapList()
        {
            throw new Exception("not yet implemented");
        }
    }
}
