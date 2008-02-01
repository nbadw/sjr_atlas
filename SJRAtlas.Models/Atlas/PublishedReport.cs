using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord(DiscriminatorValue = "PublishedReport")]
    public class PublishedReport : Publication
    {
    }
}
