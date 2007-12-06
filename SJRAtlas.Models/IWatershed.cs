using System;
using System.Collections.Generic;
namespace SJRAtlas.Models
{
    interface IWatershed
    {
        float AreaHA { get; set; }
        float AreaPercent { get; set; }
        string BorderInd { get; set; }
        string DrainageCode { get; set; }
        string DrainsInto { get; set; }
        string FlowsInto { get; }
        string Level1Name { get; set; }
        string Level1No { get; set; }
        string Level2Name { get; set; }
        string Level2No { get; set; }
        string Level3Name { get; set; }
        string Level3No { get; set; }
        string Level4Name { get; set; }
        string Level4No { get; set; }
        string Level5Name { get; set; }
        string Level5No { get; set; }
        string Level6Name { get; set; }
        string Level6No { get; set; }
        string Name { get; set; }
        int StreamOrder { get; set; }
        string TributaryOf { get; }
        string UnitType { get; set; }
        InteractiveMap[] RelatedInteractiveMaps { get; }
        IPublication[] RelatedPublications { get; }
        DataSet[] DataSets { get; }
    }
}
