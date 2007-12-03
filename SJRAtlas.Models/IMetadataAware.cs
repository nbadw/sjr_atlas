using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models
{
    public interface IMetadataAware
    {
        Metadata GetMetadata();
    }
}
