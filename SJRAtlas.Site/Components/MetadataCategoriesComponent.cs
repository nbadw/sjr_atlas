using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Core;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("metadatacategories")]
    public class MetadataCategoriesComponent : ViewComponent
    {
        private IMetadataUtils metadataUtils;	
        private IMetadata[] metadata;

        public MetadataCategoriesComponent(IMetadataUtils metadataUtils)
        {
            this.metadataUtils = metadataUtils;
        }

        public override void Initialize()
        {
            metadata = (IMetadata[])ComponentParams["metadata"];
            if (metadata == null)
            {
                metadata = new IMetadata[0];
            }

            base.Initialize();
        }

        public override void Render()
        {
            PropertyBag["published_maps"] = MetadataUtils.FilterByType(MetadataType.Maps, metadata);
            PropertyBag["reports"] = MetadataUtils.FilterByType(MetadataType.Reports, metadata);
            PropertyBag["data"] = MetadataUtils.FilterByType(MetadataType.Data, metadata);
            PropertyBag["tabular"] = MetadataUtils.FilterByType(MetadataType.Tabular_Data, metadata);
                    
            base.Render();
        }

        public IMetadataUtils MetadataUtils
        {
            get { return metadataUtils; }
            set { metadataUtils = value; }
        }
    }
}
