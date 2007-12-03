using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Core
{
    public interface IEasyMap
    {
        int Id { get; }
        string Title { get; }
        string Description { get; }
        string Thumbnail { get; }
        string LargeThumbnail { get; }
        string ResourceName { get; }
        bool FullBasinCoverage { get; }
        IMetadata Metadata { get; set; }
    }
}
