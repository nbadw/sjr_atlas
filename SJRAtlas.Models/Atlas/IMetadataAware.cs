using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models.Atlas
{
    public interface IMetadataAware
    {
        Metadata GetMetadata();
    }
}
