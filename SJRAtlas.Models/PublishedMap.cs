using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord(DiscriminatorValue="PublishedMap")]
    public class PublishedMap : Publication
    {
    }
}
