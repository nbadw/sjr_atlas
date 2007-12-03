using System;

namespace SJRAtlas.Core
{
    public interface IPlaceName
    {
        string Id { get; }
        string Name { get; }
        string Region { get; }
        string County { get; }
        string Latitude { get; }
        string Longitude { get; }
        double LatDec { get; }
        double LongDec { get; }
        string StatusTerm { get; }
        string ConciseTerm { get; }
        string GenericTerm { get; }
    }
}
