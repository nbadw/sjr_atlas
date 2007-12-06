using System;
namespace SJRAtlas.Models
{
    interface IWaterBody
    {
        string Abbreviation { get; set; }
        string AltCgndbKey { get; set; }
        string AltName { get; set; }
        string CgndbKey { get; set; }
        int ComplexId { get; set; }
        string Datum { get; set; }
        int FlowsIntoWaterBodyId { get; set; }
        string FlowsIntoWaterBodyName { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        string SurveyedInd { get; set; }
        string Type { get; set; }
        DataSet[] DataSets { get; }
        InteractiveMap[] RelatedInteractiveMaps { get; }
        IPublication[] RelatedPublications { get; }
    }
}
