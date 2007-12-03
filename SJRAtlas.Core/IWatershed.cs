using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Core
{
    public interface IWatershed
    {
        string Id { get; }
        string Name { get; }
        string DrainageCode { get; }
        string TributaryOf { get; }
        string DrainsInto { get; }
        string CgndbKey { get; }
        IPlaceName PlaceName { get; set; }
    }
}
