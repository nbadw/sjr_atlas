using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Core
{
    public interface IMetadataLookup : ILookupService<IMetadata>
    {
        IMetadata[] FindByType(MetadataType type);
    }
}
