using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SJRAtlas.Core
{
    public interface IMetadata
    {
        int Id { get; }
        string Type { get; }
        string Title { get; }
        string Abstract { get; }
        string Origin { get; }
        string TimePeriod { get; }
        Uri[] Resources { get; }
    }
}
