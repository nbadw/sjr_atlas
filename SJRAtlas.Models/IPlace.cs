using System;
namespace SJRAtlas.Models
{
    public interface IPlace
    {
        string CgndbKey { get; set; }
        string ConciseTerm { get; set; }
        string ConciseType { get; set; }
        string CoordAccM { get; set; }
        string County { get; set; }
        string Datum { get; set; }
        string FeatureId { get; set; }
        string GenericTerm { get; set; }
        bool IsWithinBasin();
        double Latitude { get; set; }
        double Longitude { get; set; }
        string Name { get; set; }
        string NameStatus { get; set; }
        string NtsMap { get; set; }
        string Region { get; set; }
        InteractiveMap[] RelatedInteractiveMaps { get; }
        IPublication[] RelatedPublications { get; }
    }
}
