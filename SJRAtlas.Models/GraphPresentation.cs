using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord(DiscriminatorValue = "GraphPresentation")]
    public class GraphPresentation : Presentation
    {
    }
}
