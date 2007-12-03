using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Core
{
    public interface IAtlasUtils
    {
        bool IsWithinBasin(IPlaceName placename);
        bool IsWithinBasin(IWatershed watershed);
        bool IsWatershed(IPlaceName placename);
        string FindDrainageCode(IPlaceName placename);
        bool IsWaterBody(IPlaceName placename);
        IWaterBody CreateWaterBodyFromPlaceName(IPlaceName placename);
        string[] DataSetTitles(IWaterBody waterbody);
        string[] DataSetTitles(IWatershed watershed);
    }
}
