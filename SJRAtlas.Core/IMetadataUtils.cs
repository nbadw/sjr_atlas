using System;
using System.IO;

namespace SJRAtlas.Core
{
    public interface IMetadataUtils
    {
        void ToPdf(Stream stream);
        void ToXml(Stream stream);
        void ToHtml(Stream stream);
        IMetadata[] FilterByType(MetadataType type, IMetadata[] metadata);
        string BuildGetByTitlesQuery(params string[] titles);
        string BuildDefaultQuery(string queryParam);
        string BuildDefaultPlaceNameFilters(string name);
    }
}
