using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord(DiscriminatorValue = "PublishedReport")]
    public class PublishedReport : Publication
    {
    }
}
